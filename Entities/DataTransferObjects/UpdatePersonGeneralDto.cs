using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class UpdatePersonGeneralDto
    {
        [Required]
        [MaxLength(40)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(40)]
        public string Lastname { get; set; }

        [Required]
        [ValidGender]
        public string Gender { get; set; }
    }
}
