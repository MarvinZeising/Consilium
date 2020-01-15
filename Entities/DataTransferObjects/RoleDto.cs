using System;

namespace Entities.DataTransferObjects
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public bool CalendarRead { get; set; }
        public bool CalendarWrite { get; set; }
        public bool KnowledgeBaseRead { get; set; }
        public bool KnowledgeBaseWrite { get; set; }
        public bool ParticipantsRead { get; set; }
        public bool ParticipantsWrite { get; set; }
        public bool RolesRead { get; set; }
        public bool RolesWrite { get; set; }
        public bool SettingsRead { get; set; }
        public bool SettingsWrite { get; set; }
        public bool Editable { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public ProjectDto Project { get; set; }
    }
}
