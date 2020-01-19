using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class CreateCategoryDto
    {
        [Required]
        [ValidName]
        public string Name { get; set; }

        public ICollection<CreateEligibilityDto> Eligibilities { get; set; }
    }
}
