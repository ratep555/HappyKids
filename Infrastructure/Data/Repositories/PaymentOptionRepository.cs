using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Orders;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class PaymentOptionRepository : IPaymentOptionRepository
    {
        private readonly HappyKidsContext _context;
        public PaymentOptionRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<PaymentOption> GetPaymentOptionById(int id)
        {
            return await _context.PaymentOptions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public string GetPaymentOptionName(int id)
        {
            return _context.PaymentOptions.Where(x => x.Id == id).First().Name;
        }
    }
}









