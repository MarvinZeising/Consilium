using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Entities.Validators;

namespace Server.Entities.Models
{
    [Table ("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid ();

        [Required]
        [ValidEmail]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        [ValidLanguage]
        public string Language { get; set; }

        [Required]
        [ValidTheme]
        public string Theme { get; set; }

        [Required]
        public string DateFormat { get; set; }

        [Required]
        public string TimeFormat { get; set; }

        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTime { get; set; }

        [DatabaseGenerated (DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedTime { get; set; }

        public ICollection<Person> Persons { get; set; }
    }

    public enum InterfaceLanguage
    {
        enUS,
        deDE
    }
}
