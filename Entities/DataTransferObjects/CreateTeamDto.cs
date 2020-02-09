using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class CreateTeamDto
    {
        [Required]
        [ValidName]
        public string Name { get; set; }

        [MaxLength (1000)]
        public string Description { get; set; }

        [MaxLength (1000)]
        public string HelpLink { get; set; }
    }
}
