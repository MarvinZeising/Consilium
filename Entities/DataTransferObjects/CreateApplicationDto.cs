using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Entities.DataTransferObjects
{
    public class CreateApplicationDto
    {
        [Required]
        public Guid ShiftId { get; set; }

        [Required]
        public bool AvailableAfter { get; set; }

        [MaxLength (200)]
        public string Notes { get; set; }
    }
}
