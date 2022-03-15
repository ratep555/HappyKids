using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly HappyKidsContext _context;
        public WarehouseRepository(HappyKidsContext context)
        {
            _context = context;
        }

          public async Task<List<Warehouse>> GetAllWarehouses(QueryParameters queryParameters)
        {
            IQueryable<Warehouse> warehouses = _context.Warehouses.Include(x => x.Country)
                .AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                warehouses = warehouses.Where(t => t.Name.Contains(queryParameters.Query));
            }

            warehouses = warehouses.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await warehouses.ToListAsync();
        }

        public async Task<int> GetCountForWarehouses()
        {
            return await _context.Warehouses.CountAsync();
        }

        public async Task<Warehouse> GetWarehouseById(int id)
        {
            return await _context.Warehouses.FirstOrDefaultAsync(p => p.Id == id);
        }

         public async Task CreateWarehouse(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();                    
        }

        public async Task UpdateWarehouse(Warehouse warehouse)
        {
            _context.Entry(warehouse).State = EntityState.Modified;        
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWarehouse(Warehouse warehouse)
        {
    
            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _context.Countries.OrderBy(x => x.Name).ToListAsync();
        }
    }
}

