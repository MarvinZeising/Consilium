using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Entities.DataTransferObjects
{
    public class CreateInvitationDto
    {
        [Required]
        public Guid PersonId { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }
}
