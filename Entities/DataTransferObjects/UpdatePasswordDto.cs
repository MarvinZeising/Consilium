using System.ComponentModel.DataAnnotations;

namespace Server.Entities.DataTransferObjects
{
    public class UpdatePasswordDto
    {
        [Required]
        [MinLength (128)]
        [MaxLength (128)]
        public string OldPassword { get; set; }

        [Required]
        [MinLength (128)]
        [MaxLength (128)]
        public string NewPassword { get; set; }
    }
}
