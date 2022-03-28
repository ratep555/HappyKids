using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Orders;
using Core.Interfaces;
using Core.Utilities;
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

        public async Task<List<OrderStatus>> GetAllOrderStatuses(QueryParameters queryParameters)
        {
            IQueryable<OrderStatus> orderStatuses = _context.OrderStatuses.AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                orderStatuses = orderStatuses.Where(t => t.Name.Contains(queryParameters.Query));
            }

            orderStatuses = orderStatuses.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await orderStatuses.ToListAsync();
        }

        public async Task<int> GetCountForOrderStatuses()
        {
            return await _context.OrderStatuses.CountAsync();
        }

        public async Task<List<OrderStatus>> GetAllOrderStatusesForEditing()
        {
            return await _context.OrderStatuses.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task CreateOrderStatus(OrderStatus orderStatus)
        {
            _context.OrderStatuses.Add(orderStatus);
            await _context.SaveChangesAsync();                    
        }

        public async Task UpdateOrderStatus(OrderStatus orderStatus)
        {
            _context.Entry(orderStatus).State = EntityState.Modified;  
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderStatus(OrderStatus orderStatus)
        {
            _context.OrderStatuses.Remove(orderStatus);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderStatus>> GetOrderStatusesAssociatedWithOrdersForChildrenItems()
        {
            var clientOrders = await _context.ClientOrders.ToListAsync();

            IEnumerable<int?> ids = clientOrders.Select(x => x.OrderStatusId);

            var orderStatuses = await _context.OrderStatuses.Where(x => ids.Contains(x.Id))
                .OrderBy(x => x.Name).ToListAsync();

            return orderStatuses;
        }

        public async Task<OrderStatus> GetOrderStatusById(int id)
        {
            return await _context.OrderStatuses.FirstOrDefaultAsync(x => x.Id == id);
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
    }
}




