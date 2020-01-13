using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class UpdatePersonMiscellaneousDto
    {
        [MaxLength(100)]
        public string Languages { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }
    }
}
