using System;
using System.Collections.Generic;

namespace Server.Entities.DataTransferObjects
{
    public class AttendeeDto
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public PersonDto Person { get; set; }
        public Guid ShiftId { get; set; }
        public ShiftDto Shift { get; set; }
        public Guid TeamId { get; set; }
        public TeamDto Team { get; set; }
        public Guid ApplicationId { get; set; }
        public ApplicationDto Application { get; set; }
        public bool IsCaptain { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
