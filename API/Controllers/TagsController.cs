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
    public class TagsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<TagDto>>> GetAllTags([FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.TagRepository.GetCountForTags();
            
            var list = await _unitOfWork.TagRepository.GetAllTags(queryParameters);

            var data = _mapper.Map<IEnumerable<TagDto>>(list);

            return Ok(new Pagination<TagDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagDto>> GetTagById(int id)
        {
            var tag = await _unitOfWork.TagRepository.GetTagById(id);

            if (tag == null) return NotFound();

            return _mapper.Map<TagDto>(tag);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTag([FromBody] TagCreateEditDto tagDto)
        {
            var tag = _mapper.Map<Tag>(tagDto);

            await _unitOfWork.TagRepository.CreateTag(tag);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTag(int id, [FromBody] TagCreateEditDto tagDto)
        {
            var tag = _mapper.Map<Tag>(tagDto);

            if (id != tag.Id) return BadRequest("Bad request!");        

            await _unitOfWork.TagRepository.UpdateTag(tag);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTag(int id)
        {
            var tag = await _unitOfWork.TagRepository.GetTagById(id);

            if (tag == null) return NotFound();

            await _unitOfWork.TagRepository.DeleteTag(tag);

            return NoContent();
        }      
    }
}