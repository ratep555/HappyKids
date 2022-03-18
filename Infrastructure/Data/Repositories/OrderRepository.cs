using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.ClientBaskets;
using Core.Entities.Orders;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly HappyKidsContext _context;
        public OrderRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<List<ClientOrder>> GetAllOrdersForChildrenItems(QueryParameters queryParameters)
        {
            var orderStatuses = await _context.OrderStatuses
                .Where(x => x.Id == queryParameters.OrderStatusId).ToListAsync();

            IEnumerable<int?> ids = orderStatuses.Select(x =>(int?) x.Id);

            IQueryable<ClientOrder> orders = _context.ClientOrders.Include(x => x.OrderStatus)
                .Include(x => x.PaymentOption).Include(x => x.OrderChildrenItems).Include(x => x.ShippingOption)
                .AsQueryable().OrderBy(x => x.DateOfCreation);

            if (queryParameters.HasQuery())
            {
                orders = orders.Where(t => t.ShippingAddress.LastName.Contains(queryParameters.Query));
            }
            if (queryParameters.OrderStatusId.HasValue)
            {
                orders = orders.Where(x => ids.Contains(x.OrderStatusId));
            }

            orders = orders.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await orders.ToListAsync();
        }

        public async Task<int> GetCountForOrdersForChildrenItems()
        {
            return await _context.ClientOrders.CountAsync();
        }

        public async Task<List<ClientOrder>> GetOrdersForChildrenItemsForClient(
                QueryParameters queryParameters, string clientEmail)
        {
            IQueryable<ClientOrder> orders = _context.ClientOrders.Include(x => x.OrderStatus)
                .Include(x => x.PaymentOption).Include(x => x.OrderChildrenItems).Include(x => x.ShippingOption)
                .Where(x => x.CustomerEmail == clientEmail).AsQueryable().OrderBy(x => x.DateOfCreation);

            if (queryParameters.HasQuery())
            {
                orders = orders.Where(t => t.PaymentOption.Name.Contains(queryParameters.Query));
            }

            orders = orders.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await orders.ToListAsync();
        }

        public async Task<int> GetCountForOrdersForChildrenItemsForClient(string clientEmail)
        {
            return await _context.ClientOrders.Where(x => x.CustomerEmail == clientEmail).CountAsync();
        }

        public async Task<ClientOrder> GetClientOrderById(int id)
        {
            return await _context.ClientOrders.Include(x => x.OrderStatus).Include(x => x.PaymentOption)
                .Include(x => x.OrderChildrenItems).Include(x => x.ShippingOption)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ClientOrder> GetOrderForSpecificClientById(int id, string customerEmail)
        {
            return await _context.ClientOrders.Include(x => x.ShippingOption).Include(x => x.ShippingAddress)
                .Include(x => x.OrderChildrenItems)
                .FirstOrDefaultAsync(x => x.Id == id && x.CustomerEmail == customerEmail);
        }

        public async Task<ClientOrder> GetClientOrderByIdWithoutInclude(int id)
        {
            return await _context.ClientOrders.FirstOrDefaultAsync(p => p.Id == id);
        }

        public void CreateClientOrder(ClientOrder clientOrder)
        {
            _context.ClientOrders.Add(clientOrder);
        }

        public void UpdateClientOrder(ClientOrder order)
        {
            _context.ClientOrders.Update(order);
        }

        public void DeleteClientOrder(ClientOrder order)
        {
            _context.ClientOrders.Remove(order);
        }

        public async Task<ClientOrder> FindOrderByPaymentIntentId(string paymentIntentIid)
        {
            return await _context.ClientOrders.FirstOrDefaultAsync(x => x.PaymentIntentId == paymentIntentIid);
        }

        public async Task<List<ShippingOption>> GetShippingOptions()
        {
            return await _context.ShippingOptions.OrderByDescending(x => x.Price).ToListAsync();
        }

        public async Task<List<PaymentOption>> GetPaymentOptions()
        {
            return await _context.PaymentOptions.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<PaymentOption> GetStripePaymentOption()
        {
            return await _context.PaymentOptions.FirstOrDefaultAsync(x => x.Name == "Stripe");
        }
        
    }
}






