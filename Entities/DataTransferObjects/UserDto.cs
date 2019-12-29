using System;
using System.Collections.Generic;

namespace Entities.DataTransferObjects
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public string Theme { get; set; }
        public DateTime CreateTime { get; set; }
        public IEnumerable<PersonDto> Persons { get; set; }
    }
}
