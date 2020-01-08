using System.Linq;
using System;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Entities.Enums;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/persons/{personId}/projects")]
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

        [HttpGet("{projectId}")]
        public ActionResult<ProjectDto> GetProject(Guid personId, Guid projectId)
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

        [HttpPost]
        public IActionResult CreateProject(Guid personId, [FromBody] CreateProjectDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();

                var person = _db.Person.FindByCondition(x => x.Id == personId).SingleOrDefault();
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
                    PersonId = personId,
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
        public IActionResult UpdateProjectGeneral(Guid personId, Guid projectId, [FromBody] UpdateProjectGeneralDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.SettingsWrite != true) return Forbid();

                var project = _db.Project.GetById(projectId);

                project.Name = dto.Name;
                project.Email = dto.Email;

                _db.Project.Update(project);
                _db.Save();

                return Ok(_mapper.Map<ProjectDto>(project));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateProjectGeneral: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{projectId}")]
        public IActionResult DeleteProject(Guid personId, Guid projectId)
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
