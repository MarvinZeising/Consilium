using System;

namespace Entities.DataTransferObjects
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public Guid CongregationId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
