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
    public class ParticipationController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ParticipationController(
            IRepositoryWrapper db,
            ILoggerManager logger,
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("invitations")]
        public ActionResult<IEnumerable<ParticipationDto>> GetProjectInvitations(Guid personId, Guid projectId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.ParticipantsWrite != true) return Forbid();

                var participations = _db.Participation
                    .FindByCondition(
                        x => x.ProjectId == projectId
                        && x.Status.Equals(ParticipationStatus.Invited.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    .Include(x => x.Person)
                    .Include(x => x.Role)
                    .ToList();

                return Ok(_mapper.Map<IEnumerable<ParticipationDto>>(participations));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetProjectInvitations: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("requests")]
        public ActionResult<IEnumerable<ParticipationDto>> GetProjectRequests(Guid personId, Guid projectId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.ParticipantsWrite != true) return Forbid();

                var participations = _db.Participation
                    .FindByCondition(
                        x => x.ProjectId == projectId
                        && x.Status.Equals(ParticipationStatus.Requested.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    .Include(x => x.Person)
                    .Include(x => x.Role)
                    .ToList();

                return Ok(_mapper.Map<IEnumerable<ParticipationDto>>(participations));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetProjectRequests: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("participations")]
        public ActionResult<IEnumerable<ParticipationDto>> GetProjectParticipations(Guid personId, Guid projectId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.ParticipantsWrite != true) return Forbid();

                var participations = _db.Participation
                    .FindByCondition(x =>
                        x.ProjectId == projectId
                        && (x.Status.Equals(ParticipationStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)
                            || x.Status.Equals(ParticipationStatus.Inactive.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                    .ToList();

                return Ok(_mapper.Map<IEnumerable<ParticipationDto>>(participations));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetProjectParticipations: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("invitations")]
        public IActionResult CreateInvitation(Guid personId, Guid projectId, [FromBody] CreateInvitationDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.ParticipantsWrite != true) return Forbid();

                var participation = _mapper.Map<Participation>(dto);
                participation.ProjectId = projectId;
                participation.Status = ParticipationStatus.Invited.ToString();

                _db.Participation.Create(participation);
                _db.Save();

                return Ok(_mapper.Map<ParticipationDto>(participation));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in CreateParticipation: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }







// ? COPIED FROM ROLES

        // [HttpPut("participations/{participationId}")]
        // public IActionResult UpdateParticipation(Guid personId, Guid projectId, Guid participationId, [FromBody] UpdateParticipationDto dto)
        // {
        //     try
        //     {
        //         if (!ModelState.IsValid) return BadRequest();
        //         if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
        //         if (_db.Participation.GetRole(personId, projectId)?.ParticipantsWrite != true) return Forbid();

        //         Participation participation;

        //         var participationFromDb = _db.Participation.FindByCondition(x => x.Id == participationId).SingleOrDefault();
        //         if (participationFromDb?.Editable == true)
        //         {
        //             participation = _mapper.Map<Participation>(dto);
        //             participation.Editable = true;
        //         }
        //         else
        //         {
        //             participation = participationFromDb;
        //             participation.Name = dto.Name;
        //             participation.Editable = false;
        //         }

        //         participation.Id = participationId;
        //         participation.ProjectId = projectId;

        //         _db.Participation.Update(participation);
        //         _db.Save();

        //         return Ok(_mapper.Map<ParticipationDto>(participation));
        //     }
        //     catch (Exception e)
        //     {
        //         _logger.LogError($"ERROR in UpdateParticipation: {e.Message}");
        //         return StatusCode(500, "Internal server error");
        //     }
        // }

        // [HttpDelete("participations/{participationId}")]
        // public IActionResult DeleteParticipation(Guid personId, Guid projectId, Guid participationId)
        // {
        //     try
        //     {
        //         if (!ModelState.IsValid) return BadRequest();
        //         if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
        //         if (_db.Participation.GetRole(personId, projectId)?.ParticipantsWrite != true) return Forbid();

        //         var participation = _db.Participation.FindByCondition(x => x.Id == participationId).SingleOrDefault();

        //         _db.Participation.Delete(participation);
        //         _db.Save();

        //         return Ok();
        //     }
        //     catch (Exception e)
        //     {
        //         _logger.LogError($"ERROR in DeleteParticipation: {e.Message}");
        //         return StatusCode(500, "Internal server error");
        //     }
        // }

    }
}
