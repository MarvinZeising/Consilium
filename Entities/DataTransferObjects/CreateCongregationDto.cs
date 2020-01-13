using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class CreateCongregationDto
    {
        [Required]
        [ValidName]
        public string Name { get; set; }

        [MaxLength(10)]
        public string Number { get; set; }
    }
}
