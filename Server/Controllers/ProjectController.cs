using System.Linq;
using System;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Entities.Enums;
using System.Collections.Generic;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ProjectController(
            IRepositoryWrapper db,
            ILoggerManager logger,
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{projectId}/{personId}")]
        public ActionResult<ProjectDto> GetProject(Guid projectId, Guid personId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetParticipation(personId, projectId) == null) return Forbid();

                var project = _db.Project
                    .FindByCondition(x => x.Id == projectId)
                    .SingleOrDefault();

                return Ok(_mapper.Map<ProjectDto>(project));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetProject: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{projectId}/{personId}/roles")]
        public ActionResult<IEnumerable<RoleDto>> GetProjectRoles(Guid projectId, Guid personId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.RolesRead != true) return Forbid();

                var roles = _db.Role
                    .FindByCondition(x => x.ProjectId == projectId)
                    .ToList();

                return Ok(_mapper.Map<IEnumerable<RoleDto>>(roles));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetProjectRoles: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("roles")]
        public IActionResult CreateRole([FromBody] CreateRoleDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(dto.PersonId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(dto.PersonId, dto.ProjectId)?.RolesWrite != true) return Forbid();

                var role = _mapper.Map<Role>(dto);
                role.Editable = true;

                _db.Role.Create(role);
                _db.Save();

                return Ok(_mapper.Map<RoleDto>(role));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in CreateRole: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("roles")]
        public IActionResult UpdateRole([FromBody] UpdateRoleDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(dto.PersonId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(dto.PersonId, dto.ProjectId)?.RolesWrite != true) return Forbid();

                Role role;

                var roleFromDb = _db.Role.FindByCondition(x => x.Id == dto.RoleId).SingleOrDefault();
                if (roleFromDb?.Editable == true)
                {
                    role = _mapper.Map<Role>(dto);
                    role.Id = dto.RoleId;
                    role.Editable = true;
                }
                else
                {
                    role = roleFromDb;
                    role.Id = dto.RoleId;
                    role.Name = dto.Name;
                    role.Editable = false;
                }

                _db.Role.Update(role);
                _db.Save();

                return Ok(_mapper.Map<RoleDto>(role));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateRole: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{projectId}/{personId}/roles/{roleId}")]
        public IActionResult DeleteRole(Guid projectId, Guid personId, Guid roleId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.RolesWrite != true) return Forbid();

                var role = _db.Role.FindByCondition(x => x.Id == roleId).SingleOrDefault();
                if (role?.Editable != true)
                {
                    return BadRequest();
                }
                else
                {
                    _db.Role.Delete(role);
                    _db.Save();
                }

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in DeleteRole: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{projectId}/{personId}/participations")]
        public ActionResult<IEnumerable<ParticipationDto>> GetProjectParticipations(Guid projectId, Guid personId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.ParticipantsRead != true) return Forbid();

                var participations = _db.Participation
                    .FindByCondition(x => x.ProjectId == projectId)
                    .ToList();

                return Ok(_mapper.Map<IEnumerable<ParticipationDto>>(participations));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetProjectParticipations: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateProject([FromBody] CreateProjectDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(dto.PersonId, HttpContext)) return Forbid();

                var person = _db.Person.FindByCondition(x => x.Id == dto.PersonId).SingleOrDefault();
                if (person == null) return BadRequest();

                var project = _mapper.Map<Project>(dto);
                _db.Project.Create(project);

                var role = new Role
                {
                    ProjectId = project.Id,
                    Name = "Administrator",
                    KnowledgeBaseRead = true,
                    KnowledgeBaseWrite = true,
                    ParticipantsRead = true,
                    ParticipantsWrite = true,
                    RolesRead = true,
                    RolesWrite = true,
                    SettingsRead = true,
                    SettingsWrite = true,
                    Editable = false,
                };
                _db.Role.Create(role);

                var participation = new Participation
                {
                    ProjectId = project.Id,
                    PersonId = dto.PersonId,
                    RoleId = role.Id,
                    Status = nameof(ParticipationStatus.Active),
                };
                _db.Participation.Create(participation);

                _db.Save();

                return Ok(_mapper.Map<ProjectDto>(project));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in CreateProject: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{projectId}")]
        public IActionResult UpdateProjectGeneral(Guid projectId, [FromBody] UpdateProjectGeneralDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(dto.PersonId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(dto.PersonId, projectId)?.SettingsWrite != true) return Forbid();

                var project = _db.Project.GetById(projectId);

                project.Name = dto.Name;
                project.Email = dto.Email;

                _db.Project.Update(project);
                _db.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateProjectGeneral: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{projectId}/{personId}")]
        public IActionResult DeleteProject(Guid projectId, Guid personId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.SettingsWrite != true) return Forbid();

                // TODO: delete topics
                // TODO: delete articles
                // TODO: delete shifts

                var participations = _db.Participation.FindByCondition(x => x.ProjectId == projectId).ToList();
                _db.Participation.Delete(participations);

                var roles = _db.Role.FindByCondition(x => x.ProjectId == projectId).ToList();
                _db.Role.Delete(roles);

                _db.Project.Delete(new Project { Id = projectId });
                _db.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in DeleteProject: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
