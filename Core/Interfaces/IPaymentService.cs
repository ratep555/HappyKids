using System.Threading.Tasks;
using Core.Entities.ClientBaskets;
using Core.Entities.Orders;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<ClientBasket> CreatingOrUpdatingPaymentIntent(string basketId);
        Task<ClientOrder> UpdatingOrderPaymentSucceeded(string paymentIntentId);
        Task<ClientOrder> UpdatingOrderPaymentFailed(string paymentIntentId);    
    }
}
