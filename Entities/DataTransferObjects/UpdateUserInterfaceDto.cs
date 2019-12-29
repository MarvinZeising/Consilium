using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class UpdateUserInterfaceDto
    {
        [Required]
        [MaxLength(10)]
        [ValidLanguage]
        public string Language { get; set; }

        [Required]
        [MaxLength(5)]
        [ValidTheme]
        public string Theme { get; set; }
    }
}
