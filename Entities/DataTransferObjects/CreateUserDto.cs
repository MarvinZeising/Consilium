using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class CreateUserDto
    {
        [Required]
        [ValidEmail]
        public string Email { get; set; }

        [Required]
        [MinLength (128)]
        [MaxLength (128)]
        public string Password { get; set; }

        [Required]
        [ValidLanguage]
        public string Language { get; set; }

        [Required]
        [ValidTheme]
        public string Theme { get; set; }
    }
}
