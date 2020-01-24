using System;
using System.ComponentModel.DataAnnotations;
using Entities.Validators;

namespace Entities.DataTransferObjects
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
