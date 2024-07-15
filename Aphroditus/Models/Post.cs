﻿using System.ComponentModel.DataAnnotations;

namespace Aphroditus.Models
{
    public class Post
    {
        [Required]
        public int Id { get; set; }

        [StringLength(256, MinimumLength = 2)]
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? ImgName { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Post Date")]
        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }
    }
}