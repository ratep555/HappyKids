using System.Collections.Generic;
using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using Core.Dtos;
using Core.Dtos.OrdersDtos;
using Core.Entities.Orders;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IPdfService _pdfService;

        public OrdersController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork,
            IEmailService emailService, IConfiguration config, IPdfService pdfService)
        {
            _mapper = mapper;
            _orderService = orderService;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _config = config;
            _pdfService = pdfService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ClientOrderToReturnDto>>> GetAllOrdersForChildrenItems(
                [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.OrderRepository.GetCountForOrdersForChildrenItems();
            
            var list = await _unitOfWork.OrderRepository.GetAllOrdersForChildrenItems(queryParameters);

            var data = _mapper.Map<IEnumerable<ClientOrderToReturnDto>>(list);

            return Ok(new Pagination<ClientOrderToReturnDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("client")]
        public async Task<ActionResult<Pagination<ClientOrderToReturnDto>>> GetAllOrdersForChildrenItemsForClient(
                [FromQuery] QueryParameters queryParameters)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var count = await _unitOfWork.OrderRepository.GetCountForOrdersForChildrenItemsForClient(email);
            
            var list = await _unitOfWork.OrderRepository.GetOrdersForChildrenItemsForClient(queryParameters, email);

            var data = _mapper.Map<IEnumerable<ClientOrderToReturnDto>>(list);

            return Ok(new Pagination<ClientOrderToReturnDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientOrderToReturnDto>> GetOrderById(int id)
        {
            var clientOrder = await _unitOfWork.OrderRepository.GetClientOrderById(id);

            if (clientOrder == null) return NotFound(); 

            var clientOrderDto = _mapper.Map<ClientOrderToReturnDto>(clientOrder);

            clientOrderDto.ShippingAddress.Country = _unitOfWork.CountryRepository
                .GetCountryName(clientOrderDto.ShippingAddress.CountryId);
            
            return clientOrderDto;
        } 

        [HttpGet("client/{id}")]
        public async Task<ActionResult<ClientOrderToReturnDto>> GetOrderForSpecificClientById(int id)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var clientOrder = await _unitOfWork.OrderRepository.GetOrderForSpecificClientById(id, email);

            if (clientOrder == null) return NotFound(); 

            var clientOrderDto = _mapper.Map<ClientOrderToReturnDto>(clientOrder);

            clientOrderDto.ShippingAddress.Country = _unitOfWork.CountryRepository
                .GetCountryName(clientOrderDto.ShippingAddress.CountryId);
            
            return clientOrderDto;
        } 

        [HttpPost]
        public async Task<ActionResult<ClientOrder>> CreateOrder(ClientOrderDto orderDto)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var shippingOptionPrice = _unitOfWork.ShippingOptionRepository
                .GetShippingOptionPrice(orderDto.ShippingOptionId);

            var paymentOptionName = _unitOfWork.PaymentOptionRepository
                .GetPaymentOptionName(orderDto.PaymentOptionId);

            var address = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress);

            if (await _orderService.CheckIfBasketQuantityExceedsStackQuantity(orderDto.BasketId))
            {
                return BadRequest("Quantity in your basket excceds item's stack quantity!");
            }

            var order = await _orderService.CreateOrder(email, orderDto.ShippingOptionId, orderDto.PaymentOptionId,
                orderDto.BasketId, address);
                        
            var total = order.Subtotal + shippingOptionPrice;

            if (paymentOptionName == "Cash on Delivery (COD)")
            {
                string url = $"{_config["AngularAppUrl"]}/orderschildrenitemsclient/orderinfo/{order.Id}";

                await _emailService.SendEmail(email, 
                "Order confirmation", $"<h2>Thank you for your order in the amount of {total} kn</h2>" +
                $"<p>Your order will be shipped in accordance with your selected shipping preferences." +
                $" You can view details of your order by <a href='{url}'>Clicking here</a></p>");
            }

            if (paymentOptionName == "General Card Slip")
            {
                _pdfService.GeneratePdfForGeneralCardSlip(order.Id, total, orderDto.ShippingAddress.FirstName,
                    orderDto.ShippingAddress.LastName);

                string url = $"{_config["AngularAppUrl"]}/orderschildrenitemsclient/orderinfo/{order.Id}";

                await _emailService.SendEmailForGeneralCardSlipOrBirthdayOrderAcceptance(email, 
                "Order confirmation", $"<h2>Thank you for your order in the amount of {total} kn</h2>" +
                $"<p>Your order will be shipped in accordance with your selected shipping preferences" +
                $"once the payment is completed. You can view details of your order by <a href='{url}'>Clicking here</a></p>", order.Id);
            }

           await _unitOfWork.SaveAsync();

           if(order == null) return BadRequest("Problem creating order");

           return Ok(order);
        }

        [HttpPost("stripe")]
        public async Task<ActionResult<ClientOrder>> CreateOrderForStripe(ClientOrderDto orderDto)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var shippingOptionPrice = _unitOfWork.ShippingOptionRepository
                .GetShippingOptionPrice(orderDto.ShippingOptionId);

            var paymentOptionName = _unitOfWork.PaymentOptionRepository
                .GetPaymentOptionName(orderDto.PaymentOptionId);

            var address = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress);

            if (await _orderService.CheckIfBasketQuantityExceedsStackQuantity(orderDto.BasketId))
            {
                return BadRequest("Quantity in your basket excceds item's stack quantity!");
            }

            var order = await _orderService.CreateOrderForStripe(email, orderDto.ShippingOptionId, orderDto.PaymentOptionId,
                orderDto.BasketId, address);

           await _unitOfWork.SaveAsync();

           if(order == null) return BadRequest("Problem creating order");

           return Ok(order);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrderByAdmin(int id, [FromBody] OrderEditDto orderDto)
        {
            var order = await _unitOfWork.OrderRepository.GetClientOrderByIdWithoutInclude(id);

            if (order == null) return BadRequest("Bad request!");  

            order.OrderStatusId = orderDto.OrderStatusId;

            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("orderstatuses")]
        public async Task<ActionResult<List<OrderStatusDto>>> GetOrderStatuses()
        {
            var list = await _unitOfWork.OrderStatusRepository.GetAllOrderStatusesForEditing();

            return _mapper.Map<List<OrderStatusDto>>(list);
        }

        [HttpGet("orderstatusesforfiltering")]
        public async Task<ActionResult<List<OrderStatusDto>>> GetOrderStatusesAssociatedWithOrdersForChildrenItems()
        {
            var list = await _unitOfWork.OrderStatusRepository.GetOrderStatusesAssociatedWithOrdersForChildrenItems();

            return _mapper.Map<List<OrderStatusDto>>(list);
        }

        [AllowAnonymous]
        [HttpGet("shippingoptions")]
        public async Task<ActionResult<IEnumerable<ShippingOptionDto>>> GetShippingOptions()
        {
            var list = await _unitOfWork.OrderRepository.GetShippingOptions();

            var shippingoptions = _mapper.Map<IEnumerable<ShippingOptionDto>>(list);

            return Ok(shippingoptions);        
        }

        [AllowAnonymous]
        [HttpGet("paymentoptions")]
        public async Task<ActionResult<IEnumerable<PaymentOptionDto>>> GetPaymentOptions()
        {
            var list = await _unitOfWork.OrderRepository.GetPaymentOptions();

            var payingoptions = _mapper.Map<IEnumerable<PaymentOptionDto>>(list);

            return Ok(payingoptions);        
        }

        [AllowAnonymous]
        [HttpGet("stripepay")]
        public async Task<ActionResult<PaymentOptionDto>> GetStripePaymentOption()
        {
            var stripe = await _unitOfWork.OrderRepository.GetStripePaymentOption();

            return Ok(stripe);        
        }
    }
}






