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

        public async Task DecreasingChildrenItemWarehousesQuantity(int id, int quantity)
        {
            var list = await _context.ChildrenItemWarehouses
                .Where(x => x.ChildrenItemId == id && x.StockQuantity > 0).ToListAsync();

            foreach (var item in list)
            {
                int? reservedquantity  = item.ReservedQuantity;
                    
                if (item.StockQuantity >= quantity)
                {
                    item.StockQuantity = item.StockQuantity - quantity;
                    quantity = 0;
                }

                else if(item.StockQuantity < quantity)
                {
                    var model = await _context.ChildrenItemWarehouses
                        .FirstOrDefaultAsync(x => x.ChildrenItemId == item.ChildrenItemId && x.StockQuantity > 0);  

                    model.StockQuantity = model.StockQuantity - quantity;

                    await _context.SaveChangesAsync();          
                }
                quantity = 0;
            }

            var model1 = await _context.ChildrenItemWarehouses
                .FirstOrDefaultAsync(X => X.ChildrenItemId == id && X.StockQuantity > 0);

            model1.ReservedQuantity = ++model1.ReservedQuantity ?? 1;
            
            await _context.SaveChangesAsync();
        }

        public async Task DecreasingChildrenItemWarehousesQuantity1(int id, int quantity)
        {
            var list = await _context.ChildrenItemWarehouses
                .Where(x => x.ChildrenItemId == id && x.StockQuantity > 0).ToListAsync();

            foreach (var item in list)
            {
                int result = 0;

                if (quantity > 1)
                {
                    if (item.StockQuantity >= 0)
                    {
                        if (item.StockQuantity >= quantity)
                        {
                            item.StockQuantity = item.StockQuantity - quantity;
                            item.ReservedQuantity = item.ReservedQuantity + quantity ?? quantity;
                            await _context.SaveChangesAsync();
                        }
                        else if (item.StockQuantity < quantity)
                        {
                            item.ReservedQuantity = item.ReservedQuantity + item.StockQuantity ?? item.StockQuantity;
                            result = quantity - item.StockQuantity; 
                            item.StockQuantity = 0;
                            await _context.SaveChangesAsync();
                            
                            var model = await _context.ChildrenItemWarehouses
                                .FirstOrDefaultAsync(x => x.StockQuantity > 0);
                            
                            model.StockQuantity = model.StockQuantity - result;
                            model.ReservedQuantity = model.ReservedQuantity + result ?? result;
                            await _context.SaveChangesAsync();
                        }
                        quantity = 0;
                    }
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemovingReservedQuantityFromChildrenItemWarehouses(int itemId, int quantity)
        {
            var list = await _context.ChildrenItemWarehouses.Where(x => x.ChildrenItemId == itemId
                && x.ReservedQuantity != null && x.ReservedQuantity > 0).ToListAsync();

            if (quantity > 1)
            {
                foreach (var item in list)
                {
                    item.StockQuantity = item.StockQuantity += (int)item.ReservedQuantity;
                    item.ReservedQuantity = 0;
                }
                await _context.SaveChangesAsync();
            }
        }

         public async Task IncreasingChildrenItemWarehousesQuantity(int id, int quantity)
        {
            var list = await _context.ChildrenItemWarehouses.Where(x => x.ChildrenItemId == id
                && x.ReservedQuantity != null && x.ReservedQuantity > 0).ToListAsync();

            foreach (var item in list)
            {
                    if (item.StockQuantity != 0)
                    {
                        item.StockQuantity = item.StockQuantity += quantity;
                        item.ReservedQuantity = item.ReservedQuantity - quantity;
                        await _context.SaveChangesAsync();
                        
                    }
                    else if (item.StockQuantity == 0)
                    {
                        item.StockQuantity = item.StockQuantity += quantity;
                        item.ReservedQuantity = item.ReservedQuantity - quantity;
                        await _context.SaveChangesAsync();          
                    }
                    quantity = 0;
            }
        }
        
        public async Task<bool> CheckIfChildrenItemWarehouseAlreadyExists(int childrenItemId, int warehouseId)
        {
            var childrenItemWarehouses = await _context.ChildrenItemWarehouses
                .Where(x => x.ChildrenItemId == childrenItemId && x.WarehouseId == warehouseId).ToListAsync();
            
            if (childrenItemWarehouses.Any()) return true;

            return false;
        }
    }
}










