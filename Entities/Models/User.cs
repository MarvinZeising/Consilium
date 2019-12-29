using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(40)]
        public string Email { get; set; }

        [Required]
        [Column("password_hash")]
        public byte[] PasswordHash { get; set; }

        [Required]
        [Column("password_salt")]
        public byte[] PasswordSalt { get; set; }

        [Required]
        [MaxLength(40)]
        public string Language { get; set; }

        [Required]
        [MaxLength(40)]
        public string Theme { get; set; }

        [Column("create_time")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        public ICollection<Person> Persons { get; set; }
    }

    public enum InterfaceLanguage
    {
        enUS,
        deDE
    }
}
