using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class CreateEligibilityDto
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public bool ShiftsRead { get; set; }

        [Required]
        public bool ShiftsWrite { get; set; }

        [Required]
        public bool IsTeamCaptain { get; set; }

        [Required]
        public bool IsSubstituteCaptain { get; set; }

        public ICollection<CreateEligibilityDto> Eligibilities { get; set; }
    }
}
