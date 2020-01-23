using System;
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
    [Route("api/persons/{personId}/projects/{projectId}/shifts/{shiftId}")]
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

        [HttpPost("applications")]
        public ActionResult<ApplicationDto> CreateApplication(Guid personId, Guid projectId, Guid shiftId, [FromBody] CreateApplicationDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarRead != true) return Forbid();

                var shift = _db.Shift
                    .FindByCondition(x => x.Id == shiftId)
                    .Include(x => x.Category)
                    .SingleOrDefault(x => x.Category.ProjectId == projectId);
                if (shift == null) return BadRequest();
                if (_db.Participation.GetEligibilityByCategory(personId, projectId, shift.CategoryId)?.ShiftsRead != true) return Forbid();

                var application = _mapper.Map<Application>(dto);
                application.PersonId = personId;
                application.ShiftId = shiftId;

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

        [HttpDelete("applications/{applicationId}")]
        public IActionResult DeleteApplication(Guid personId, Guid projectId, Guid shiftId, Guid applicationId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarRead != true) return Forbid();

                var application = _db.Application
                    .FindByCondition(x => x.Id == applicationId && x.PersonId == personId)
                    .Include(x => x.Shift).ThenInclude(x => x.Category)
                    .SingleOrDefault(x => x.Shift.Category.ProjectId == projectId);
                if (application == null) return BadRequest();

                if (_db.Participation.GetEligibilityByCategory(personId, projectId, application.Shift.CategoryId)?.ShiftsRead != true) return Forbid();

                _db.Application.Delete(application);
                _db.Save();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in DeleteApplication: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
