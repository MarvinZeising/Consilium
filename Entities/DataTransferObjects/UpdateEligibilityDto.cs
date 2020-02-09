using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Entities.DataTransferObjects
{
    public class UpdateEligibilityDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool ShiftsRead { get; set; }

        [Required]
        public bool ShiftsWrite { get; set; }

        [Required]
        public bool IsTeamCaptain { get; set; }

        [Required]
        public bool IsSubstituteCaptain { get; set; }
    }
}
