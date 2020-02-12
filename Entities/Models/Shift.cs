using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Entities.Validators;

namespace Server.Entities.Models
{
    [Table("shift")]
    public class Shift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        [ValidDate]
        public int Date { get; set; }

        [Required]
        [ValidTime]
        public int Time { get; set; }

        [Required]
        [ValidTime]
        public int Duration { get; set; }

        [Required]
        [ValidShiftStatus]
        public string Status { get; set; }

        [Required]
        [ValidShiftMode]
        public string Mode { get; set; }

        [Required]
        [MaxLength(200)]
        public string CalledOffReason { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedTime { get; set; }

        public ICollection<Application> Applications { get; set; }

        public ICollection<Attendee> Attendees { get; set; }
    }
}
