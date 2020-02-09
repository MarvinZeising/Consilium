using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class UpdateRoleDto
    {
        [Required]
        [ValidName]
        public string Name { get; set; }

        [Required]
        public bool CalendarRead { get; set; }

        [Required]
        public bool CalendarWrite { get; set; }

        [Required]
        public bool SettingsRead { get; set; }

        [Required]
        public bool SettingsWrite { get; set; }

        [Required]
        public bool RolesRead { get; set; }

        [Required]
        public bool RolesWrite { get; set; }

        [Required]
        public bool ParticipantsRead { get; set; }

        [Required]
        public bool ParticipantsWrite { get; set; }

        [Required]
        public bool KnowledgeBaseRead { get; set; }

        [Required]
        public bool KnowledgeBaseWrite { get; set; }

        public ICollection<UpdateEligibilityDto> Eligibilities { get; set; }
    }
}
