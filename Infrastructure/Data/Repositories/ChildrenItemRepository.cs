using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.ChildrenItems;
using Core.Entities.Discounts;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ChildrenItemRepository : IChildrenItemRepository
    {
        private readonly HappyKidsContext _context;
        public ChildrenItemRepository(HappyKidsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Shows all children items
        /// </summary>
        public async Task<List<ChildrenItem>> GetAllChildrenItems(QueryParameters queryParameters)
        {
            var childrenItemManufacturers = await _context.ChildrenItemManufacturers
                .Where(x => x.ManufacturerId == queryParameters.ManufacturerId).ToListAsync();
            
            IEnumerable<int> ids = childrenItemManufacturers.Select(x => x.ChildrenItemId);

            var childrenItemTags = await _context.ChildrenItemTags
                .Where(x => x.TagId == queryParameters.TagId).ToListAsync();
            
            IEnumerable<int> ids1 = childrenItemTags.Select(x => x.ChildrenItemId);

            var childrenItemCategories = await _context.ChildrenItemCategories
                .Where(x => x.CategoryId == queryParameters.CategoryId).ToListAsync();
            
            IEnumerable<int> ids2 = childrenItemCategories.Select(x => x.ChildrenItemId);

            IQueryable<ChildrenItem> childrenItems = _context.ChildrenItems.Include(x => x.ChildrenItemDiscounts)
                .ThenInclude(x => x.Discount).Include(x => x.ChildrenItemCategories).ThenInclude(x => x.Category)
                .Include(x => x.ChildrenItemTags).ThenInclude(x => x.Tag)
                .Include(x => x.ChildrenItemManufacturers).ThenInclude(x => x.Manufacturer)
                .AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                childrenItems = childrenItems.Where(t => t.Name.Contains(queryParameters.Query));
            }
            if (queryParameters.ManufacturerId.HasValue)
            {
                childrenItems = childrenItems.Where(x => ids.Contains(x.Id));
            }
            if (queryParameters.TagId.HasValue)
            {
                childrenItems = childrenItems.Where(x => ids1.Contains(x.Id));
            }
            if (queryParameters.CategoryId.HasValue)
            {
                childrenItems = childrenItems.Where(x => ids2.Contains(x.Id));
            }

            childrenItems = childrenItems.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await childrenItems.ToListAsync();
        }

        /// <summary>
        /// This is for paging purposes, shows the total number of children items
        /// </summary>
        public async Task<int> GetCountForChildrenItems()
        {
            return await _context.ChildrenItems.CountAsync();
        }

        /// <summary>
        /// Gets the corresponding children item based on id 
        /// </summary>
        public async Task<ChildrenItem> GetChildrenItemById(int id)
        {
            return await _context.ChildrenItems.Include(x => x.ChildrenItemCategories).ThenInclude(x => x.Category)
                .Include(x => x.ChildrenItemDiscounts).ThenInclude(x => x.Discount)
                .Include(x => x.ChildrenItemManufacturers).ThenInclude(x => x.Manufacturer)
                .Include(x => x.ChildrenItemTags).ThenInclude(x => x.Tag).
            FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Used for resetting prices due to discount expiry
        /// See ChildrenItemsController/GetAllChildrenItems for more details
        /// </summary>
        public async Task<List<ChildrenItem>> GetAllPureChildrenItems()
        {
            return await _context.ChildrenItems.OrderBy(x => x.Name).ToListAsync();
        }

        /// <summary>
        /// Gets the corresponding children item based on id 
        /// Used for optimisation of query
        /// See ChildrenItemsController/IncreaseChildrenItemStockQuantity or OrderService/CreateOrder for more details
        /// </summary>
        public async Task<ChildrenItem> GetChildrenItemByIdWithoutInclude(int id)
        {
            return await _context.ChildrenItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Creates children item 
        /// </summary>
        public async Task CreateChildernItem(ChildrenItem childrenItem)
        {
            _context.ChildrenItems.Add(childrenItem);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates children item warehouse
        /// </summary>
        public async Task UpdateChildrenItem(ChildrenItem childrenItem)
        {    
            _context.Entry(childrenItem).State = EntityState.Modified;        
             await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes children item
        /// </summary>
        public async Task DeleteChildrenItem(ChildrenItem childrenItem)
        {
            _context.ChildrenItems.Remove(childrenItem);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Shows all categories
        /// Used to show dropdown list while creating/editing children item
        /// See ChildrenItemsController/GetAllCategories and add-childrenitem.component.ts for more details
        /// </summary>
        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.OrderBy(x => x.Name).ToListAsync();
        }
        
        /// <summary>
        /// Shows all discounts
        /// Used to show dropdown list while creating/editing children item
        /// See ChildrenItemsController/GetAllDiscounts and add-childrenitem.component.ts for more details
        /// </summary>
        public async Task<List<Discount>> GetAllDiscounts()
        {
            return await _context.Discounts.OrderBy(x => x.Name).ToListAsync();
        }

        /// <summary>
        /// Shows all manufacturers
        /// Used to show dropdown list while creating/editing children item
        /// See ChildrenItemsController/GetAllManufacturers and add-childrenitem.component.ts for more details
        /// </summary>
        public async Task<List<Manufacturer>> GetAllManufacturers()
        {
            return await _context.Manufacturers.OrderBy(x => x.Name).ToListAsync();
        }

        /// <summary>
        /// Shows all tags
        /// Used to show dropdown list while creating/editing children item
        /// See ChildrenItemsController/GetAllTags and add-childrenitem.component.ts for more details
        /// </summary>
        public async Task<List<Tag>> GetAllTags()
        {
            return await _context.Tags.OrderBy(x => x.Name).ToListAsync();
        }

        /// <summary>
        /// Shows categories that were not selected while creating children item
        /// Used for rendering dropdown list of non selected categories while editing children item
        /// See ChildrenItemsController/PutGetChildrenItem and edit-childrenitem.component.ts for more details
        /// </summary>
        public async Task<List<Category>> GetNonSelectedCategories(List<int> ids)
        {
            return await _context.Categories.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }

        /// <summary>
        /// Shows children item,s that were not selected while creating discount
        /// Used for rendering dropdown list of non selected children items while editing discount
        /// See DiscountsController/PutGetDiscount and edit-discount.component.ts for more details
        /// </summary>
        public async Task<List<ChildrenItem>> GetNonSelectedChildrenItems(List<int> ids)
        {
            return await _context.ChildrenItems.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }

        /// <summary>
        /// Shows discounts that were not selected while creating children item
        /// Used for rendering dropdown list of non selected discounts while editing children item
        /// See ChildrenItemsController/PutGetChildrenItem and edit-childrenitem.component.ts for more details
        /// </summary>
        public async Task<List<Discount>> GetNonSelectedDiscounts(List<int> ids)
        {
            return await _context.Discounts.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }

        /// <summary>
        /// Shows manufacturers that were not selected while creating children item
        /// Used for rendering dropdown list of non selected manufacturers while editing children item
        /// See ChildrenItemsController/PutGetChildrenItem and edit-childrenitem.component.ts for more details
        /// </summary>
        public async Task<List<Manufacturer>> GetNonSelectedManufacturers(List<int> ids)
        {
            return await _context.Manufacturers.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }
    
        /// <summary>
        /// Shows tags that were not selected while creating children item
        /// Used for presenting dropdown list of non selected tags while editing children item
        /// See ChildrenItemsController/PutGetChildrenItem and edit-childrenitem.component.ts for more details
        /// </summary>
        public async Task<List<Tag>> GetNonSelectedTags(List<int> ids)
        {
            return await _context.Tags.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }

        /// <summary>
        /// Used (while filtering) for presenting dropdown list of all the categories associated with children items
        /// See ChildrenItemsController/GetCategoriesAssociatedWithChildrenItems and webshop.component.ts for more details
        /// </summary>
        public async Task<List<Category>> GetCategoriesAssociatedWithChildrenItems()
        {
            var childrenItemCategories = await _context.ChildrenItemCategories.ToListAsync();

            IEnumerable<int> ids = childrenItemCategories.Select(x => x.CategoryId);

            var categories = await _context.Categories.Where(x => ids.Contains(x.Id))
                .OrderBy(x => x.Name).ToListAsync();

            return categories;
        }

        /// <summary>
        /// Used (while filtering) for presenting dropdown list of all the manufacturers associated with children items
        /// See ChildrenItemsController/GetCategoriesAssociatedWithChildrenItems and webshop.component.ts for more details
        /// </summary>
        public async Task<List<Manufacturer>> GetManufacturersAssociatedWithChildrenItems()
        {
            var childrenItemManufacturers = await _context.ChildrenItemManufacturers.ToListAsync();

            IEnumerable<int> ids = childrenItemManufacturers.Select(x => x.ManufacturerId);

            var manufacturers = await _context.Manufacturers.Where(x => ids.Contains(x.Id))
                .OrderBy(x => x.Name).ToListAsync();

            return manufacturers;
        }

        /// <summary>
        /// Used (while filtering) for presenting dropdown list of all the tags associated with children items
        /// See ChildrenItemsController/GetCategoriesAssociatedWithChildrenItems and webshop.component.ts for more details
        /// </summary>
        public async Task<List<Tag>> GetTagsAssociatedWithChildrenItems()
        {
            var childernItemTags = await _context.ChildrenItemTags.ToListAsync();

            IEnumerable<int> ids = childernItemTags.Select(x => x.TagId);

            var tags = await _context.Tags.Where(x => ids.Contains(x.Id))
                .OrderBy(x => x.Name).ToListAsync();

            return tags;
        }

        /// <summary>
        /// Updates children item quantity based on children item warehouse stock quantity 
        /// We use this when user retrieves the list of children items
        /// See ChildrenItemsController/GetAllChildrenItems for more details
        /// </summary>
        public async Task UpdatingChildrenItemStockQuantityBasedOnWarehousesQuantity(List<ChildrenItem> childrenItems)
        {
            IEnumerable<int> ids = childrenItems.Select(x => x.Id);

            foreach (var item in childrenItems)
            {
                
                item.StockQuantity = await _context.ChildrenItemWarehouses
                    .Where(x => ids.Contains(x.ChildrenItemId) && x.ChildrenItemId == item.Id)
                    .SumAsync(x => x.StockQuantity);
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates children item discounted price while updating children item
        /// See ChildrenItemsController/UpdateChildrenItem for more details
        /// </summary>
        public async Task ResetChildrenItemDiscountedPrice(ChildrenItem item)
        {     
            var childrenItemDiscounts = await _context.ChildrenItemDiscounts
                .Where(x => x.ChildrenItemId == item.Id).ToListAsync();

            IEnumerable<int> ids1 = childrenItemDiscounts.Select(x => x.ChildrenItemId);

            var list = await _context.ChildrenItems.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {
                foreach (var product in list)
                {
                    var discountPercentage = await _context.ChildrenItemDiscounts
                        .Where(x => x.ChildrenItemId == item.Id && x.ChildrenItemId == product.Id).FirstOrDefaultAsync();

                    decimal discountPercentage1 = discountPercentage.Discount.DiscountPercentage;

                    var discountAmount = (discountPercentage1 / 100) * item.Price;
                
                    if  (discountAmount > 0)
                        {          
                            item.DiscountedPrice = item.DiscountedPrice + discountAmount;

                            if (item.DiscountedPrice == item.Price)
                            {
                                item.DiscountedPrice = null;
                                item.HasDiscountsApplied = null;
                            }
                        }

                    _context.Entry(item).State = EntityState.Modified;        
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}










