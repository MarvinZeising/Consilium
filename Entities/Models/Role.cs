using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Validators;

namespace Entities.Models
{
    [Table("role")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        [Required]
        [ValidName]
        public string Name { get; set; }

        [Required]
        public bool KnowledgeBaseRead { get; set; }

        [Required]
        public bool KnowledgeBaseWrite { get; set; }

        [Required]
        public bool ParticipantsRead { get; set; }

        [Required]
        public bool ParticipantsWrite { get; set; }

        [Required]
        public bool RolesRead { get; set; }

        [Required]
        public bool RolesWrite { get; set; }

        [Required]
        public bool SettingsRead { get; set; }

        [Required]
        public bool SettingsWrite { get; set; }

        [Required]
        public bool Editable { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedTime { get; set; }
    }
}
