using System;
using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class UpdateShiftDto
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [ValidDate]
        public int Date { get; set; }

        [Required]
        [ValidTime]
        public int Time { get; set; }

        [Required]
        [ValidTime]
        public int Duration { get; set; }
    }
}
