using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class CreateCategoryDto
    {
        [Required]
        [ValidName]
        public string Name { get; set; }

        public ICollection<CreateEligibilityDto> Eligibilities { get; set; }
    }
}
