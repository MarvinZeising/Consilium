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

        [HttpPost("tasks")]
        public ActionResult<TaskDto> CreateTask(Guid personId, Guid projectId, [FromBody] CreateTaskDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

                var task = _mapper.Map<Task>(dto);
                task.ProjectId = projectId;

                _db.Task.Create(task);
                _db.Save();

                return Ok(_mapper.Map<TaskDto>(task));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in CreateTask: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("tasks/{taskId}")]
        public ActionResult<TaskDto> UpdateTask(Guid personId, Guid projectId, Guid taskId, [FromBody] UpdateTaskDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

                var task = _db.Task
                    .FindByCondition(x => x.Id == taskId && x.ProjectId == projectId)
                    .SingleOrDefault();
                if (task == null) return BadRequest();

                task.Name = dto.Name;
                task.Description = dto.Description;
                task.HelpLink = dto.HelpLink;

                _db.Task.Update(task);
                _db.Save();

                return Ok(_mapper.Map<TaskDto>(task));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateTask: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("tasks/{taskId}")]
        public IActionResult DeleteTask(Guid personId, Guid projectId, Guid taskId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.CalendarWrite != true) return Forbid();

                var task = _db.Task
                    .FindByCondition(x => x.Id == taskId && x.ProjectId == projectId)
                    .SingleOrDefault();

                _db.Task.Delete(task);
                _db.Save();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in DeleteTask: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
