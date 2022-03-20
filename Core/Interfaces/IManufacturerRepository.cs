using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IManufacturerRepository
    {
         
        Task<List<Manufacturer>> GetAllManufacturers(QueryParameters queryParameters);
        Task<int> GetCountForManufacturers();
        Task<List<Manufacturer>> GetAllPureManufacturers();
        Task<Manufacturer> GetManufacturerById(int id);
        Task CreateManufacturer(Manufacturer manufacturer);
        Task UpdateManufacturer(Manufacturer manufacturer);
        Task DeleteManufacturer(Manufacturer manufacturer);
    }
}




