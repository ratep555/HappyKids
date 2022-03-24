namespace Core.Dtos.BlogsDtos
{
    public class BlogCommentCreateEditDto
    {
        public int Id { get; set; }
        public int? ParentBlogCommentId { get; set; }
        public int BlogId { get; set; }
        public string CommentContent { get; set; }
    }
}