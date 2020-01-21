using System;

namespace Entities.DataTransferObjects
{
    public class ShiftDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
