using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Orders;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IShippingOptionRepository
    {
        Task<List<ShippingOption>> GetAllShippingOptions(QueryParameters queryParameters);
        Task<int> GetCountForShippingOptions();
        Task<ShippingOption> GetShippingOptionById(int id);
        decimal GetShippingOptionPrice(int id);
        Task CreateShippingOption(ShippingOption shippingOption);
        Task UpdateShippingOption(ShippingOption shippingOption);
        Task DeleteShippingOption(ShippingOption shippingOption);
       
    }
}