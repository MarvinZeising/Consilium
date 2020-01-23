using System;

namespace Entities.DataTransferObjects
{
    public class ApplicationDto
    {
        public Guid Id { get; set; }
        public Guid ShiftId { get; set; }
        public ShiftDto Shift { get; set; }
        public Guid PersonId { get; set; }
        public PersonDto Person { get; set; }
        public bool AvailableAfter { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
