using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos.Identity;
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
    }
}