using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class UpdatePersonContactDto
    {
        [ValidEmail]
        public string Email { get; set; }

        [ValidLanguage]
        public string Language { get; set; }

        [MaxLength (40)]
        public string Phone { get; set; }
    }
}
