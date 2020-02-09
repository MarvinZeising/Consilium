using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Entities.Validators;

namespace Server.Entities.Models
{
    [Table ("project")]
    public class Project
    {
        [Key]
        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid ();

        [Required]
        [MinLength (3)]
        [ValidName]
        public string Name { get; set; }

        [Required]
        [ValidEmail]
        public string Email { get; set; }

        [Required]
        public bool AllowRequests { get; set; }

        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTime { get; set; }

        [DatabaseGenerated (DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedTime { get; set; }

        public ICollection<Participation> Participants { get; set; }
        public ICollection<Topic> Topics { get; set; }
    }
}
