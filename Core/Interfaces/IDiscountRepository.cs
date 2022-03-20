using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos.ChildrenItemsDtos;
using Core.Entities.ChildrenItems;
using Core.Entities.Discounts;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IDiscountRepository
    {
        Task<List<Discount>> GetAllDiscounts(QueryParameters queryParameters);
        Task<int> GetCountForDiscounts();
        Task<Discount> GetDiscountById(int id);
        Task CreateDiscount(Discount discount);  
        Task UpdateDiscount(Discount discount);
        Task DeleteDiscount(Discount discount);
        Task<decimal> DiscountSum(ChildrenItem childrenItem);
        Task<decimal> DiscountSumForDto(ChildrenItemDto childrenItem);
        Task UpdateChildrenItemWithDiscount(ChildrenItem item);
        Task UpdateChildrenItemWithDiscount1(Discount discount);
        Task ResetCategoryDiscountedPrice(Discount discount);
        Task ResetChildrenItemDiscountedPrice(Discount discount);
        Task ResetManufacturerDiscountedPrice(Discount discount);
        Task ResetCategoryDiscountedPriceDueToDiscountExpiry(IEnumerable<ChildrenItem> items);
        Task ResetChildrenItemDiscountedPriceDueToDiscountExpiry(IEnumerable<ChildrenItem> childrenItems);
        Task ResetManufacturerDiscountedPriceDueToDiscountExpiry(IEnumerable<ChildrenItem> items);
        Task UpdateChildrenItemWithCategoryDiscount(Discount discount);
        Task UpdateChildrenItemWithManufacturerDiscount(Discount discount);
    }
}

