using System;
using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class CreateProjectDto
    {
        [Required]
        [ValidName]
        public string Name { get; set; }

        [Required]
        [ValidEmail]
        public string Email { get; set; }

        [Required]
        public Guid PersonId { get; set; }
    }
}
