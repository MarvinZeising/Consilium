using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class AuthenticateDto
    {
        [Required]
        [ValidEmail]
        public string Email { get; set; }

        [Required]
        [MinLength (128)]
        [MaxLength (128)]
        public string Password { get; set; }
    }
}
