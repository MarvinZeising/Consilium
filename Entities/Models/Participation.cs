using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Validators;

namespace Entities.Models
{
    [Table("participation")]
    public class Participation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey(nameof(Person))]
        public Guid PersonId { get; set; }
        public Person Person { get; set; }

        [Required]
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        [Required]
        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        [Required]
        [ValidParticipationStatus]
        public string Status { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedTime { get; set; }
    }
}
