using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Entities.Orders;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Handling shipping options for children item orders
    /// </summary>
    public class ShippingOptionsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShippingOptionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ShippingOptionDto>>> GetAllShippingOptions(
                [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.ShippingOptionRepository.GetCountForShippingOptions();
            
            var list = await _unitOfWork.ShippingOptionRepository.GetAllShippingOptions(queryParameters);

            var data = _mapper.Map<IEnumerable<ShippingOptionDto>>(list);

            return Ok(new Pagination<ShippingOptionDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShippingOptionDto>> GetShippingOptionById(int id)
        {
            var shippingOption = await _unitOfWork.ShippingOptionRepository.GetShippingOptionById(id);

            if (shippingOption == null) return NotFound();

            return _mapper.Map<ShippingOptionDto>(shippingOption);
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPost]
        public async Task<ActionResult> CreateShippingOption([FromBody] ShippingOptionCreateEitDto shippingOptionDto)
        {
            var shippingOption = _mapper.Map<ShippingOption>(shippingOptionDto);

            await _unitOfWork.ShippingOptionRepository.CreateShippingOption(shippingOption);

            return Ok();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateShippingOption(int id, [FromBody] ShippingOptionCreateEitDto shippingOptionDto)
        {
            var shippingOption = _mapper.Map<ShippingOption>(shippingOptionDto);

            if (id != shippingOption.Id) return BadRequest("Bad request!");        

            await _unitOfWork.ShippingOptionRepository.UpdateShippingOption(shippingOption);

            return NoContent();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteShippingOption(int id)
        {
            var shippingOptiopn = await _unitOfWork.ShippingOptionRepository.GetShippingOptionById(id);

            if (shippingOptiopn == null) return NotFound();

            await _unitOfWork.ShippingOptionRepository.DeleteShippingOption(shippingOptiopn);

            return NoContent();
        }      
    }
}