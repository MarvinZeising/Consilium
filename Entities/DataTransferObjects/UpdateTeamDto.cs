using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class UpdateTeamDto
    {
        [Required]
        [ValidName]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(1000)]
        public string HelpLink { get; set; }
    }
}
