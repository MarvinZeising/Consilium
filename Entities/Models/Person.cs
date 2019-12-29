using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required]
        // [ForeignKey(nameof(User))]
        public Guid CongregationId { get; set; }
        // public User User { get; set; }

        [Required]
        [MaxLength(40)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(40)]
        public string Lastname { get; set; }

        [Required]
        [MaxLength(6)]
        public string Gender { get; set; }

        [Column("create_time")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    }
}
