using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class UpdatePersonGeneralDto
    {
        [Required]
        [ValidName]
        public string Firstname { get; set; }

        [Required]
        [ValidName]
        public string Lastname { get; set; }

        [Required]
        [ValidGender]
        public string Gender { get; set; }
    }
}
