using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.ClientBaskets;
using Core.Entities.Orders;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services
{
    /// <summary>
    /// Here we are creating/updating Stripe payment intent with the help of Stripe keys located in appsettings.json
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration config)
        {
            _config = config;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Method creates/updates payment intent 
        /// <param name="Amount">Stripe oes not take decimals, it takes numbers in long format, we have to cast everything to long.</param>
        /// </summary>
        public async Task<ClientBasket> CreatingOrUpdatingPaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            var basket = await _basketRepository.GetClientBasket(basketId);

            if (basket == null) return null;
            
            var shippingPrice = 0m;

            if (basket.ShippingOptionId.HasValue)
            {
                var shippingOption = await _unitOfWork.ShippingOptionRepository
                    .GetShippingOptionById((int)basket.ShippingOptionId);

                shippingPrice = shippingOption.Price;
            }

            foreach (var item in basket.BasketChildrenItems)
            {
                var childrenItem = await _unitOfWork.ChildrenItemRepository
                    .GetChildrenItemByIdWithoutInclude(item.Id);

                if (item.Price != childrenItem.Price)
                {
                    item.Price = childrenItem.Price;
                }
            }

            var service = new PaymentIntentService();

            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long) basket.BasketChildrenItems.Sum(i => i.Quantity * (i.Price * 100)) + (long) shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> {"card"}
                };
                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long) basket.BasketChildrenItems.Sum(i => i.Quantity * (i.Price * 100)) + (long) shippingPrice * 100
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }
            await _basketRepository.UpdateClientBasket(basket);

            return basket;
        }
        
        /// <summary>
        /// Sets payment_intent.payment_failed status for Stripe payments
        /// </summary>
        public async Task<ClientOrder> UpdatingOrderPaymentFailed(string paymentIntentId)
        {
            var order = await _unitOfWork.OrderRepository.FindOrderByPaymentIntentId(paymentIntentId);;

            if (order == null) return null;

            order.OrderStatusId = _unitOfWork.OrderStatusRepository.GetFailedPaymentOrderStatusId();

            await _unitOfWork.SaveAsync();

            return order;        
        }

        /// <summary>
        /// Sets payment_intent.succeeded status for Stripe payments
        /// </summary>
        public async Task<ClientOrder> UpdatingOrderPaymentSucceeded(string paymentIntentId)
        {
            var order = await _unitOfWork.OrderRepository.FindOrderByPaymentIntentId(paymentIntentId);;

            if (order == null) return null;

            order.OrderStatusId = _unitOfWork.OrderStatusRepository.GetReceivedPaymentOrderStatusId();

            _unitOfWork.OrderRepository.UpdateClientOrder(order);

            await _unitOfWork.SaveAsync();

            return order;           
        }
    }
}






