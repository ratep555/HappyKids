using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using Core.Dtos.BlogsDtos;
using Core.Entities.Blogs;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BlogsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private string containerName = "blogs";

        public BlogsController(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        // BLOGS

        /// <summary>
        /// Showing list of all blogs
        /// This is rendered in client view and also in the administrative (admin) part of the application        
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<Pagination<BlogDto>>> GetAllBlogs([FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.BlogRepository.GetCountForBlogs();
            
            var list = await _unitOfWork.BlogRepository.GetAllBlogs(queryParameters);
   
            var data = _mapper.Map<IEnumerable<BlogDto>>(list);

            return Ok(new Pagination<BlogDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }  

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<Pagination<BlogDto>>> GetAllBlogsForUser([FromQuery] QueryParameters queryParameters)
        {
            var userId = User.GetUserId();

            var count = await _unitOfWork.BlogRepository.GetCountForBlogsForUser(userId);
            
            var list = await _unitOfWork.BlogRepository.GetAllBlogsForUser(userId, queryParameters);
   
            var data = _mapper.Map<IEnumerable<BlogDto>>(list);

            return Ok(new Pagination<BlogDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDto>> GetBlogById(int id)
        {
            var blog = await _unitOfWork.BlogRepository.GetBlogById(id);

            if (blog == null) return NotFound();

            return _mapper.Map<BlogDto>(blog);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateBlog([FromForm] BlogCreateEditDto blogDto)
        {
            var blog = _mapper.Map<Blog>(blogDto);

            var userId = User.GetUserId();

            if (blogDto.Picture != null)
            {
                blog.Picture = await _fileStorageService.SaveFile(containerName, blogDto.Picture);
            }
            
            blog.ApplicationUserId = userId;
            blog.PublishedOn = DateTime.Now.ToLocalTime();

            await _unitOfWork.BlogRepository.AddBlog(blog);

            return Ok();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBlog(int id, [FromForm] BlogCreateEditDto blogDto)
        {
            var blog = await _unitOfWork.BlogRepository.GetBlogById(id);

            if (blog == null) return NotFound();

            var userId = User.GetUserId();

            blog = _mapper.Map(blogDto, blog);
            
            if (blogDto.Picture != null)
            {
                blog.Picture = await _fileStorageService
                    .EditFile(containerName, blogDto.Picture, blog.Picture);
            }

            blog.ApplicationUserId = userId;
            blog.UpdatedOn = DateTime.Now.ToLocalTime();

            await _unitOfWork.BlogRepository.UpdateBlog(blog);

            return NoContent();
        }

        // BLOGCOMMENTS

        /// <summary>
        /// Showing list of all blog comments related to certain blog
        /// </summary>        
        [HttpGet("blogcomments/{blogId}")]
        public async Task<ActionResult<IEnumerable<BlogCommentDto>>> GetAllBlogComments(int blogId)
        {
            var blogComments = await _unitOfWork.BlogRepository.GetAllBlogComments(blogId);

            var data = _mapper.Map<IEnumerable<BlogCommentDto>>(blogComments);

            return Ok(data);
        }

        /// <summary>
        /// Creates/updates blog comment
        /// </summary>      
        [Authorize]
        [HttpPost("blogcomments")]
        public async Task<ActionResult<BlogCommentDto>> UpsertBlogComment([FromBody] BlogCommentCreateEditDto blogCommentDto)
        {
            var blogComment = _mapper.Map<BlogComment>(blogCommentDto);

            var userId = User.GetUserId();

            if (blogCommentDto.Id == -1)
            {
                blogComment.ApplicationUserId = userId;
                blogComment.PublishedOn = DateTime.Now.ToLocalTime();
                blogComment.Id = 0;

                await _unitOfWork.BlogRepository.AddBlogComment(blogComment);
            }
            else
            {
                blogComment.ApplicationUserId = userId;
                blogComment.UpdatedOn = DateTime.Now.ToLocalTime();

                 await _unitOfWork.BlogRepository.UpdateBlogComment(blogComment);
            }
            var commentToReturn = _mapper.Map<BlogCommentDto>(blogComment);

            var comment1 = await _unitOfWork.BlogRepository.GetBlogCommentById(commentToReturn.Id);

            commentToReturn.Username = comment1.ApplicationUser.UserName;

            return Ok(commentToReturn);
        }

        /// <summary>
        /// Alolws user to delete hers/his blog comment
        /// </summary>      
        [Authorize]
        [HttpDelete("blogcomments/{id}")]
        public async Task<ActionResult<int>> DeleteBlogComment(int id)
        {
            var userId = User.GetUserId();

            var blogComment = await _unitOfWork.BlogRepository.GetBlogCommentById(id);

            if (blogComment == null) return NotFound();

            if (blogComment.ApplicationUserId == userId)
            {
                _unitOfWork.BlogRepository.DeleteBlogComment(blogComment);

                var affectedRows = await _unitOfWork.BlogRepository.Complete();

                return Ok(affectedRows);
            }
            else
            {
                return BadRequest("This comment was not created by the current user.");
            }
        }
    }
}










