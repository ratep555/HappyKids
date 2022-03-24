using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Orders;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly HappyKidsContext _context;
        public OrderStatusRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<List<OrderStatus>> GetAllOrderStatusesForEditing()
        {
            return await _context.OrderStatuses.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<OrderStatus>> GetOrderStatusesAssociatedWithOrdersForChildrenItems()
        {
            var clientOrders = await _context.ClientOrders.ToListAsync();

            IEnumerable<int?> ids = clientOrders.Select(x => x.OrderStatusId);

            var orderStatuses = await _context.OrderStatuses.Where(x => ids.Contains(x.Id))
                .OrderBy(x => x.Name).ToListAsync();

            return orderStatuses;
        }

        public int GetPendingPaymentOrderStatusId()
        {
            return _context.OrderStatuses.FirstOrDefault(x => x.Name == "Pending Payment").Id;
        }

        public int GetFailedPaymentOrderStatusId()
        {
            return _context.OrderStatuses.FirstOrDefault(x => x.Name == "Failed Payment").Id;
        }

        public int GetReceivedPaymentOrderStatusId()
        {
            return _context.OrderStatuses.FirstOrDefault(x => x.Name == "Received Payment").Id;
        }

        public int GetOrderAccepotedOrderStatusId()
        {
            return _context.OrderStatuses.FirstOrDefault(x => x.Name == "Order Accepted").Id;
        }

        public int GetOrderRejectedOrderStatusId()
        {
            return _context.OrderStatuses.FirstOrDefault(x => x.Name == "Order Rejected").Id;
        }
    }
}




