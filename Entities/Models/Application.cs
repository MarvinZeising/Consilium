using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities.Models
{
    [Table("application")]
    public class Application
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey(nameof(Shift))]
        public Guid ShiftId { get; set; }
        public Shift Shift { get; set; }

        [Required]
        [ForeignKey(nameof(Person))]
        public Guid PersonId { get; set; }
        public Person Person { get; set; }

        [ForeignKey(nameof(Team))]
        public Guid? TeamId { get; set; }
        public Team Team { get; set; }

        [Required]
        public bool AvailableAfter { get; set; }

        [MaxLength(200)]
        public string Notes { get; set; }

        [Required]
        public bool IsCaptain { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedTime { get; set; }

        public Application()
        {
            Notes = "";
        }
    }
}
