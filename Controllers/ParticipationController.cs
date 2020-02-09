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

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route ("api/persons/{personId}/projects/{projectId}")]
    public class ParticipationController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ParticipationController (
            IRepositoryWrapper db,
            ILoggerManager logger,
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet ("invitations")]
        public ActionResult<IEnumerable<ParticipationDto>> GetProjectInvitations (Guid personId, Guid projectId)
        {
            try
            {
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();
                if (_db.Participation.GetRole (personId, projectId)?.ParticipantsWrite != true)return Forbid ();

                var participations = _db.Participation
                    .FindByCondition (
                        x => x.ProjectId == projectId &&
                        x.Status.Equals (ParticipationStatus.Invited.ToString (), StringComparison.CurrentCultureIgnoreCase))
                    .Include (x => x.Person)
                    .Include (x => x.Role)
                    .ToList ();

                return Ok (_mapper.Map<IEnumerable<ParticipationDto>> (participations));
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in GetProjectInvitations: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpGet ("requests")]
        public ActionResult<IEnumerable<ParticipationDto>> GetProjectRequests (Guid personId, Guid projectId)
        {
            try
            {
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();
                if (_db.Participation.GetRole (personId, projectId)?.ParticipantsWrite != true)return Forbid ();

                var participations = _db.Participation
                    .FindByCondition (
                        x => x.ProjectId == projectId &&
                        x.Status.Equals (ParticipationStatus.Requested.ToString (), StringComparison.CurrentCultureIgnoreCase))
                    .Include (x => x.Person)
                    .Include (x => x.Role)
                    .ToList ();

                return Ok (_mapper.Map<IEnumerable<ParticipationDto>> (participations));
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in GetProjectRequests: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpGet ("participations")]
        public ActionResult<IEnumerable<ParticipationDto>> GetProjectParticipations (Guid personId, Guid projectId)
        {
            try
            {
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();
                if (_db.Participation.GetRole (personId, projectId)?.ParticipantsWrite != true)return Forbid ();

                var participations = _db.Participation
                    .FindByCondition (x =>
                        x.ProjectId == projectId &&
                        (x.Status.Equals (ParticipationStatus.Active.ToString (), StringComparison.CurrentCultureIgnoreCase) ||
                            x.Status.Equals (ParticipationStatus.Inactive.ToString (), StringComparison.CurrentCultureIgnoreCase)))
                    .Include (x => x.Person)
                    .Include (x => x.Role)
                    .ToList ();

                return Ok (_mapper.Map<IEnumerable<ParticipationDto>> (participations));
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in GetProjectParticipations: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpPost ("invitations")]
        public ActionResult<ParticipationDto> CreateInvitation (Guid personId, Guid projectId, [FromBody] CreateInvitationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)return BadRequest ();
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();
                if (_db.Participation.GetRole (personId, projectId)?.ParticipantsWrite != true)return Forbid ();

                var person = _db.Person.FindByCondition (x => x.Id == dto.PersonId).SingleOrDefault ();
                if (person == null)return BadRequest (nameof (PersonNotFoundException));

                var role = _db.Role.FindByCondition (x => x.Id == dto.RoleId && x.ProjectId == projectId).SingleOrDefault ();
                if (role == null)return BadRequest ();

                var participation = _mapper.Map<Participation> (dto);
                participation.ProjectId = projectId;
                participation.Status = ParticipationStatus.Invited.ToString ();

                _db.Participation.Create (participation);
                _db.Save ();

                var insertedParticipation = _db.Participation
                    .FindByCondition (x => x.Id == participation.Id)
                    .Include (x => x.Person)
                    .Include (x => x.Role)
                    .SingleOrDefault ();

                return Ok (_mapper.Map<ParticipationDto> (insertedParticipation));
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in CreateInvitation: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpPost ("requests")]
        public ActionResult<ParticipationDto> CreateRequest (Guid personId, Guid projectId)
        {
            try
            {
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();

                var project = _db.Project.FindByCondition (x => x.Id == projectId).SingleOrDefault ();
                if (project == null)return BadRequest (nameof (ProjectNotFoundException));
                if (!project.AllowRequests)return BadRequest (nameof (RequestsNotAllowedException));

                var participation = new Participation
                {
                    PersonId = personId,
                    ProjectId = projectId,
                    RoleId = null,
                    Status = ParticipationStatus.Requested.ToString (),
                };

                _db.Participation.Create (participation);
                _db.Save ();

                var insertedParticipation = _db.Participation
                    .FindByCondition (x => x.Id == participation.Id)
                    .Include (x => x.Project)
                    .Include (x => x.Role)
                    .SingleOrDefault ();

                return Ok (_mapper.Map<ParticipationDto> (insertedParticipation));
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in CreateRequest: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpPut ("invitations/{participationId}")]
        public ActionResult<ParticipationDto> UpdateInvitation (Guid personId, Guid projectId, Guid participationId, [FromBody] UpdateParticipationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)return BadRequest ();
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();
                if (_db.Participation.GetRole (personId, projectId)?.ParticipantsWrite != true)return Forbid ();

                var role = _db.Role.FindByCondition (x => x.Id == dto.RoleId && x.ProjectId == projectId).SingleOrDefault ();
                if (role == null)return BadRequest ();

                var participation = _db.Participation
                    .FindByCondition (x => x.Id == participationId && x.ProjectId == projectId)
                    .SingleOrDefault ();
                if (participation == null)return BadRequest ();

                participation.RoleId = dto.RoleId;

                _db.Participation.Update (participation);
                _db.Save ();

                var updatedParticipation = _db.Participation
                    .FindByCondition (x => x.Id == participationId)
                    .Include (x => x.Person)
                    .Include (x => x.Role)
                    .SingleOrDefault ();

                return Ok (_mapper.Map<ParticipationDto> (updatedParticipation));
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in UpdateInvitation: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpPut ("invitations/{participationId}/accept")]
        public ActionResult<ParticipationDto> AcceptInvitation (Guid personId, Guid projectId, Guid participationId)
        {
            try
            {
                if (!ModelState.IsValid)return BadRequest ();
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();

                var participation = _db.Participation
                    .FindByCondition (x =>
                        x.Id == participationId &&
                        x.ProjectId == projectId &&
                        x.PersonId == personId &&
                        x.Status == ParticipationStatus.Invited.ToString ())
                    .SingleOrDefault ();
                if (participation == null)return BadRequest ();

                participation.Status = ParticipationStatus.Active.ToString ();

                _db.Participation.Update (participation);
                _db.Save ();

                var updatedParticipation = _db.Participation
                    .FindByCondition (x => x.Id == participationId)
                    .Include (x => x.Project)
                    .Include (x => x.Role)
                    .SingleOrDefault ();

                return Ok (_mapper.Map<ParticipationDto> (updatedParticipation));
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in AcceptInvitation: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpPut ("invitations/{participationId}/decline")]
        public IActionResult DeclineInvitation (Guid personId, Guid projectId, Guid participationId)
        {
            try
            {
                if (!ModelState.IsValid)return BadRequest ();
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();

                var participation = _db.Participation
                    .FindByCondition (x =>
                        x.Id == participationId &&
                        x.ProjectId == projectId &&
                        x.PersonId == personId)
                    .SingleOrDefault ();
                if (participation == null)return BadRequest ();

                _db.Participation.Delete (participation);
                _db.Save ();

                return Ok ();
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in DeclineInvitation: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpPut ("requests/{participationId}/accept")]
        public ActionResult<ParticipationDto> AcceptRequest (Guid personId, Guid projectId, Guid participationId, [FromBody] UpdateParticipationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)return BadRequest ();
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();
                if (_db.Participation.GetRole (personId, projectId)?.ParticipantsWrite != true)return Forbid ();

                var role = _db.Role.FindByCondition (x => x.Id == dto.RoleId && x.ProjectId == projectId).SingleOrDefault ();
                if (role == null)return BadRequest ();

                var participation = _db.Participation
                    .FindByCondition (x =>
                        x.Id == participationId &&
                        x.ProjectId == projectId)
                    .SingleOrDefault ();
                if (participation == null)return BadRequest ();

                participation.RoleId = dto.RoleId;
                participation.Status = ParticipationStatus.Active.ToString ();

                _db.Participation.Update (participation);
                _db.Save ();

                var updatedParticipation = _db.Participation
                    .FindByCondition (x => x.Id == participationId)
                    .Include (x => x.Person)
                    .Include (x => x.Role)
                    .SingleOrDefault ();

                return Ok (_mapper.Map<ParticipationDto> (updatedParticipation));
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in AcceptRequest: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpPut ("requests/{participationId}/decline")]
        public IActionResult DeclineRequest (Guid personId, Guid projectId, Guid participationId)
        {
            try
            {
                if (!ModelState.IsValid)return BadRequest ();
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();
                if (_db.Participation.GetRole (personId, projectId)?.ParticipantsWrite != true)return Forbid ();

                var participation = _db.Participation
                    .FindByCondition (x =>
                        x.Id == participationId &&
                        x.ProjectId == projectId)
                    .SingleOrDefault ();
                if (participation == null)return BadRequest ();

                _db.Participation.Delete (participation);
                _db.Save ();

                return Ok ();
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in DeclineRequest: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpPut ("requests/{participationId}/cancel")]
        public IActionResult CancelRequest (Guid personId, Guid projectId, Guid participationId)
        {
            try
            {
                if (!ModelState.IsValid)return BadRequest ();
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();

                var participation = _db.Participation
                    .FindByCondition (x =>
                        x.Id == participationId &&
                        x.ProjectId == projectId &&
                        x.PersonId == personId)
                    .SingleOrDefault ();
                if (participation == null)return BadRequest ();

                _db.Participation.Delete (participation);
                _db.Save ();

                return Ok ();
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in CancelRequest: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpPut ("participants/{participationId}")]
        public ActionResult<ParticipationDto> UpdateParticipant (Guid personId, Guid projectId, Guid participationId, [FromBody] UpdateParticipantDto dto)
        {
            try
            {
                if (!ModelState.IsValid)return BadRequest ();
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();
                if (_db.Participation.GetRole (personId, projectId)?.ParticipantsWrite != true)return Forbid ();

                var participation = _db.Participation
                    .FindByCondition (x => x.Id == participationId && x.ProjectId == projectId)
                    .Include (x => x.Role)
                    .SingleOrDefault ();
                if (participation == null)return BadRequest ();

                var person = _db.Person.GetById (participation.PersonId);

                if (dto.CongregationId != null)
                {
                    var congregation = _db.Congregation.FindByCondition (x => x.Id == dto.CongregationId).SingleOrDefault ();
                    if (congregation == null)return BadRequest ();

                    person.Congregation = congregation;
                }

                person.Firstname = dto.Firstname;
                person.Lastname = dto.Lastname;
                person.Gender = dto.Gender;
                person.Email = dto.Email;
                person.Language = dto.Language;
                person.Phone = dto.Phone;
                person.Privilege = dto.Privilege;
                person.Assignment = dto.Assignment;
                person.CongregationId = dto.CongregationId;
                person.Languages = dto.Languages;
                person.Notes = dto.Notes;

                _db.Person.Update (person);
                _db.Save ();

                participation.Person = person;

                return Ok (_mapper.Map<ParticipationDto> (participation));
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in UpdateParticipant: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpDelete ("invitations/{participationId}")]
        public IActionResult DeleteInvitation (Guid personId, Guid projectId, Guid participationId)
        {
            try
            {
                if (!ModelState.IsValid)return BadRequest ();
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();
                if (_db.Participation.GetRole (personId, projectId)?.ParticipantsWrite != true)return Forbid ();

                var participation = _db.Participation
                    .FindByCondition (x => x.Id == participationId && x.ProjectId == projectId)
                    .SingleOrDefault ();

                _db.Participation.Delete (participation);
                _db.Save ();

                return Ok ();
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in DeleteInvitation: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpDelete ("participants/{participationId}")]
        public IActionResult DeleteParticipant (Guid personId, Guid projectId, Guid participationId)
        {
            try
            {
                if (!ModelState.IsValid)return BadRequest ();
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();
                if (_db.Participation.GetRole (personId, projectId)?.ParticipantsWrite != true)return Forbid ();

                var participation = _db.Participation
                    .FindByCondition (x => x.Id == participationId && x.ProjectId == projectId)
                    .SingleOrDefault ();

                _db.Participation.Delete (participation);
                _db.Save ();

                return Ok ();
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in DeleteParticipant: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

        [HttpDelete ("participations/{participationId}")]
        public IActionResult DeleteParticipation (Guid personId, Guid projectId, Guid participationId)
        {
            try
            {
                if (!_db.Person.BelongsToUser (personId, HttpContext))return Forbid ();

                var participation = _db.Participation
                    .FindByCondition (x => x.Id == participationId && x.PersonId == personId)
                    .SingleOrDefault ();

                _db.Participation.Delete (participation);
                _db.Save ();

                return Ok ();
            }
            catch (Exception e)
            {
                _logger.LogError ($"ERROR in DeleteParticipation: {e.Message}");
                return StatusCode (500, "Internal server error");
            }
        }

    }
}
