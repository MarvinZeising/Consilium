using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class MakeAssignmentDto
    {
        [Required]
        public ICollection<CreateAttendeeDto> Attendees { get; set; }
    }
}
