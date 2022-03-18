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

        public async Task<int> GetCountForManufacturers()
        {
            return await _context.Manufacturers.CountAsync();
        }

        public async Task<Manufacturer> GetManufacturerById(int id)
        {
            return await _context.Manufacturers.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateManufacturer(Manufacturer manufacturer)
        {
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();                    
        }

        public async Task UpdateManufacturer(Manufacturer manufacturer)
        {
            _context.Entry(manufacturer).State = EntityState.Modified;  
            await _context.SaveChangesAsync();
        }

        public async Task DeleteManufacturer(Manufacturer manufacturer)
        {
            _context.Manufacturers.Remove(manufacturer);
            await _context.SaveChangesAsync();
        }
    }
}