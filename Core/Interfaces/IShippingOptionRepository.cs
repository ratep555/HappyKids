using System.Threading.Tasks;
using Core.Entities.Orders;

namespace Core.Interfaces
{
    public interface IShippingOptionRepository
    {
        Task<ShippingOption> GetShippingOptionById(int id);
        decimal GetShippingOptionPrice(int id);
    }
}