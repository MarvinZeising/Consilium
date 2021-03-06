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
                    .Select(x => new Article
                    {
                        Id = x.Id,
                            TopicId = x.TopicId,
                            Title = x.Title,
                            Content = x.Content.Substring(0, 200),
                            CreatedTime = x.CreatedTime,
                            LastUpdatedTime = x.LastUpdatedTime,
                    })
                    .ToList();

                return Ok(_mapper.Map<IEnumerable<ArticleDto>>(articles));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetArticles: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("topics/{topicId}/articles/{articleId}")]
        public ActionResult<ArticleDto> GetArticle(Guid personId, Guid projectId, Guid topicId, Guid articleId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.KnowledgeBaseRead != true) return Forbid();

                var topic = _db.Topic.FindByCondition(x => x.Id == topicId).SingleOrDefault();
                if (topic == null || topic.ProjectId != projectId) return BadRequest();

                var article = _db.Article
                    .FindByCondition(x => x.Id == articleId && x.TopicId == topicId)
                    .Select(x => new Article
                    {
                        Id = x.Id,
                            TopicId = x.TopicId,
                            Title = x.Title,
                            Content = x.Content,
                            CreatedTime = x.CreatedTime,
                            LastUpdatedTime = x.LastUpdatedTime,
                    })
                    .SingleOrDefault();

                return Ok(_mapper.Map<ArticleDto>(article));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in GetArticle: {e.Message}");
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

        [HttpPost("topics/{topicId}/articles")]
        public ActionResult<ArticleDto> CreateArticle(Guid personId, Guid projectId, Guid topicId, [FromBody] CreateArticleDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.KnowledgeBaseWrite != true) return Forbid();

                var topic = _db.Topic.FindByCondition(x => x.Id == topicId && x.ProjectId == projectId).SingleOrDefault();
                if (topic == null) return BadRequest();

                var article = _mapper.Map<Article>(dto);
                article.TopicId = topicId;

                _db.Article.Create(article);
                _db.Save();

                var insertedArticle = _db.Article
                    .FindByCondition(x => x.Id == article.Id)
                    .Select(x => new Article
                    {
                        Id = x.Id,
                            TopicId = x.TopicId,
                            Title = x.Title,
                            Content = x.Content.Substring(0, 200),
                            CreatedTime = x.CreatedTime,
                            LastUpdatedTime = x.LastUpdatedTime,
                    })
                    .SingleOrDefault();

                return Ok(_mapper.Map<ArticleDto>(insertedArticle));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in CreateArticle: {e.Message}");
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

        [HttpPut("topics/{topicId}/articles/{articleId}")]
        public ActionResult<ArticleDto> UpdateArticle(Guid personId, Guid projectId, Guid topicId, Guid articleId, [FromBody] UpdateArticleDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.KnowledgeBaseWrite != true) return Forbid();

                var topic = _db.Topic.FindByCondition(x => x.Id == topicId && x.ProjectId == projectId).SingleOrDefault();
                if (topic == null) return BadRequest();

                var article = _db.Article.FindByCondition(x => x.Id == articleId && x.TopicId == topicId).SingleOrDefault();
                if (article == null) return BadRequest();

                article.Title = dto.Title;
                article.Content = dto.Content;

                _db.Article.Update(article);
                _db.Save();

                return Ok(_mapper.Map<ArticleDto>(article));
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateArticle: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("topics/{topicId}")]
        public IActionResult DeleteTopic(Guid personId, Guid projectId, Guid topicId)
        {
            try
            {
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

        [HttpDelete("topics/{topicId}/articles/{articleId}")]
        public IActionResult DeleteArticle(Guid personId, Guid projectId, Guid topicId, Guid articleId)
        {
            try
            {
                if (!_db.Person.BelongsToUser(personId, HttpContext)) return Forbid();
                if (_db.Participation.GetRole(personId, projectId)?.KnowledgeBaseWrite != true) return Forbid();

                var topic = _db.Topic.FindByCondition(x => x.Id == topicId && x.ProjectId == projectId).SingleOrDefault();
                if (topic == null) return BadRequest();

                var article = _db.Article.FindByCondition(x => x.Id == articleId && x.TopicId == topicId).SingleOrDefault();
                if (article == null) return BadRequest();

                _db.Article.Delete(article);
                _db.Save();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR in UpdateArticle: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
