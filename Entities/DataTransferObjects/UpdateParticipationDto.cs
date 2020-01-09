using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class UpdateParticipationDto
    {
        [Required]
        public Guid RoleId { get; set; }
    }
}
