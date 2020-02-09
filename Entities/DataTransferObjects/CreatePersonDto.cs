using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class CreatePersonDto
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
