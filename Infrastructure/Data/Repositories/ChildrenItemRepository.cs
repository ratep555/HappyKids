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

        public async Task<int> GetCountForChildrenItems()
        {
            return await _context.ChildrenItems.CountAsync();
        }

        public async Task<ChildrenItem> GetChildrenItemById(int id)
        {
            return await _context.ChildrenItems.Include(x => x.ChildrenItemCategories).ThenInclude(x => x.Category)
                .Include(x => x.ChildrenItemDiscounts).ThenInclude(x => x.Discount)
                .Include(x => x.ChildrenItemManufacturers).ThenInclude(x => x.Manufacturer)
                .Include(x => x.ChildrenItemTags).ThenInclude(x => x.Tag).
            FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddChildernItem(ChildrenItem childrenItem)
        {
            _context.ChildrenItems.Add(childrenItem);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateChildrenItem(ChildrenItem childrenItem)
        {    
            _context.Entry(childrenItem).State = EntityState.Modified;        
             await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<Discount>> GetAllDiscounts()
        {
            return await _context.Discounts.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<Manufacturer>> GetAllManufacturers()
        {
            return await _context.Manufacturers.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<Tag>> GetAllTags()
        {
            return await _context.Tags.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<Category>> GetNonSelectedCategories(List<int> ids)
        {
            return await _context.Categories.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<Discount>> GetNonSelectedDiscounts(List<int> ids)
        {
            return await _context.Discounts.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<Manufacturer>> GetNonSelectedManufacturers(List<int> ids)
        {
            return await _context.Manufacturers.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }
    
        public async Task<List<Tag>> GetNonSelectedTags(List<int> ids)
        {
            return await _context.Tags.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesAssociatedWithChildrenItems()
        {
            var childrenItemCategories = await _context.ChildrenItemCategories.ToListAsync();

            IEnumerable<int> ids = childrenItemCategories.Select(x => x.CategoryId);

            var categories = await _context.Categories.Where(x => ids.Contains(x.Id))
                .OrderBy(x => x.Name).ToListAsync();

            return categories;
        }

        public async Task<List<Manufacturer>> GetManufacturersAssociatedWithChildrenItems()
        {
            var childrenItemManufacturers = await _context.ChildrenItemManufacturers.ToListAsync();

            IEnumerable<int> ids = childrenItemManufacturers.Select(x => x.ManufacturerId);

            var manufacturers = await _context.Manufacturers.Where(x => ids.Contains(x.Id))
                .OrderBy(x => x.Name).ToListAsync();

            return manufacturers;
        }

        public async Task<List<Tag>> GetTagsAssociatedWithChildrenItems()
        {
            var childernItemTags = await _context.ChildrenItemTags.ToListAsync();

            IEnumerable<int> ids = childernItemTags.Select(x => x.TagId);

            var tags = await _context.Tags.Where(x => ids.Contains(x.Id))
                .OrderBy(x => x.Name).ToListAsync();

            return tags;
        }

    }
}










