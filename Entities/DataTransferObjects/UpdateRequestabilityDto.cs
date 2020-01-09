using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class UpdateRequestabilityDto
    {
        [Required]
        public bool AllowRequests { get; set; }
    }
}
