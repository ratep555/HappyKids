using System.Linq;
using System.Threading.Tasks;
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