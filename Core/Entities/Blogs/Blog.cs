using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Identity;

namespace Core.Entities.Blogs
{
    public class Blog : BaseEntity
    {
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(5000)]
        public string BlogContent { get; set; }
        public string Picture { get; set; }


        [DataType(DataType.Date)]
        public DateTime PublishedOn { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdatedOn { get; set; }
    }
}