using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Blogs;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly HappyKidsContext _context;
        public BlogRepository(HappyKidsContext context)
        {
            _context = context;
        }

        // BLOGS
        /// <summary>
        /// Shows all blogs
        /// </summary>
        public async Task<List<Blog>> GetAllBlogs(QueryParameters queryParameters)
        {
            IQueryable<Blog> blogs = _context.Blogs.AsQueryable().OrderBy(x => x.Title);

            if (queryParameters.HasQuery())
            {
                blogs = blogs.Where(t => t.Title.Contains(queryParameters.Query));
            }

            blogs = blogs.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await blogs.ToListAsync();
        }

        /// <summary>
        /// This is for paging purposes, shows the total number of blogs
        /// </summary>
        public async Task<int> GetCountForBlogs()
        {
            return await _context.Blogs.CountAsync();
        }

        /// <summary>
        /// Shows all blogs presented in user interface for user
        /// See BlogsController/GetAllBlogsForUser for more details
        /// </summary>
        public async Task<List<Blog>> GetAllBlogsForUser(int userId, QueryParameters queryParameters)
        {
            IQueryable<Blog> blogs = _context.Blogs.Include(x => x.ApplicationUser)
                .Where(x => x.ApplicationUserId == userId)
                .AsQueryable().OrderBy(x => x.Title);

            if (queryParameters.HasQuery())
            {
                blogs = blogs.Where(t => t.Title.Contains(queryParameters.Query));
            }

            blogs = blogs.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await blogs.ToListAsync();
        }

        /// <summary>
        /// This is for paging purposes, shows the total number of blogs in a form presented to user
        /// </summary>
        public async Task<int> GetCountForBlogsForUser(int userId)
        {
            return await _context.Blogs.Include(x => x.ApplicationUser)
                .Where(x => x.ApplicationUserId == userId).CountAsync();
        }

        /// <summary>
        /// Gets the corresponding blog based on id 
        /// </summary>
        public async Task<Blog> GetBlogById(int id)
        {
            return await _context.Blogs.Include(x => x.ApplicationUser).FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Creates blog
        /// </summary>
        public async Task AddBlog(Blog blog)
        {
            _context.Blogs.Add(blog);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates blog
        /// </summary>
        public async Task UpdateBlog(Blog blog)
        {    
            _context.Entry(blog).State = EntityState.Modified;     

            await _context.SaveChangesAsync();
        }

        // BLOG COMMENTS
        /// <summary>
        /// Shows all blog comments
        /// </summary>
        public async Task<List<BlogComment>> GetAllBlogComments(int blogId)
        {
            return await _context.BlogComments.Include(x => x.ApplicationUser)
                .Where(x => x.BlogId == blogId).ToListAsync();
        }

        /// <summary>
        /// Gets the corresponding blog comment based on id 
        /// </summary>
        public async Task<BlogComment> GetBlogCommentById(int id)
        {
            return await _context.BlogComments.Include(x => x.ApplicationUser).FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Creates blog comment
        /// </summary>
        public async Task AddBlogComment(BlogComment blogComment)
        {
            _context.BlogComments.Add(blogComment);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates blog comment
        /// </summary>
        public async Task UpdateBlogComment(BlogComment blogComment)
        {    
            _context.Entry(blogComment).State = EntityState.Modified;        
             await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes blog comment
        /// </summary>
        public void DeleteBlogComment(BlogComment blogComment)
        {
            _context.BlogComments.Remove(blogComment);
        }

        /// <summary>
        /// Saves changes upon deleting blog comment and returns number of affected rows
        /// See BlogsController/DeleteBlogComment and comments.component.ts for more details
        /// </summary>
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }
    }
}












