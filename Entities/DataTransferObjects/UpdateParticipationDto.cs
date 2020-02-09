using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Entities.DataTransferObjects
{
    public class UpdateParticipationDto
    {
        [Required]
        public Guid RoleId { get; set; }
    }
}
