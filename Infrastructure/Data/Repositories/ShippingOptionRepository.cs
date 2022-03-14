using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Orders;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ShippingOptionRepository : IShippingOptionRepository
    {
        private readonly HappyKidsContext _context;
        public ShippingOptionRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<ShippingOption> GetShippingOptionById(int id)
        {
            return await _context.ShippingOptions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public decimal GetShippingOptionPrice(int id)
        {
            return _context.ShippingOptions.Where(x => x.Id == id).First().Price;
        }
    }
}