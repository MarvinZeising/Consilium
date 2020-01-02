using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Validators;

namespace Entities.Models
{
    [Table("project")]
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MinLength(3)]
        [ValidName]
        public string Name { get; set; }

        [Required]
        [ValidEmail]
        public string Email { get; set; }

        public ICollection<Participation> Participants { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedTime { get; set; } = DateTime.UtcNow;
    }
}
