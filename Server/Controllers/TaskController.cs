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
    [Route("api/persons/{personId}/projects/{projectId}")]
    public class TaskController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public TaskController(
            IRepositoryWrapper db,
            ILoggerManager logger,
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("tasks")]
        public ActionResult<IEnumerable<TaskDto>> GetProjectTasks(Guid personId, Guid projectId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

                var tasks = _db.Task
                    .FindByCondition(x => x.ProjectId == projectId)
                    .ToList();

                return Ok(_mapper.Map<IEnumerable<TaskDto>>(tasks));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetProjectTasks: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // [HttpPost("categories")]
        // public ActionResult<CategoryDto> CreateCategory(Guid personId, Guid projectId, [FromBody] CreateCategoryDto dto)
        // {
        //     try
        //     {
        //         if (!ModelState.IsValid) return BadRequest();
        //         if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
        //         if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

        //         var category = _mapper.Map<Category>(dto);
        //         category.ProjectId = projectId;

        //         foreach (var eligibility in category.Eligibilities)
        //         {
        //             var role = _db.Role
        //                 .FindByCondition(x => x.Id == eligibility.RoleId && x.ProjectId == projectId)
        //                 .SingleOrDefault();
        //             if (role == null) return BadRequest();

        //             eligibility.ShiftsWrite = eligibility.ShiftsWrite && eligibility.ShiftsRead;
        //             eligibility.IsTeamCaptain = eligibility.IsTeamCaptain && eligibility.ShiftsRead;
        //             eligibility.IsSubstituteCaptain = eligibility.IsSubstituteCaptain && eligibility.ShiftsRead && !eligibility.IsTeamCaptain;
        //         }

        //         _db.Category.Create(category);
        //         _db.Save();

        //         var createdCategory = _db.Category
        //             .FindByCondition(x => x.Id == category.Id && x.ProjectId == projectId)
        //             .Include(x => x.Eligibilities).ThenInclude(x => x.Role)
        //             .SingleOrDefault();

        //         return Ok(_mapper.Map<CategoryDto>(createdCategory));
        //     }
        //     catch (Exception e)
        //     {
        //         _logger.LogError($"ERROR in CreateCategory: {e.Message}");
        //         return StatusCode(500, "Internal server error");
        //     }
        // }

        // [HttpPut("categories/{categoryId}")]
        // public ActionResult<CategoryDto> UpdateCategory(Guid personId, Guid projectId, Guid categoryId, [FromBody] UpdateCategoryDto dto)
        // {
        //     try
        //     {
        //         if (!ModelState.IsValid) return BadRequest();
        //         if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
        //         if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

        //         var category = _db.Category
        //             .FindByCondition(x => x.Id == categoryId && x.ProjectId == projectId)
        //             .SingleOrDefault();
        //         if (category == null) return BadRequest();

        //         category.Name = dto.Name;

        //         foreach (var eligibility in dto.Eligibilities)
        //         {
        //             var eligibilityFromDb = _db.Eligibility
        //                 .FindByCondition(x => x.Id == eligibility.Id && x.CategoryId == categoryId)
        //                 .SingleOrDefault();

        //             if (eligibilityFromDb != null)
        //             {
        //                 eligibilityFromDb.ShiftsRead = eligibility.ShiftsRead;
        //                 eligibilityFromDb.ShiftsWrite = eligibility.ShiftsWrite && eligibility.ShiftsRead;
        //                 eligibilityFromDb.IsTeamCaptain = eligibility.IsTeamCaptain && eligibility.ShiftsRead;
        //                 eligibilityFromDb.IsSubstituteCaptain = eligibility.IsSubstituteCaptain && eligibility.ShiftsRead && !eligibility.IsTeamCaptain;

        //                 _db.Eligibility.Update(eligibilityFromDb);
        //             }
        //         }


        //         _db.Category.Update(category);
        //         _db.Save();

        //         return Ok(_mapper.Map<CategoryDto>(category));
        //     }
        //     catch (Exception e)
        //     {
        //         _logger.LogError($"ERROR in UpdateCategory: {e.Message}");
        //         return StatusCode(500, "Internal server error");
        //     }
        // }

        // [HttpDelete("categories/{categoryId}")]
        // public IActionResult DeleteCategory(Guid personId, Guid projectId, Guid categoryId)
        // {
        //     try
        //     {
        //         if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
        //         if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

        //         var category = _db.Category
        //             .FindByCondition(x => x.Id == categoryId && x.ProjectId == projectId)
        //             .SingleOrDefault();

        //         _db.Category.Delete(category);
        //         _db.Save();

        //         return Ok();
        //     }
        //     catch (Exception e)
        //     {
        //         _logger.LogError($"ERROR in DeleteCategory: {e.Message}");
        //         return StatusCode(500, "Internal server error");
        //     }
        // }

    }
}
