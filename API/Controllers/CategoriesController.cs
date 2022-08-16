using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Handling categories related to children items
    /// </summary>
    public class CategoriesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Showing list of all categories that can be associated with children item and/or dicount
        /// </summary>      
        [HttpGet]
        public async Task<ActionResult<Pagination<CategoryDto>>> GetAllCategories(
                [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.CategoryRepository.GetCountForCategories();
            
            var list = await _unitOfWork.CategoryRepository.GetAllCategories(queryParameters);

            var data = _mapper.Map<IEnumerable<CategoryDto>>(list);

            return Ok(new Pagination<CategoryDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryById(id);

            if (category == null) return NotFound();

            return _mapper.Map<CategoryDto>(category);
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryCreateEditDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            await _unitOfWork.CategoryRepository.CreateCategory(category);

            return Ok();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryCreateEditDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            if (id != category.Id) return BadRequest("Bad request!");        

            await _unitOfWork.CategoryRepository.UpdateCategory(category);

            return NoContent();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryById(id);

            if (category == null) return NotFound();

            await _unitOfWork.CategoryRepository.DeleteCategory(category);

            return NoContent();
        }      
    }
}