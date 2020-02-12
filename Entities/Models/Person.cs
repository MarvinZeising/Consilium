using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Entities.Validators;

namespace Server.Entities.Models
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

        [Required]
        [ValidEmail]
        public string Email { get; set; }

        [Required]
        [ValidLanguage]
        public string Language { get; set; }

        [Required]
        [MaxLength(40)]
        public string Phone { get; set; }

        [Required]
        [ValidPrivilege]
        public string Privilege { get; set; }

        [Required]
        [ValidAssignment]
        public string Assignment { get; set; }

        [Required]
        [MaxLength(100)]
        public string Languages { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Notes { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedTime { get; set; }

        public ICollection<Participation> Participations { get; set; }
    }
}
