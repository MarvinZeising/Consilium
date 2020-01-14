using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class UpdateParticipantDto
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

        [ValidEmail]
        public string Email { get; set; }

        [Required]
        [ValidLanguage]
        public string Language { get; set; }

        [MaxLength(40)]
        public string Phone { get; set; }

        [Required]
        [ValidAssignment]
        public string Assignment { get; set; }

        [Required]
        [ValidPrivilege]
        public string Privilege { get; set; }

        [MaxLength(100)]
        public string Languages { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }
    }
}
