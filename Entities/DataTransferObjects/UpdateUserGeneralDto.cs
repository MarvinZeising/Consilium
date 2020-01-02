using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class UpdateUserGeneralDto
    {
        [Required]
        [ValidEmail]
        public string Email { get; set; }
    }
}
