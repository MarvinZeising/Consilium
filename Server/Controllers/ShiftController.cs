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
    [Route("api/persons/{personId}/projects/{projectId}/categories/{categoryId}")]
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

        [HttpGet("shifts")]
        public ActionResult<IEnumerable<ShiftDto>> GetProjectShifts(Guid personId, Guid projectId, Guid categoryId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarRead != true) return Forbid();
                // TODO: check category permission

                var category = _db.Category
                    .FindByCondition(x => x.Id == categoryId && x.ProjectId == projectId)
                    .SingleOrDefault();
                if (category == null) return BadRequest();

                var shifts = _db.Shift
                    .FindByCondition(x => x.CategoryId == categoryId) // TODO: only for one month
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
        public ActionResult<ShiftDto> CreateShift(Guid personId, Guid projectId, Guid categoryId, [FromBody] CreateShiftDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();
                // TODO: check category permission

                var category = _db.Category
                    .FindByCondition(x => x.Id == categoryId && x.ProjectId == projectId)
                    .SingleOrDefault();
                if (category == null) return BadRequest();

                var shift = _mapper.Map<Shift>(dto);
                shift.CategoryId = categoryId;

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

    }
}
