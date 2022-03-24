using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Orders;

namespace Core.Interfaces
{
    public interface IOrderStatusRepository
    {
        Task<List<OrderStatus>> GetAllOrderStatusesForEditing();
        Task<List<OrderStatus>> GetOrderStatusesAssociatedWithOrdersForChildrenItems();
        int GetPendingPaymentOrderStatusId();
        int GetFailedPaymentOrderStatusId();
        int GetReceivedPaymentOrderStatusId();
        int GetOrderAccepotedOrderStatusId();
        public int GetOrderRejectedOrderStatusId();
    }
}







