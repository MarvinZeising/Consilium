using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Validators;
using System.Collections.Generic;

namespace Entities.Models
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

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedTime { get; set; }

        public ICollection<Application> Applications { get; set; }
    }
}
