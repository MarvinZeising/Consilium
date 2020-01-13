using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class UpdatePersonContactDto
    {
        [ValidEmail]
        public string Email { get; set; }

        [MaxLength(40)]
        public string Phone { get; set; }
    }
}
