using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class UpdateInvitationDto
    {
        [Required]
        public Guid RoleId { get; set; }
    }
}
