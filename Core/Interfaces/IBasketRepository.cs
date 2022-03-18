using System.Threading.Tasks;
using Core.Entities.ClientBaskets;

namespace Core.Interfaces
{
    public interface IBasketRepository
    {
        Task<ClientBasket> GetClientBasket(string basketId);
        Task<ClientBasket> UpdateClientBasket(ClientBasket basket);
        Task<bool> DeleteClientBasket(string basketId);
    }
}