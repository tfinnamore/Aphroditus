using System.ComponentModel.DataAnnotations;

namespace Aphroditus.Models
{
    public class EditPostViewModel
    {
        [Required]
        public int Id { get; set; }

        [StringLength(256, MinimumLength = 2)]
        [Required]
        public string? Title { get; set; }

        [MaxLength(1024)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Post Date")]
        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }

    }
}
