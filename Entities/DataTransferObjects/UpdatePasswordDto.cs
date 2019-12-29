using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class UpdatePasswordDto
    {
        [Required]
        [MaxLength(128)]
        public string OldPassword { get; set; }

        [Required]
        [MaxLength(128)]
        public string NewPassword { get; set; }
    }
}
