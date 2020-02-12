using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Entities.DataTransferObjects;
using Server.Entities.Models;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/persons/{personId}/projects/{projectId}")]
    public class RoleController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public RoleController(
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
                    .Include(x => x.Eligibilities).ThenInclude(x => x.Category)
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

                foreach (var eligibility in role.Eligibilities)
                {
                    var category = _db.Category
                        .FindByCondition(x => x.Id == eligibility.CategoryId && x.ProjectId == projectId)
                        .SingleOrDefault();
                    if (category == null) return BadRequest();

                    eligibility.ShiftsWrite = eligibility.ShiftsWrite && eligibility.ShiftsRead;
                    eligibility.IsTeamCaptain = eligibility.IsTeamCaptain && eligibility.ShiftsRead;
                    eligibility.IsSubstituteCaptain = eligibility.IsSubstituteCaptain && eligibility.ShiftsRead && !eligibility.IsTeamCaptain;
                }

                // TODO: create the eligibility for administrator manually with full permissions

                _db.Role.Create(role);
                _db.Save();

                var createdRole = _db.Role
                    .FindByCondition(x => x.Id == role.Id && x.ProjectId == projectId)
                    .Include(x => x.Eligibilities).ThenInclude(x => x.Category)
                    .SingleOrDefault();

                return Ok(_mapper.Map<RoleDto>(createdRole));
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

                var roleFromDb = _db.Role
                    .FindByCondition(x => x.Id == roleId && x.ProjectId == projectId)
                    .SingleOrDefault();

                if (roleFromDb?.Editable == true)
                {
                    role = _mapper.Map<Role>(dto);
                    role.Editable = true;
                    role.CreatedTime = roleFromDb.CreatedTime;
                }
                else
                {
                    role = roleFromDb;
                    role.Name = dto.Name;
                    role.Editable = false;
                }

                role.Id = roleId;
                role.ProjectId = projectId;
                role.Eligibilities = new List<Eligibility>();

                _db.Role.Update(role);

                foreach (var eligibility in dto.Eligibilities)
                {
                    var eligibilityFromDb = _db.Eligibility
                        .FindByCondition(x => x.Id == eligibility.Id && x.RoleId == roleId)
                        .SingleOrDefault();

                    if (eligibilityFromDb != null)
                    {
                        eligibilityFromDb.ShiftsRead = eligibility.ShiftsRead;
                        eligibilityFromDb.ShiftsWrite = eligibility.ShiftsWrite && eligibility.ShiftsRead;
                        eligibilityFromDb.IsTeamCaptain = eligibility.IsTeamCaptain && eligibility.ShiftsRead;
                        eligibilityFromDb.IsSubstituteCaptain = eligibility.IsSubstituteCaptain && eligibility.ShiftsRead && !eligibility.IsTeamCaptain;

                        _db.Eligibility.Update(eligibilityFromDb);
                    }
                }

                _db.Save();

                var updatedRole = _db.Role
                    .FindByCondition(x => x.Id == roleId && x.ProjectId == projectId)
                    .Include(x => x.Eligibilities).ThenInclude(x => x.Category)
                    .SingleOrDefault();

                return Ok(_mapper.Map<RoleDto>(updatedRole));
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

                var role = _db.Role
                    .FindByCondition(x => x.Id == roleId && x.ProjectId == projectId)
                    .SingleOrDefault();

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
