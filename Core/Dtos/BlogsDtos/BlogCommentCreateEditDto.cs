using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.BlogsDtos
{
    public class BlogCommentCreateEditDto
    {
        public int Id { get; set; }
        public int? ParentBlogCommentId { get; set; }
        public int BlogId { get; set; }
        

        [Required, MinLength(10), MaxLength(300)]
        public string CommentContent { get; set; }
    }
}