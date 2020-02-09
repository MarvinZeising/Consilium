using System;

namespace Server.Entities.DataTransferObjects
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectDto Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HelpLink { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
