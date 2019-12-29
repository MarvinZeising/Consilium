using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class AuthenticateDto
    {
        [Required]
        [MaxLength(40)]
        [ValidEmail]
        public string Email { get; set; }

        [Required]
        [MinLength(128)]
        [MaxLength(128)]
        public string Password { get; set; }
    }
}
