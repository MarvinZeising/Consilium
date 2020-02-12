using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Entities.DataTransferObjects;
using Server.Entities.Enums;
using Server.Entities.Models;
using Server.Entities.Validators;

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

                var canEdit = role?.CalendarWrite == true;

                var categoryIds = _db.Category
                    .FindByCondition(x => x.ProjectId == projectId)
                    .Include(x => x.Eligibilities)
                    .Where(x => x.Eligibilities.Any(e => e.RoleId == role.Id && e.ShiftsRead))
                    .Select(x => x.Id);

                var shiftsFromDb = _db.Shift
                    .FindByCondition(x =>
                        categoryIds.Contains(x.CategoryId) &&
                        x.Date >= from &&
                        x.Date <= to)
                    .Include(x => x.Category)
                    .Include(x => x.Applications).ThenInclude(x => x.Person).ThenInclude(x => x.Congregation)
                    .Include(x => x.Attendees).ThenInclude(x => x.Team)
                    .Include(x => x.Attendees).ThenInclude(x => x.Person).ThenInclude(x => x.Congregation)
                    .ToList();

                var shifts = _mapper.Map<IEnumerable<ShiftDto>>(shiftsFromDb);

                if (!canEdit)
                {
                    foreach (var shift in shifts)
                    {
                        if (Enum.Parse<ShiftStatus>(shift.Status, ignoreCase : true) != ShiftStatus.Scheduled)
                        {
                            shift.Attendees = new List<AttendeeDto>();
                        }
                        else
                        {
                            shift.Applications = new List<ApplicationDto>();
                        }
                    }
                }

                foreach (var shift in shifts)
                {
                    var application = shift.Applications.SingleOrDefault(x => x.PersonId == personId);
                    if (application != null)
                    {
                        shift.IsApplicant = true;
                        shift.IsAttendee = shift.Attendees.Any(x => x.ApplicationId == application.Id);
                    }
                }

                return Ok(shifts);
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

                shift.Status = ShiftStatus.Draft.ToString();
                shift.Mode = ShiftMode.Open.ToString();
                shift.CalledOffReason = string.Empty;

                _db.Shift.Create(shift);
                _db.Save();

                return Ok(_db.Shift.GetFullShift(_mapper, shift.Id, personId));
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
                if (dto.CategoryId != shift.CategoryId)
                {
                    if (_db.Participation.GetEligibilityByCategory(personId, projectId, shift.CategoryId)?.ShiftsWrite != true) return Forbid();
                }

                shift.CategoryId = dto.CategoryId;
                shift.Date = dto.Date;
                shift.Time = dto.Time;
                shift.Duration = dto.Duration;

                _db.Shift.Update(shift);
                _db.Save();

                return Ok(_db.Shift.GetFullShift(_mapper, shiftId, personId));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateShift: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("shifts/{shiftId}/status/{status}")]
        public ActionResult<ShiftDto> UpdateShiftStatus(Guid personId, Guid projectId, Guid shiftId, string status)
        {
            try
            {
                if (!Enum.TryParse<ShiftStatus>(status, ignoreCase : true, out var shiftStatus)) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

                var shift = _db.Shift.FindByCondition(x => x.Id == shiftId).SingleOrDefault();
                if (shift == null) return BadRequest();
                if (_db.Participation.GetEligibilityByCategory(personId, projectId, shift.CategoryId)?.ShiftsWrite != true) return Forbid();

                shift.Status = status;

                _db.Shift.Update(shift);
                _db.Save();

                return Ok(_db.Shift.GetFullShift(_mapper, shiftId, personId));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateShiftStatus: {e.Message}");
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

                    var application = shift.Applications.SingleOrDefault(x => x.PersonId == attendee.PersonId);
                    if (application == null) return BadRequest();
                    attendee.ApplicationId = application.Id;

                    // TODO: check teamId

                    _db.Attendee.Create(attendee);
                }

                _db.Save();

                return Ok(_db.Shift.GetFullShift(_mapper, shiftId, personId));
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
