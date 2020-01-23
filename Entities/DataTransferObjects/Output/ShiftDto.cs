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
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public ICollection<ApplicationDto> Applications { get; set; }
    }
}
