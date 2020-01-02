using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Validators;

namespace Entities.Models
{
    [Table("person")]
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(Congregation))]
        public Guid? CongregationId { get; set; }
        public Congregation Congregation { get; set; }

        [Required]
        [ValidName]
        public string Firstname { get; set; }

        [Required]
        [ValidName]
        public string Lastname { get; set; }

        [Required]
        [ValidGender]
        public string Gender { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedTime { get; set; } = DateTime.UtcNow;

        public ICollection<Participation> Participations { get; set; }
    }
}
