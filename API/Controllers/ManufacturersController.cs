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
    /// Handling manufacturers related to children items
    /// </summary>
    public class ManufacturersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ManufacturersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Showing list of all manufacturers of children items available in our webshop
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<Pagination<ManufacturerDto>>> GetAllManufacturers(
                [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.ManufacturerRepository.GetCountForManufacturers();
            
            var list = await _unitOfWork.ManufacturerRepository.GetAllManufacturers(queryParameters);

            var data = _mapper.Map<IEnumerable<ManufacturerDto>>(list);

            return Ok(new Pagination<ManufacturerDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ManufacturerDto>> GetManufacturerById(int id)
        {
            var manufacturer = await _unitOfWork.ManufacturerRepository.GetManufacturerById(id);

            if (manufacturer == null) return NotFound();

            return _mapper.Map<ManufacturerDto>(manufacturer);
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPost]
        public async Task<ActionResult> CreateManufacturer([FromBody] ManufacturerCreateEditDto manufacturerDto)
        {
            var manufacturer = _mapper.Map<Manufacturer>(manufacturerDto);

            await _unitOfWork.ManufacturerRepository.CreateManufacturer(manufacturer);

            return Ok();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateManufacturer(int id, 
            [FromBody] ManufacturerCreateEditDto manufacturerDto)
        {
            var manufacturer = _mapper.Map<Manufacturer>(manufacturerDto);

            if (id != manufacturer.Id) return BadRequest("Bad request!");        

            await _unitOfWork.ManufacturerRepository.UpdateManufacturer(manufacturer);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteManufacturer(int id)
        {
            var manufacturer = await _unitOfWork.ManufacturerRepository.GetManufacturerById(id);

            if (manufacturer == null) return NotFound();

            await _unitOfWork.ManufacturerRepository.DeleteManufacturer(manufacturer);

            return NoContent();
        }      
    }
}