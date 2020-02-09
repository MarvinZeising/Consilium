using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities.Models
{
    [Table ("attendee")]
    public class Attendee
    {
        [Key]
        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid ();

        [Required]
        [ForeignKey (nameof (Person))]
        public Guid PersonId { get; set; }
        public Person Person { get; set; }

        [Required]
        [ForeignKey (nameof (Shift))]
        public Guid ShiftId { get; set; }
        public Shift Shift { get; set; }

        [Required]
        [ForeignKey (nameof (Team))]
        public Guid TeamId { get; set; }
        public Team Team { get; set; }

        [ForeignKey (nameof (Application))]
        public Guid ApplicationId { get; set; }
        public Application Application { get; set; }

        [Required]
        public bool IsCaptain { get; set; }

        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTime { get; set; }

        [DatabaseGenerated (DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedTime { get; set; }
    }
}
