using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Entities.Orders;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PaymentOptionsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentOptionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<PaymentOptionDto>>> GetAllPaymentOptions(
                [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.PaymentOptionRepository.GetCountForPaymentOptions();
            
            var list = await _unitOfWork.PaymentOptionRepository.GetAllPaymentOptions(queryParameters);

            var data = _mapper.Map<IEnumerable<PaymentOptionDto>>(list);

            return Ok(new Pagination<PaymentOptionDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentOptionDto>> GetPaymentOptionById(int id)
        {
            var paymentOption = await _unitOfWork.PaymentOptionRepository.GetPaymentOptionById(id);

            if (paymentOption == null) return NotFound();

            return _mapper.Map<PaymentOptionDto>(paymentOption);
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPost]
        public async Task<ActionResult> CreatePaymentOption([FromBody] PaymentOptionCreateEditDto paymentOptionDto)
        {
            var paymentOption = _mapper.Map<PaymentOption>(paymentOptionDto);

            await _unitOfWork.PaymentOptionRepository.CreatePaymentOption(paymentOption);

            return Ok();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePaymentOption(int id, 
            [FromBody] PaymentOptionCreateEditDto paymentOptionDto)
        {
            var paymentOption = _mapper.Map<PaymentOption>(paymentOptionDto);

            if (id != paymentOption.Id) return BadRequest("Bad request!");        

            await _unitOfWork.PaymentOptionRepository.UpdatePaymentOption(paymentOption);

            return NoContent();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePaymentOption(int id)
        {
            var paymentOption = await _unitOfWork.PaymentOptionRepository.GetPaymentOptionById(id);

            if (paymentOption == null) return NotFound();

            await _unitOfWork.PaymentOptionRepository.DeletePaymentOption(paymentOption);

            return NoContent();
        }      
    }
}