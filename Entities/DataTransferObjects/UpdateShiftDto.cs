using System;
using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
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
