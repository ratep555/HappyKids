using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Orders;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IOrderStatusRepository
    {
        Task<List<OrderStatus>> GetAllOrderStatuses(QueryParameters queryParameters);
        Task<int> GetCountForOrderStatuses();
        Task CreateOrderStatus(OrderStatus orderStatus);
        Task UpdateOrderStatus(OrderStatus orderStatus);
        Task DeleteOrderStatus(OrderStatus orderStatus);
        Task<List<OrderStatus>> GetAllOrderStatusesForEditing();
        Task<List<OrderStatus>> GetOrderStatusesAssociatedWithOrdersForChildrenItems();
        Task<OrderStatus> GetOrderStatusById(int id);
        int GetPendingPaymentOrderStatusId();
        int GetFailedPaymentOrderStatusId();
        int GetReceivedPaymentOrderStatusId();
    }
}







