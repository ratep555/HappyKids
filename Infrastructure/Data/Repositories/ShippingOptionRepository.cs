using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Orders;
using Core.Interfaces;
using Core.Utilities;
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
        /// <summary>
        /// Shows all shipping options
        /// </summary>
        public async Task<List<ShippingOption>> GetAllShippingOptions(QueryParameters queryParameters)
        {
            IQueryable<ShippingOption> shippingOptions = _context.ShippingOptions.AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                shippingOptions = shippingOptions.Where(t => t.Name.Contains(queryParameters.Query));
            }

            shippingOptions = shippingOptions.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await shippingOptions.ToListAsync();
        }
        /// <summary>
        /// This is for paging purposes, shows the total number of all shipping options
        /// </summary>
        public async Task<int> GetCountForShippingOptions()
        {
            return await _context.ShippingOptions.CountAsync();
        }
        /// <summary>
        /// Gets the corresponding shipping option based on id
        /// </summary>
        public async Task<ShippingOption> GetShippingOptionById(int id)
        {
            return await _context.ShippingOptions.FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// We need shipping option price in the process of creation of order
        /// See OrdersController/CreateOrder for more details
        /// </summary>
        public decimal GetShippingOptionPrice(int id)
        {
            return _context.ShippingOptions.Where(x => x.Id == id).First().Price;
        }
        /// <summary>
        /// Creates shipping option
        /// </summary>
        public async Task CreateShippingOption(ShippingOption shippingOption)
        {
            _context.ShippingOptions.Add(shippingOption);
            await _context.SaveChangesAsync();                    
        }
         /// <summary>
        /// Updates shipping option
        /// </summary>
        public async Task UpdateShippingOption(ShippingOption shippingOption)
        {
            _context.Entry(shippingOption).State = EntityState.Modified;  
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Deletes shipping option
        /// </summary>
        public async Task DeleteShippingOption(ShippingOption shippingOption)
        {
            _context.ShippingOptions.Remove(shippingOption);
            await _context.SaveChangesAsync();
        }
    }
}









