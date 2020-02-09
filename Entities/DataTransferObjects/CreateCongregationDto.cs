using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class CreateCongregationDto
    {
        [Required]
        [ValidName]
        public string Name { get; set; }

        [MaxLength (10)]
        public string Number { get; set; }
    }
}
