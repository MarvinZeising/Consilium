using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class CreateUserDto
    {
        [Required]
        [MaxLength(40)]
        [ValidEmail]
        public string Email { get; set; }

        [Required]
        [MaxLength(128)]
        public string Password { get; set; }

        [Required]
        [MaxLength(10)]
        [ValidLanguage]
        public string Language { get; set; }

        [Required]
        [MaxLength(5)]
        [ValidTheme]
        public string Theme { get; set; }
    }
}
