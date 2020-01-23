using System;

namespace Entities.DataTransferObjects
{
    public class CongregationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public int NumberOfParticipants { get; set; }
    }
}
