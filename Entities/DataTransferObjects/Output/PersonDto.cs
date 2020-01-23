using System;
using System.Collections.Generic;

namespace Entities.DataTransferObjects
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public string Phone { get; set; }
        public string Privilege { get; set; }
        public string Assignment { get; set; }
        public string Languages { get; set; }
        public string Notes { get; set; }
        public Guid? CongregationId { get; set; }
        public CongregationDto Congregation { get; set; }
        public DateTime CreatedTime { get; set; }
        public ICollection<ParticipationDto> Participations { get; set; }
    }
}
