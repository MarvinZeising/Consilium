using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class CreateApplicationDto
    {
        [Required]
        public bool AvailableAfter { get; set; }

        [MaxLength(200)]
        public string Notes { get; set; }
    }
}
