using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IWarehouseRepository
    {
        Task<List<Warehouse>> GetAllWarehouses(QueryParameters queryParameters);
        Task<int> GetCountForWarehouses();
        Task<Warehouse> GetWarehouseById(int id);
        Task CreateWarehouse(Warehouse warehouse);
        Task UpdateWarehouse(Warehouse warehouse);
        Task DeleteWarehouse(Warehouse warehouse);
        Task<List<Country>> GetAllCountries();
    }
}