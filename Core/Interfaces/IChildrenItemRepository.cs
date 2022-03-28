using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.ChildrenItems;
using Core.Entities.Discounts;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IChildrenItemRepository
    {
        Task<List<ChildrenItem>> GetAllChildrenItems(QueryParameters queryParameters);
        Task<int> GetCountForChildrenItems();
        Task<List<ChildrenItem>> GetAllPureChildrenItems();
        Task<ChildrenItem> GetChildrenItemById(int id);
        Task<ChildrenItem> GetChildrenItemByIdWithoutInclude(int id);
        Task CreateChildernItem(ChildrenItem childrenItem);
        Task UpdateChildrenItem(ChildrenItem childrenItem);
        Task DeleteChildrenItem(ChildrenItem childrenItem);
        Task<List<Category>> GetAllCategories();
        Task<List<Discount>> GetAllDiscounts();
        Task<List<Manufacturer>> GetAllManufacturers();
        Task<List<Tag>> GetAllTags();
        Task<List<Category>> GetNonSelectedCategories(List<int> ids);
        Task<List<ChildrenItem>> GetNonSelectedChildrenItems(List<int> ids);
        Task<List<Discount>> GetNonSelectedDiscounts(List<int> ids);
        Task<List<Manufacturer>> GetNonSelectedManufacturers(List<int> ids);
        Task<List<Tag>> GetNonSelectedTags(List<int> ids);
        Task<List<Category>> GetCategoriesAssociatedWithChildrenItems();
        Task<List<Manufacturer>> GetManufacturersAssociatedWithChildrenItems();
        Task<List<Tag>> GetTagsAssociatedWithChildrenItems();
        Task UpdatingChildrenItemStockQuantityBasedOnWarehousesQuantity(List<ChildrenItem> childrenItems);
        Task ResetChildrenItemDiscountedPrice(ChildrenItem item);
    }
}