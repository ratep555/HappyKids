using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos.BirthdayOrdersDtos;
using Core.Entities.BirthdayOrders;
using Core.Entities.Discounts;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IBirthdayPackageRepository
    {
        Task<List<BirthdayPackage>> GetAllBirthdayPackages(QueryParameters queryParameters);
        Task<int> GetCountForBirthdayPackages();
        Task<List<BirthdayPackage>> GetAllPureBirthdayPackages();
        Task<BirthdayPackage> GetBirthdayPackageById(int id);
        Task CreateBirthdayPackage(BirthdayPackage package);
        Task UpdateBirthdayPackage(BirthdayPackage birthdayPackage);
        Task DeleteBirthdayPackage(BirthdayPackage birthdayPackage);
        Task UpdateBirthdayPackageWithDiscount(BirthdayPackage birthdayPackage);
        Task ResetBirthdayPackageDiscountedPriceDueToDiscountExpiry(IEnumerable<BirthdayPackage> birthdayPackages);
        Task ResetBirthdayPackageDiscountedPrice(BirthdayPackage birthdayPackage);
        Task<decimal> DiscountSum(BirthdayPackage birthdayPackage);        
        Task DiscountSumForDto(IEnumerable<BirthdayPackage> birthdayPackages, IEnumerable<BirthdayPackageDto> birthdayPackagesDto);
        Task<List<KidActivity>> GetNonSelectedKidActivities(List<int> ids);
    }
}