using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using Core.Dtos.OrdersDtos;
using Core.Entities.Orders;
using Core.Interfaces;
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
                string url = $"{_config["AngularAppUrl"]}/orders/{order.Id}";

                await _emailService.SendEmail(email, 
                "Order confirmation", $"<h2>Thank you for your order in the amount of {total} kn</h2>" +
                $"<p>Your order will be shipped in accordance with your selected shipping preferences." +
                $" You can view details of your order by <a href='{url}'>Clicking here</a></p>");
            }

            if (paymentOptionName == "General Card Slip")
            {
                _pdfService.GeneratePdfForGeneralCardSlip(order.Id, total, orderDto.ShippingAddress.FirstName,
                    orderDto.ShippingAddress.LastName);

                string url = $"{_config["AngularAppUrl"]}/orders/{order.Id}";

                await _emailService.SendEmailForGeneralCardSlip(email, 
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
    }
}






