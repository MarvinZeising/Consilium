using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class UpdateCategoryDto
    {
        [Required]
        [ValidName]
        public string Name { get; set; }

        public ICollection<UpdateEligibilityDto> Eligibilities { get; set; }
    }
}
