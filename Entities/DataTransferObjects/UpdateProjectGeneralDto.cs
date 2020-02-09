using System;
using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class UpdateProjectGeneralDto
    {
        [Required]
        [ValidName]
        public string Name { get; set; }

        [Required]
        [ValidEmail]
        public string Email { get; set; }
    }
}
