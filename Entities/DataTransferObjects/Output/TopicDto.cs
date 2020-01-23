using System;
using System.Collections.Generic;

namespace Entities.DataTransferObjects
{
    public class TopicDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public ProjectDto Project { get; set; }
        public ICollection<ArticleDto> Articles { get; set; }
    }
}
