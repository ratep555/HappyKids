using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

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

            if (category == null) return NotFound(/* new ServerResponse(404) */);

            return _mapper.Map<CategoryDto>(category);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryCreateEditDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            await _unitOfWork.CategoryRepository.CreateCategory(category);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryCreateEditDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            if (id != category.Id) return BadRequest("Bad request!");        

            await _unitOfWork.CategoryRepository.UpdateCategory(category);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryById(id);

            if (category == null) return NotFound(/* new ServerResponse(404) */);

            await _unitOfWork.CategoryRepository.DeleteCategory(category);

            return NoContent();
        }      
    }
}