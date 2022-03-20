using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.ChildrenItems;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IChildrenItemWarehouseRepository
    {
        Task<List<ChildrenItemWarehouse>> GetAllChildrenItemWarehouses(QueryParameters queryParameters);
        Task<int> GetCountForChildrenItemWarehouses();
        Task<ChildrenItemWarehouse> GetChildrenItemWarehouseByChildrenItemIdAndWarehouseId(int itemId, int warehouseId);
        Task AddChildrenItemWarehouse(ChildrenItemWarehouse childrenItemWarehouse);
        Task UpdateChildrenItemWarehouse(ChildrenItemWarehouse childrenItemWarehouse);
        Task<List<Warehouse>> GetAllWarehousesForChildrenItemWarehouses();
        Task AddingNewStockQuantityToChildrenItem(ChildrenItem childrenItem);
        Task DecreasingChildrenItemWarehousesQuantity(int id, int quantity);
        Task DecreasingChildrenItemWarehousesQuantity1(int id, int quantity);
        Task RemovingReservedQuantityFromChildrenItemWarehouses(int itemId, int quantity);
        Task IncreasingChildrenItemWarehousesQuantity(int id, int quantity);
        Task<bool> CheckIfChildrenItemWarehouseAlreadyExists(int childrenItemId, int warehouseId);
    }
}








