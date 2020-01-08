using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/persons/{personId}/projects/{projectId}")]
    public class RolesController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public RolesController(
            IRepositoryWrapper db,
            ILoggerManager logger,
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("roles")]
        public ActionResult<IEnumerable<RoleDto>> GetProjectRoles(Guid personId, Guid projectId)
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
        public ActionResult<RoleDto> CreateRole(Guid personId, Guid projectId, [FromBody] CreateRoleDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.RolesWrite != true) return Forbid();

                var role = _mapper.Map<Role>(dto);
                role.ProjectId = projectId;
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

        [HttpPut("roles/{roleId}")]
        public ActionResult<RoleDto> UpdateRole(Guid personId, Guid projectId, Guid roleId, [FromBody] UpdateRoleDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.RolesWrite != true) return Forbid();

                Role role;

                var roleFromDb = _db.Role.FindByCondition(x => x.Id == roleId).SingleOrDefault();
                if (roleFromDb?.Editable == true)
                {
                    role = _mapper.Map<Role>(dto);
                    role.Editable = true;
                }
                else
                {
                    role = roleFromDb;
                    role.Name = dto.Name;
                    role.Editable = false;
                }

                role.Id = roleId;
                role.ProjectId = projectId;

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

        [HttpDelete("roles/{roleId}")]
        public IActionResult DeleteRole(Guid personId, Guid projectId, Guid roleId)
        {
            try
            {
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

    }
}
