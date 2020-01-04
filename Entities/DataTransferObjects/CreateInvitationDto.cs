using System;
using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
{
    public class CreateInvitationDto
    {
        [Required]
        public string PersonId { get; set; }

        [Required]
        public string RoleId { get; set; }
    }
}
