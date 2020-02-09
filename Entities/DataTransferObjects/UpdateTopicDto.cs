using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class UpdateTopicDto
    {
        [Required]
        [ValidName]
        public string Name { get; set; }
    }
}
