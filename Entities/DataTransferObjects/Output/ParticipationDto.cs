using System;

namespace Entities.DataTransferObjects
{
    public class ParticipationDto
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? RoleId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public PersonDto Person { get; set; }
        public ProjectDto Project { get; set; }
        public RoleDto Role { get; set; }
    }
}
