using System;
using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class CreateRoleDto
    {
        [Required]
        public Guid PersonId { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        [ValidName]
        public string Name { get; set; }

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
    }
}
