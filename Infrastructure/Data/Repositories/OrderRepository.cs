using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.ClientBaskets;
using Core.Entities.Orders;
using Core.Interfaces;
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

    }
}






