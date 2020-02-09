using System;

namespace Server.Entities.DataTransferObjects
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public TopicDto Topic { get; set; }
    }
}
