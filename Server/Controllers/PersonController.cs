using System.Security.Claims;
using System;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/persons")]
    public class PersonController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public PersonController(
            IRepositoryWrapper db,
            ILoggerManager logger,
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{personId}/participations")]
        public ActionResult<IEnumerable<ParticipationDto>> GetPersonParticipations(Guid personId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();

                var participations = _db.Participation
                    .FindByCondition(x => x.PersonId == personId)
                    .Include(x => x.Project).ThenInclude(x => x.Topics)
                    .Include(x => x.Role).ThenInclude(x => x.Eligibilities)
                    .ToList();

                return Ok(_mapper.Map<IEnumerable<ParticipationDto>>(participations));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetCurrentUser: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public ActionResult<PersonDto> CreatePerson([FromBody] CreatePersonDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();

                var userId = HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
                var person = _mapper.Map<Person>(dto);

                person.UserId = new Guid(userId);
                person.CongregationId = null;

                _db.Person.Create(person);
                _db.Save();

                return base.Ok(_mapper.Map<PersonDto>(person));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in CreatePerson: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{personId}/general")]
        public ActionResult<PersonDto> UpdateGeneral(Guid personId, [FromBody] UpdatePersonGeneralDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();

                var person = _db.Person.GetById(personId);

                person.Firstname = dto.Firstname;
                person.Lastname = dto.Lastname;
                person.Gender = dto.Gender;

                _db.Person.Update(person);
                _db.Save();

                return Ok(_mapper.Map<PersonDto>(person));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateGeneral: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{personId}/contact")]
        public ActionResult<PersonDto> UpdateContact(Guid personId, [FromBody] UpdatePersonContactDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();

                var person = _db.Person.GetById(personId);

                person.Email = dto.Email;
                person.Language = dto.Language;
                person.Phone = dto.Phone;

                _db.Person.Update(person);
                _db.Save();

                return Ok(_mapper.Map<PersonDto>(person));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateContact: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{personId}/theocratic")]
        public ActionResult<PersonDto> UpdateTheocratic(Guid personId, [FromBody] UpdatePersonTheocraticDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();

                var person = _db.Person.GetById(personId);

                person.Assignment = dto.Assignment;
                person.Privilege = dto.Privilege;

                _db.Person.Update(person);
                _db.Save();

                return Ok(_mapper.Map<PersonDto>(person));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateTheocratic: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{personId}/miscellaneous")]
        public ActionResult<PersonDto> UpdateMiscellaneous(Guid personId, [FromBody] UpdatePersonMiscellaneousDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();

                var person = _db.Person.GetById(personId);

                person.Languages = dto.Languages;
                person.Notes = dto.Notes;

                _db.Person.Update(person);
                _db.Save();

                return Ok(_mapper.Map<PersonDto>(person));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateMiscellaneous: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{personId}")]
        public IActionResult DeletePerson(Guid personId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();

                _db.Person.Delete(new Person { Id = personId });
                _db.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in DeletePerson: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
