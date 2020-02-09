using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class UpdateCategoryDto
    {
        [Required]
        [ValidName]
        public string Name { get; set; }

        public ICollection<UpdateEligibilityDto> Eligibilities { get; set; }
    }
}
