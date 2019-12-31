using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class UpdateUserGeneralDto
    {
        [Required]
        [MaxLength(100)]
        [ValidEmail]
        public string Email { get; set; }
    }
}
