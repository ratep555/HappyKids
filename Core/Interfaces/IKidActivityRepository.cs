using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.BirthdayOrders;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IKidActivityRepository
    {
        Task<List<KidActivity>> GetAllKidActivities(QueryParameters queryParameters);
        Task<int> GetCountForKidActivities();
        Task<List<KidActivity>> GetAllPureKidActivities();
        Task<KidActivity> GetKidActivityById(int id);
        Task CreateKidActivity(KidActivity kidActivity);
        Task UpdateKidActivity(KidActivity kidActivity);
        Task DeleteKidActivity(KidActivity kidActivity);
    }
}







