using System;
using System.Collections.Generic;

namespace Entities.DataTransferObjects
{
    public class ShiftDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public int Date { get; set; }
        public int Time { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; }
        public string Mode { get; set; }
        public string CalledOffReason { get; set; }
        public bool IsApplicant { get; set; }
        public bool IsAttendee { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public ICollection<ApplicationDto> Applications { get; set; }
        public ICollection<AttendeeDto> Attendees { get; set; }
    }
}
