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

        public async Task<int> GetCountForCategories()
        {
            return await _context.Categories.CountAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();                    
        }

        public async Task UpdateCategory(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;  
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}