using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly HappyKidsContext _context;
        public CategoryRepository(HappyKidsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Shows all categories
        /// </summary>
        public async Task<List<Category>> GetAllCategories(QueryParameters queryParameters)
        {
            IQueryable<Category> categories = _context.Categories.AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                categories = categories.Where(t => t.Name.Contains(queryParameters.Query));
            }

            categories = categories.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await categories.ToListAsync();
        }

        /// <summary>
        /// This is for paging purposes, shows the total number of categories
        /// </summary>
        public async Task<int> GetCountForCategories()
        {
            return await _context.Categories.CountAsync();
        }

        /// <summary>
        /// Shows all the manufacturers, this is used in the process of creating/editing discounts
        /// See for example DiscountsController/GetAllCategoriesForDiscounts and add-discount.component.ts for more details
        /// </summary>
        public async Task<List<Category>> GetAllPureCategories()
        {
            return await _context.Categories.OrderBy(x => x.Name).ToListAsync();
        }

        /// <summary>
        /// Gets the corresponding category based on id 
        /// </summary>
        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Creates category
        /// </summary>
        public async Task CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();                    
        }

        /// <summary>
        /// Updates category
        /// </summary>
        public async Task UpdateCategory(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;  
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes category
        /// </summary>
        public async Task DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}