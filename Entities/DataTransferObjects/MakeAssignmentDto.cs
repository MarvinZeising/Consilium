using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Entities.DataTransferObjects
{
    public class MakeAssignmentDto
    {
        [Required]
        public ICollection<CreateAttendeeDto> Attendees { get; set; }
    }
}
