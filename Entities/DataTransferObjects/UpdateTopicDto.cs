using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class UpdateTopicDto
    {
        [Required]
        [ValidName]
        public string Name { get; set; }
    }
}
