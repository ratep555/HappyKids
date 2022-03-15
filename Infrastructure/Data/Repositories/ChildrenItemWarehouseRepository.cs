using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.ChildrenItems;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ChildrenItemWarehouseRepository : IChildrenItemWarehouseRepository
    {
        private readonly HappyKidsContext _context;
        public ChildrenItemWarehouseRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<List<ChildrenItemWarehouse>> GetAllChildrenItemWarehouses(QueryParameters queryParameters)
        {
            IQueryable<ChildrenItemWarehouse> childrenItemWarehouses = _context.ChildrenItemWarehouses
                .Include(x => x.ChildrenItem).Include(x => x.Warehouse)
                .AsQueryable().OrderBy(x => x.ChildrenItem.Name);

            if (queryParameters.HasQuery())
            {
                childrenItemWarehouses = childrenItemWarehouses
                    .Where(t => t.ChildrenItem.Name.Contains(queryParameters.Query));
            }

            childrenItemWarehouses = childrenItemWarehouses
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1)).Take(queryParameters.PageCount);

            return await childrenItemWarehouses.ToListAsync();
        }

        public async Task<int> GetCountForChildrenItemWarehouses()
        {
            return await _context.ChildrenItemWarehouses.CountAsync();
        }

        public async Task<ChildrenItemWarehouse> GetChildrenItemWarehouseByChildrenItemIdAndWarehouseId(
                int itemId, int warehouseId)
        {
            return await _context.ChildrenItemWarehouses.Include(x => x.ChildrenItem).Include(x => x.Warehouse)
                .FirstOrDefaultAsync(x => x.ChildrenItemId == itemId && x.WarehouseId == warehouseId);
        }

        public async Task AddChildrenItemWarehouse(ChildrenItemWarehouse childrenItemWarehouse)
        {
            _context.ChildrenItemWarehouses.Add(childrenItemWarehouse);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateChildrenItemWarehouse(ChildrenItemWarehouse childrenItemWarehouse)
        {
            _context.Entry(childrenItemWarehouse).State = EntityState.Modified;        

            await _context.SaveChangesAsync();
        }

        public async Task<List<ChildrenItem>> GetAllChildrenItemsForChildrenItemWarehouses()
        {
            return await _context.ChildrenItems.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<Warehouse>> GetAllWarehousesForChildrenItemWarehouses()
        {
            return await _context.Warehouses.Include(x => x.Country).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task AddingNewStockQuantityToChildrenItem(ChildrenItem childrenItem)
        {
            childrenItem.StockQuantity = await _context.ChildrenItemWarehouses
                .Where(x => x.ChildrenItemId == childrenItem.Id)
                .SumAsync(x => x.StockQuantity);

            await _context.SaveChangesAsync();
        }


    }
}


