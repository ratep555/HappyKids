using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.ClientBaskets;
using Core.Entities.Orders;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork,
            IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            _paymentService = paymentService;
        }

        public async Task<ClientOrder> CreateOrder(string buyerEmail, int shippingOptionId, 
            int paymentOptionId, string basketId, ShippingAddress shippingAddress)
        {
            var basket = await _basketRepository.GetClientBasket(basketId);

            var orderChildrenItems = new List<OrderChildrenItem>();

            foreach (var item in basket.BasketChildrenItems)
            {
                var childrenItem = await _unitOfWork.ChildrenItemRepository
                    .GetChildrenItemByIdWithoutInclude(item.Id);

                var basketChildrenItemOrdered = new BasketChildrenItemOrdered(childrenItem.Id, childrenItem.Name);

                var orderChildrenItem = new OrderChildrenItem
                    (basketChildrenItemOrdered, childrenItem.Price, item.Quantity);
               
                if (childrenItem.DiscountedPrice != null)
                {
                    orderChildrenItem.Price = (decimal)childrenItem.DiscountedPrice;
                }

                orderChildrenItems.Add(orderChildrenItem);
            }

            var shippingOption = await _unitOfWork.ShippingOptionRepository.GetShippingOptionById(shippingOptionId);

            var paymentOption = await _unitOfWork.PaymentOptionRepository.GetPaymentOptionById(paymentOptionId);
 
            var subtotal = orderChildrenItems.Sum(item => item.Price * item.Quantity);

            var clientOrder = new ClientOrder(orderChildrenItems, buyerEmail, shippingAddress, shippingOption,
                paymentOption, subtotal, basket.PaymentIntentId);
            
            _unitOfWork.OrderRepository.CreateClientOrder(clientOrder);

            if (await _unitOfWork.SaveAsync()) return clientOrder;

            return null;        
        }

        public async Task<ClientOrder> CreateOrderForStripe(string buyerEmail, int shippingOptionId, 
            int paymentOptionId, string basketId, ShippingAddress shippingAddress)
        {
            var basket = await _basketRepository.GetClientBasket(basketId);

            var orderChildrenItems = new List<OrderChildrenItem>();

            foreach (var item in basket.BasketChildrenItems)
            {
                var childrenItem = await _unitOfWork.ChildrenItemRepository
                    .GetChildrenItemByIdWithoutInclude(item.Id);

                var basketChildrenItemOrdered = new BasketChildrenItemOrdered(childrenItem.Id, childrenItem.Name);

                var orderChildrenItem = new OrderChildrenItem
                    (basketChildrenItemOrdered, childrenItem.Price, item.Quantity);
               
                if (childrenItem.DiscountedPrice != null)
                {
                    orderChildrenItem.Price = (decimal)childrenItem.DiscountedPrice;
                }                
                
                orderChildrenItems.Add(orderChildrenItem);
            }

            var shippingOption = await _unitOfWork.ShippingOptionRepository.GetShippingOptionById(shippingOptionId);

            var paymentOption = await _unitOfWork.PaymentOptionRepository.GetPaymentOptionById(paymentOptionId);
 
            var subtotal = orderChildrenItems.Sum(item => item.Price * item.Quantity);

            var existingOrder = await _unitOfWork.OrderRepository.FindOrderByPaymentIntentId(basket.PaymentIntentId);

            if (existingOrder != null)
            {
                _unitOfWork.OrderRepository.DeleteClientOrder(existingOrder);

                await _paymentService.CreatingOrUpdatingPaymentIntent(basket.PaymentIntentId);
            }
            
            var clientOrder = new ClientOrder(orderChildrenItems, buyerEmail, shippingAddress, shippingOption,
            paymentOption, subtotal, basket.PaymentIntentId);

            _unitOfWork.OrderRepository.CreateClientOrder(clientOrder);

            if (await _unitOfWork.SaveAsync()) return clientOrder;

            return null;        
        }

        public async Task<bool> CheckIfBasketQuantityExceedsStackQuantity(string basketId)
        {
            var basket = await _basketRepository.GetClientBasket(basketId);

            foreach (var item in basket.BasketChildrenItems)
            {
                var childrenItem = await _unitOfWork.ChildrenItemRepository
                    .GetChildrenItemByIdWithoutInclude(item.Id);

                if (childrenItem.StockQuantity < 0) return true;              
            }
            return false;
        }
    }
}












