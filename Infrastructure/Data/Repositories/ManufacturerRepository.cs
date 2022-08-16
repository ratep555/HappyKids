using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly HappyKidsContext _context;
        public ManufacturerRepository(HappyKidsContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Shows all manufacturers
        /// </summary>
        public async Task<List<Manufacturer>> GetAllManufacturers(QueryParameters queryParameters)
        {
            IQueryable<Manufacturer> manufacturers = _context.Manufacturers.AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                manufacturers = manufacturers.Where(t => t.Name.Contains(queryParameters.Query));
            }

            manufacturers = manufacturers.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await manufacturers.ToListAsync();
        }
        /// <summary>
        /// This is for paging purposes, shows the total number of manufacturers
        /// </summary>
        public async Task<int> GetCountForManufacturers()
        {
            return await _context.Manufacturers.CountAsync();
        }

        /// <summary>
        /// Shows all the manufacturers, this is used in the process of creating/editing manufacturer discounts
        /// See for example DiscountsController/GetAllManufacturersForDiscounts and add-discount.component.ts for more details
        /// </summary>
        public async Task<List<Manufacturer>> GetAllPureManufacturers()
        {
            return await _context.Manufacturers.OrderBy(x => x.Name).ToListAsync();
        }
        
        /// <summary>
        /// Gets the corresponding manufacturer based on id
        /// </summary>
        public async Task<Manufacturer> GetManufacturerById(int id)
        {
            return await _context.Manufacturers.FirstOrDefaultAsync(p => p.Id == id);
        }
        /// <summary>
        /// Creates manufacturer
        /// </summary>
        public async Task CreateManufacturer(Manufacturer manufacturer)
        {
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();                    
        }
        /// <summary>
        /// Updates manufacturer
        /// </summary>
        public async Task UpdateManufacturer(Manufacturer manufacturer)
        {
            _context.Entry(manufacturer).State = EntityState.Modified;  
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Deletes manufacturer
        /// </summary>
        public async Task DeleteManufacturer(Manufacturer manufacturer)
        {
            _context.Manufacturers.Remove(manufacturer);
            await _context.SaveChangesAsync();
        }
    }
}