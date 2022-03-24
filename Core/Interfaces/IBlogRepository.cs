using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Blogs;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IBlogRepository
    {
        // blogs
        Task<List<Blog>> GetAllBlogs(QueryParameters queryParameters);
        Task<int> GetCountForBlogs();
        Task<List<Blog>> GetAllBlogsForUser(int userId, QueryParameters queryParameters);  
        Task<int> GetCountForBlogsForUser(int userId);
        Task<Blog> GetBlogById(int id);
        Task AddBlog(Blog blog);
        Task UpdateBlog(Blog blog);

        // blogcomments
        Task<List<BlogComment>> GetAllBlogComments(int blogId);
        Task<BlogComment> GetBlogCommentById(int id);
        Task AddBlogComment(BlogComment blogComment);
        Task UpdateBlogComment(BlogComment blogComment);
        void DeleteBlogComment(BlogComment blogComment);
        Task<int> Complete();
    }
}










