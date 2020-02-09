using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class UpdateUserGeneralDto
    {
        [Required]
        [ValidEmail]
        public string Email { get; set; }
    }
}
