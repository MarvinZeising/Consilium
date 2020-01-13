using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class UpdateArticleDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(10000)]
        public string Content { get; set; }
    }
}
