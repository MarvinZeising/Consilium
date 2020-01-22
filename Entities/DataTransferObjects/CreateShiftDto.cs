using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class CreateShiftDto
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [MinLength(16)]
        [MaxLength(16)]
        public string Start { get; set; }

        [Required]
        [MinLength(16)]
        [MaxLength(16)]
        public string End { get; set; }
    }
}
