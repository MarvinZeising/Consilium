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

        [HttpPost("projects/{projectId}/congregations")]
        public ActionResult<CongregationDto> CreateCongregation(Guid personId, Guid projectId, [FromBody] CreateCongregationDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.ParticipantsWrite != true) return Forbid();
                // TODO: check if project is valid / paid for

                var congregationWithThisData = _db.Congregation
                    .FindByCondition(x => x.Name == dto.Name || x.Number == dto.Number)
                    .SingleOrDefault();
                if (congregationWithThisData?.Name == dto.Name) return BadRequest(nameof(CongregationNameUniqueException));
                if (congregationWithThisData?.Number == dto.Number) return BadRequest(nameof(CongregationNumberUniqueException));

                var congregation = _mapper.Map<Congregation>(dto);

                _db.Congregation.Create(congregation);
                _db.Save();

                var insertedCongregation = _db.Congregation.FindByCondition(x => x.Id == congregation.Id).SingleOrDefault();

                return Ok(_mapper.Map<CongregationDto>(insertedCongregation));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in CreateCongregation: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("projects/{projectId}/congregations/{congregationId}")]
        public ActionResult<CongregationDto> UpdateCongregation(Guid personId, Guid projectId, Guid congregationId, [FromBody] UpdateCongregationDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.ParticipantsWrite != true) return Forbid();
                // TODO: check if project is valid / paid for

                var congregationWithThisData = _db.Congregation
                    .FindByCondition(x => x.Id != congregationId && (x.Name == dto.Name || x.Number == dto.Number))
                    .SingleOrDefault();
                if (congregationWithThisData?.Name == dto.Name) return BadRequest(nameof(CongregationNameUniqueException));
                if (congregationWithThisData?.Number == dto.Number) return BadRequest(nameof(CongregationNumberUniqueException));

                var congregation = _db.Congregation.FindByCondition(x => x.Id == congregationId).SingleOrDefault();
                if (congregation == null) return BadRequest();

                congregation.Name = dto.Name;
                congregation.Number = dto.Number;

                _db.Congregation.Update(congregation);
                _db.Save();

                var updatedCongregation = _db.Congregation.FindByCondition(x => x.Id == congregationId).SingleOrDefault();

                return Ok(_mapper.Map<CongregationDto>(updatedCongregation));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateCongregation: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("projects/{projectId}/congregations/{congregationId}")]
        public IActionResult DeleteCongregation(Guid personId, Guid projectId, Guid congregationId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.ParticipantsWrite != true) return Forbid();
                // TODO: check if project is valid / paid for

                var congregation = _db.Congregation
                    .FindByCondition(x => x.Id == congregationId)
                    .Include(x => x.Persons)
                    .SingleOrDefault();
                if (congregation.Persons.Count > 0) return BadRequest();

                _db.Congregation.Delete(congregation);
                _db.Save();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in DeleteCongregation: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
