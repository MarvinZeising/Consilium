using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class UpdateUserInterfaceDto
    {
        [Required]
        [ValidLanguage]
        public string Language { get; set; }

        [Required]
        [ValidTheme]
        public string Theme { get; set; }

        [Required]
        public string DateFormat { get; set; }

        [Required]
        public string TimeFormat { get; set; }
    }
}
