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

                var response = _mapper.Map<PersonDto>(person);

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in SignIn: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public IActionResult UpdateGeneral([FromBody] UpdateUserGeneralDto dto)
        {
            try
            {
                if (dto == null || !ModelState.IsValid)
                {
                    return BadRequest();
                }

                var userId = HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value;
                var user = _db.User.GetUserById(new Guid(userId));

                user.Email = dto.Email;

                _db.User.Update(user);
                _db.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateGeneral: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser()
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value;

                _db.User.Delete(new User { Id = new Guid(userId) });
                _db.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in DeleteUser: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
