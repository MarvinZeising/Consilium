using System.ComponentModel.DataAnnotations;

namespace Server.Entities.DataTransferObjects
{
    public class UpdateRequestabilityDto
    {
        [Required]
        public bool AllowRequests { get; set; }
    }
}
