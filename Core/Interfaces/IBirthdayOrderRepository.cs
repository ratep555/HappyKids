using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.BirthdayOrders;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IBirthdayOrderRepository
    {
        Task<List<ClientBirthdayOrder>> GetAllBirthdayOrders(QueryParameters queryParameters);
        Task<int> GetCountForBirthdayOrders();
        Task<ClientBirthdayOrder> GetBirthdayOrderById(int id);
        Task AddBirthdayOrder(ClientBirthdayOrder birthdayOrder);
        Task<decimal> DiscountedAdditionalBillingPerParticipant(BirthdayPackage birthdayPackage);
    }
}