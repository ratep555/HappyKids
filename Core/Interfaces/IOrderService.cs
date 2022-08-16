using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Orders;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<ClientOrder> CreateOrder(string buyerEmail, int shippingOptionId, 
            int paymentOptionId, string basketId, ShippingAddress shippingAddress);
        Task<ClientOrder> CreateOrderForStripe(string buyerEmail, int shippingOptionId, 
            int paymentOptionId, string basketId, ShippingAddress shippingAddress);
        Task<bool> CheckIfBasketQuantityExceedsStockQuantity(string basketId);
    }
}