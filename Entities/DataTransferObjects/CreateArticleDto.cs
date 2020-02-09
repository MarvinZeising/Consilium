using System.ComponentModel.DataAnnotations;

namespace Server.Entities.DataTransferObjects
{
    public class CreateArticleDto
    {
        [Required]
        [MaxLength (100)]
        public string Title { get; set; }

        [Required]
        [MaxLength (10000)]
        public string Content { get; set; }
    }
}
