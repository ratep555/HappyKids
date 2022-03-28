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
    public class OrderStatusesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderStatusesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<OrderStatusDto>>> GetAllOrderStatuses(
                [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.OrderStatusRepository.GetCountForOrderStatuses();
            
            var list = await _unitOfWork.OrderStatusRepository.GetAllOrderStatuses(queryParameters);

            var data = _mapper.Map<IEnumerable<OrderStatusDto>>(list);

            return Ok(new Pagination<OrderStatusDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderStatusDto>> GetOrderStatusById(int id)
        {
            var orderStatus = await _unitOfWork.OrderStatusRepository.GetOrderStatusById(id);

            if (orderStatus == null) return NotFound();

            return _mapper.Map<OrderStatusDto>(orderStatus);
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPost]
        public async Task<ActionResult> CreateOrderStatus([FromBody] OrderStatusCreateEditDto orderStatusDto)
        {
            var orderStatus = _mapper.Map<OrderStatus>(orderStatusDto);

            await _unitOfWork.OrderStatusRepository.CreateOrderStatus(orderStatus);

            return Ok();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusCreateEditDto orderStatusDto)
        {
            var orderStatus = _mapper.Map<OrderStatus>(orderStatusDto);

            if (id != orderStatus.Id) return BadRequest("Bad request!");        

            await _unitOfWork.OrderStatusRepository.UpdateOrderStatus(orderStatus);

            return NoContent();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderStatus(int id)
        {
            var orderStatus = await _unitOfWork.OrderStatusRepository.GetOrderStatusById(id);

            if (orderStatus == null) return NotFound();

            await _unitOfWork.OrderStatusRepository.DeleteOrderStatus(orderStatus);

            return NoContent();
        }      
    }
}









