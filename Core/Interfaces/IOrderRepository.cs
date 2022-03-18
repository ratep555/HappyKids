using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Orders;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<ClientOrder>> GetAllOrdersForChildrenItems(QueryParameters queryParameters);
        Task<int> GetCountForOrdersForChildrenItems();
        Task<List<ClientOrder>> GetOrdersForChildrenItemsForClient(QueryParameters queryParameters, string clientEmail);
        Task<int> GetCountForOrdersForChildrenItemsForClient(string clientEmail);
        Task<ClientOrder> GetClientOrderById(int id);
        Task<ClientOrder> GetOrderForSpecificClientById(int id, string customerEmail);
        Task<ClientOrder> GetClientOrderByIdWithoutInclude(int id);
        void CreateClientOrder(ClientOrder clientOrder);
        void UpdateClientOrder(ClientOrder order);
        void DeleteClientOrder(ClientOrder order);
        Task<ClientOrder> FindOrderByPaymentIntentId(string paymentIntentIid);
        Task<List<ShippingOption>> GetShippingOptions();
        Task<List<PaymentOption>> GetPaymentOptions();
        Task<PaymentOption> GetStripePaymentOption();
    }
}