using System;

namespace Server.Entities.DataTransferObjects
{
    public class EligibilityDto
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public RoleDto Role { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public bool ShiftsRead { get; set; }
        public bool ShiftsWrite { get; set; }
        public bool IsTeamCaptain { get; set; }
        public bool IsSubstituteCaptain { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
