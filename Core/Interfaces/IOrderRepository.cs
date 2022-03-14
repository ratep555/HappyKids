using System.Threading.Tasks;
using Core.Entities.Orders;

namespace Core.Interfaces
{
    public interface IOrderRepository
    {
        void CreateClientOrder(ClientOrder clientOrder);
        void UpdateClientOrder(ClientOrder order);
        void DeleteClientOrder(ClientOrder order);
        Task<ClientOrder> FindOrderByPaymentIntentId(string paymentIntentIid);
    }
}