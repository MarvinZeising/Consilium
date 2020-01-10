using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class CreateArticleDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Content { get; set; }
    }
}
