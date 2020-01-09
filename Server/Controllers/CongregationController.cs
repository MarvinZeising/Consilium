using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/persons/{personId}/projects/{projectId}")]
    public class CongregationController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CongregationController(
            IRepositoryWrapper db,
            ILoggerManager logger,
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("congregations")]
        public ActionResult<IEnumerable<CongregationDto>> GetCongregations(Guid personId, Guid projectId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.ParticipantsWrite != true) return Forbid();
                // TODO: check if project is valid / paid for

                var congregations = _db.Congregation
                    .FindAll()
                    .Include(x => x.Persons)
                    .ToList()
                    .Select((x) =>
                    {
                        var congregation = _mapper.Map<CongregationDto>(x);
                        congregation.NumberOfParticipants = x.Persons.Count;
                        return congregation;
                    });

                return Ok(congregations);
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetCongregations: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
