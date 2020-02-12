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
    public class TeamController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public TeamController(
            IRepositoryWrapper db,
            ILoggerManager logger,
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("teams")]
        public ActionResult<IEnumerable<TeamDto>> GetProjectTeams(Guid personId, Guid projectId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

                var teams = _db.Team
                    .FindByCondition(x => x.ProjectId == projectId)
                    .ToList();

                return Ok(_mapper.Map<IEnumerable<TeamDto>>(teams));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetProjectTeams: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("teams")]
        public ActionResult<TeamDto> CreateTeam(Guid personId, Guid projectId, [FromBody] CreateTeamDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

                var team = _mapper.Map<Team>(dto);
                team.ProjectId = projectId;

                _db.Team.Create(team);
                _db.Save();

                return Ok(_mapper.Map<TeamDto>(team));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in CreateTeam: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("teams/{teamId}")]
        public ActionResult<TeamDto> UpdateTeam(Guid personId, Guid projectId, Guid teamId, [FromBody] UpdateTeamDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

                var team = _db.Team
                    .FindByCondition(x => x.Id == teamId && x.ProjectId == projectId)
                    .SingleOrDefault();
                if (team == null) return BadRequest();

                team.Name = dto.Name;
                team.Description = dto.Description;
                team.HelpLink = dto.HelpLink;

                _db.Team.Update(team);
                _db.Save();

                return Ok(_mapper.Map<TeamDto>(team));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateTeam: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("teams/{teamId}")]
        public IActionResult DeleteTeam(Guid personId, Guid projectId, Guid teamId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

                var team = _db.Team
                    .FindByCondition(x => x.Id == teamId && x.ProjectId == projectId)
                    .SingleOrDefault();

                _db.Team.Delete(team);
                _db.Save();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in DeleteTeam: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
