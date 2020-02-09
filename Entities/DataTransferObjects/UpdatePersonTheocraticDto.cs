using System;
using System.ComponentModel.DataAnnotations;
using Server.Entities.Validators;

namespace Server.Entities.DataTransferObjects
{
    public class UpdatePersonTheocraticDto
    {
        [Required]
        [ValidAssignment]
        public string Assignment { get; set; }

        [Required]
        [ValidPrivilege]
        public string Privilege { get; set; }

        public Guid? CongregationId { get; set; }
    }
}
