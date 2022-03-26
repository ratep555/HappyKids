using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos.Identity;
using Core.Dtos.StatisticsDtos;
using Core.Entities.Identity;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<UserToReturnDto>> GetAllUsers(QueryParameters queryParameters);
        Task<int> GetCountForUsers();
        Task<ApplicationUser> FindUserById(int id);
        Task LockUser(int id);
        Task UnlockUser(int id);   
        Task<List<ApplicationRole>> GetRolesAssociatedWithUsers();
        Task<StatisticsDto> ShowCountForEntities();
        Task<IEnumerable<BuyersPaymentOptionsChart>> GetNumberOfBuyersForEachPaymentOption();
        Task<IEnumerable<ClientOrderStatusesChart>> GetAllOrderStatusesForChildrenItems();
        Task<IEnumerable<BirthdayOrdersStatusesChart>> GetAllOrderStatusesForBirthdayOrders();
    }
}








