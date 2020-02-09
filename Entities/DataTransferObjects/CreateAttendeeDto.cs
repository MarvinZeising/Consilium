using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Entities.DataTransferObjects
{
    public class CreateAttendeeDto
    {
        [Required]
        public Guid PersonId { get; set; }

        [Required]
        public Guid TeamId { get; set; }

        [Required]
        public bool IsCaptain { get; set; }
    }
}
