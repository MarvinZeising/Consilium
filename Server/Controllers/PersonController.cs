using System.Security.Claims;
using System;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost]
        public IActionResult CreatePerson([FromBody] CreatePersonDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

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

        [HttpPut("{personId}")]
        public IActionResult UpdateGeneral(Guid personId, [FromBody] UpdatePersonGeneralDto dto)
        {
            try
            {
                if (dto == null || !ModelState.IsValid)
                {
                    return BadRequest();
                }

                var person = _db.Person.GetById(personId);

                var userId = HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
                if (person.UserId != new Guid(userId))
                {
                    return Forbid();
                }

                person.Firstname = dto.Firstname;
                person.Lastname = dto.Lastname;
                person.Gender = dto.Gender;

                _db.Person.Update(person);
                _db.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateGeneral: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{personId}")]
        public IActionResult DeletePerson(Guid personId)
        {
            try
            {
                var person = _db.Person.GetById(personId);

                var userId = HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
                if (person.UserId != new Guid(userId))
                {
                    return Forbid();
                }

                _db.Person.Delete(person);
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
