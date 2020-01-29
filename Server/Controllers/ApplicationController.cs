using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/persons/{personId}")]
    public class ApplicationController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ApplicationController(
            IRepositoryWrapper db,
            ILoggerManager logger,
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("applications")]
        public ActionResult<IEnumerable<ApplicationDto>> GetApplications(Guid personId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();

                var applications = _db.Application
                    .FindByCondition(x => x.PersonId == personId)
                    .Include(x => x.Shift).ThenInclude(x => x.Category).ThenInclude(x => x.Project)
                    .ToList();

                return Ok(_mapper.Map<IEnumerable<ApplicationDto>>(applications));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetApplications: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("applications")]
        public ActionResult<ApplicationDto> CreateApplication(Guid personId, [FromBody] CreateApplicationDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();

                var shift = _db.Shift
                    .FindByCondition(x => x.Id == dto.ShiftId)
                    .Include(x => x.Category)
                    .SingleOrDefault();
                if (shift == null) return BadRequest();
                if (_db.Participation.GetRole(personId, shift.Category.ProjectId)?.CalendarRead != true) return Forbid();
                if (_db.Participation.GetEligibilityByCategory(personId, shift.Category.ProjectId, shift.CategoryId)?.ShiftsRead != true) return Forbid();

                var application = _mapper.Map<Application>(dto);
                application.PersonId = personId;
                application.ShiftId = dto.ShiftId;

                _db.Application.Create(application);
                _db.Save();

                var createdApplication = _db.Application
                    .FindByCondition(x => x.Id == application.Id)
                    .Include(x => x.Person)
                    .Single();

                return Ok(_mapper.Map<ApplicationDto>(createdApplication));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in CreateApplication: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("shifts/{shiftId}/cancelAttendance")]
        public IActionResult CancelAttendance(Guid personId, Guid shiftId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();

                var attendee = _db.Attendee
                    .FindByCondition(x => x.ShiftId == shiftId && x.PersonId == personId)
                    .Include(x => x.Shift).ThenInclude(x => x.Category)
                    .SingleOrDefault();
                if (attendee == null) return BadRequest();

                var projectId = attendee.Shift.Category.ProjectId;
                if (_db.Participation.GetRole(personId, projectId)?.CalendarRead != true) return Forbid();
                if (_db.Participation.GetEligibilityByCategory(personId, projectId, attendee.Shift.CategoryId)?.ShiftsRead != true) return Forbid();

                _db.Attendee.Delete(attendee);
                _db.Save();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in CancelAttendance: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("shifts/{shiftId}/cancelApplication")]
        public IActionResult CancelApplication(Guid personId, Guid shiftId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();

                var application = _db.Application
                    .FindByCondition(x => x.ShiftId == shiftId && x.PersonId == personId)
                    .Include(x => x.Shift).ThenInclude(x => x.Category)
                    .SingleOrDefault();
                if (application == null) return BadRequest();

                var projectId = application.Shift.Category.ProjectId;
                if (_db.Participation.GetRole(personId, projectId)?.CalendarRead != true) return Forbid();
                if (_db.Participation.GetEligibilityByCategory(personId, projectId, application.Shift.CategoryId)?.ShiftsRead != true) return Forbid();

                _db.Application.Delete(application);
                _db.Save();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in CancelApplication: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
