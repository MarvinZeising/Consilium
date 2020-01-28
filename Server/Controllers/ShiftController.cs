using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Enums;
using Entities.Models;
using Entities.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/persons/{personId}/projects/{projectId}")]
    public class ShiftController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ShiftController(
            IRepositoryWrapper db,
            ILoggerManager logger,
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("shifts/{from}-{to}")]
        public ActionResult<IEnumerable<ShiftDto>> GetProjectShifts(Guid personId, Guid projectId, [ValidDate] int from, [ValidDate] int to)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();

                var role = _db.Participation.GetRole(personId, projectId);
                if (role?.CalendarRead != true) return Forbid();

                var categoryIds = _db.Category
                    .FindByCondition(x => x.ProjectId == projectId)
                    .Include(x => x.Eligibilities)
                    .Where(x => x.Eligibilities.Any(e => e.RoleId == role.Id && e.ShiftsRead))
                    .Select(x => x.Id);

                var shifts = _db.Shift
                    .FindByCondition(x =>
                        categoryIds.Contains(x.CategoryId) &&
                        x.Date >= from &&
                        x.Date <= to)
                    .Include(x => x.Category)
                    .Include(x => x.Applications).ThenInclude(x => x.Person).ThenInclude(x => x.Congregation)
                    .Include(x => x.Attendees).ThenInclude(x => x.Team)
                    .Include(x => x.Attendees).ThenInclude(x => x.Person).ThenInclude(x => x.Congregation)
                    .ToList();

                return Ok(_mapper.Map<IEnumerable<ShiftDto>>(shifts));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetProjectShifts: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("shifts")]
        public ActionResult<ShiftDto> CreateShift(Guid personId, Guid projectId, [FromBody] CreateShiftDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();
                if (_db.Participation.GetEligibilityByCategory(personId, projectId, dto.CategoryId)?.ShiftsWrite != true) return Forbid();

                var category = _db.Category
                    .FindByCondition(x => x.Id == dto.CategoryId && x.ProjectId == projectId)
                    .SingleOrDefault();
                if (category == null) return BadRequest();

                var shift = _mapper.Map<Shift>(dto);

                _db.Shift.Create(shift);
                _db.Save();

                return Ok(_mapper.Map<ShiftDto>(shift));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in CreateShift: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("shifts/{shiftId}")]
        public ActionResult<ShiftDto> UpdateShift(Guid personId, Guid projectId, Guid shiftId, [FromBody] UpdateShiftDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();
                if (_db.Participation.GetEligibilityByCategory(personId, projectId, dto.CategoryId)?.ShiftsWrite != true) return Forbid();

                var shift = _db.Shift
                    .FindByCondition(x => x.Id == shiftId)
                    .SingleOrDefault();
                if (shift == null) return BadRequest();
                if (dto.CategoryId != shift.CategoryId) {
                    if (_db.Participation.GetEligibilityByCategory(personId, projectId, shift.CategoryId)?.ShiftsWrite != true) return Forbid();
                }

                shift.CategoryId = dto.CategoryId;
                shift.Date = dto.Date;
                shift.Time = dto.Time;
                shift.Duration = dto.Duration;

                _db.Shift.Update(shift);
                _db.Save();

                var updatedShift = _db.Shift.GetFullShift(shiftId);
                return Ok(_mapper.Map<ShiftDto>(updatedShift));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateShift: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("shifts/{shiftId}/plan")]
        public ActionResult<ShiftDto> PlanShift(Guid personId, Guid projectId, Guid shiftId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

                var shift = _db.Shift
                    .FindByCondition(x => x.Id == shiftId && x.Status.Equals(ShiftStatus.Draft.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    .SingleOrDefault();
                if (shift == null) return BadRequest();
                if (_db.Participation.GetEligibilityByCategory(personId, projectId, shift.CategoryId)?.ShiftsWrite != true) return Forbid();

                shift.Status = ShiftStatus.Pending.ToString();

                _db.Shift.Update(shift);
                _db.Save();

                var updatedShift = _db.Shift.GetFullShift(shiftId);
                return Ok(_mapper.Map<ShiftDto>(updatedShift));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in PlanShift: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("shifts/{shiftId}/assignments")]
        public ActionResult<ShiftDto> MakeAssignment(Guid personId, Guid projectId, Guid shiftId, [FromBody] MakeAssignmentDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

                var shift = _db.Shift
                    .FindByCondition(x => x.Id == shiftId)
                    .Include(x => x.Applications)
                    .Include(x => x.Attendees)
                    .SingleOrDefault();
                if (shift == null) return BadRequest();
                if (_db.Participation.GetEligibilityByCategory(personId, projectId, shift.CategoryId)?.ShiftsWrite != true) return Forbid();

                foreach (var attendee in shift.Attendees)
                {
                    _db.Attendee.Delete(attendee);
                }

                foreach (var createAttendeeDto in dto.Attendees)
                {
                    var attendee = _mapper.Map<Attendee>(createAttendeeDto);
                    attendee.ShiftId = shiftId;

                    if (!shift.Applications.Any(x => x.PersonId == attendee.PersonId)) return Forbid();
                    // TODO: check teamId

                    _db.Attendee.Create(attendee);
                }

                _db.Save();

                var updatedShift = _db.Shift.GetFullShift(shiftId);
                return Ok(_mapper.Map<ShiftDto>(updatedShift));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateShift: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("shifts/{shiftId}")]
        public IActionResult DeleteShift(Guid personId, Guid projectId, Guid shiftId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

                var shift = _db.Shift.FindByCondition(x => x.Id == shiftId).SingleOrDefault();
                if (_db.Participation.GetEligibilityByCategory(personId, projectId, shift.CategoryId)?.ShiftsWrite != true) return Forbid();

                _db.Shift.Delete(shift);
                _db.Save();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in DeleteShift: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
