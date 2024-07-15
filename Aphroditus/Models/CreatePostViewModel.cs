using System.ComponentModel.DataAnnotations;

namespace Aphroditus.Models
{
    public class CreatePostViewModel 
    {
        public int Id { get; set; }

        [StringLength(256, MinimumLength = 2)]
        [Required]
        public string? Title { get; set; }

        [MaxLength(1024)]
        public string? Description { get; set; }

        [Required]
        public IFormFile? Image { get; set; }


    }
}
