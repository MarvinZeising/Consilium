using System;

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
    }
}
