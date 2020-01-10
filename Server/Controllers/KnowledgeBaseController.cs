using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/persons/{personId}/projects/{projectId}")]
    public class KnowledgeBaseController : ControllerBase
    {
        private readonly IRepositoryWrapper _db;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public KnowledgeBaseController(
            IRepositoryWrapper db,
            ILoggerManager logger,
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("topics")]
        public ActionResult<IEnumerable<TopicDto>> GetTopics(Guid personId, Guid projectId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.KnowledgeBaseRead != true) return Forbid();

                var topics = _db.Topic
                    .FindByCondition(x => x.ProjectId == projectId)
                    .ToList();

                return Ok(_mapper.Map<IEnumerable<TopicDto>>(topics));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetTopics: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("topics/{topicId}/articles")]
        public ActionResult<IEnumerable<ArticleDto>> GetArticles(Guid personId, Guid projectId, Guid topicId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.KnowledgeBaseRead != true) return Forbid();

                var topic = _db.Topic.FindByCondition(x => x.Id == topicId).SingleOrDefault();
                if (topic == null || topic.ProjectId != projectId) return BadRequest();

                var articles = _db.Article
                    .FindByCondition(x => x.TopicId == topicId)
                    .ToList();

                return Ok(_mapper.Map<IEnumerable<ArticleDto>>(articles));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetArticles: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("topics")]
        public ActionResult<TopicDto> CreateTopic(Guid personId, Guid projectId, [FromBody] CreateTopicDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.KnowledgeBaseWrite != true) return Forbid();

                var topic = _mapper.Map<Topic>(dto);
                topic.ProjectId = projectId;

                _db.Topic.Create(topic);
                _db.Save();

                var insertedTopic = _db.Topic
                    .FindByCondition(x => x.Id == topic.Id)
                    .SingleOrDefault();

                return Ok(_mapper.Map<TopicDto>(insertedTopic));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in CreateTopic: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("topics/{topicId}")]
        public ActionResult<TopicDto> UpdateTopic(Guid personId, Guid projectId, Guid topicId, [FromBody] UpdateTopicDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.KnowledgeBaseWrite != true) return Forbid();

                var topic = _db.Topic
                    .FindByCondition(x => x.Id == topicId && x.ProjectId == projectId)
                    .SingleOrDefault();
                if (topic == null) return BadRequest();

                topic.Name = dto.Name;

                _db.Topic.Update(topic);
                _db.Save();

                var updatedTopic = _db.Topic
                    .FindByCondition(x => x.Id == topicId)
                    .SingleOrDefault();

                return Ok(_mapper.Map<TopicDto>(updatedTopic));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateTopic: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("topics/{topicId}")]
        public IActionResult DeleteTopic(Guid personId, Guid projectId, Guid topicId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.KnowledgeBaseWrite != true) return Forbid();

                var topic = _db.Topic
                    .FindByCondition(x => x.Id == topicId && x.ProjectId == projectId)
                    .SingleOrDefault();

                _db.Topic.Delete(topic);
                _db.Save();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in DeleteTopic: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
