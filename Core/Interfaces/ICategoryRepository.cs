using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategories(QueryParameters queryParameters);
        Task<int> GetCountForCategories();
        Task<List<Category>> GetAllPureCategories();
        Task<Category> GetCategoryById(int id);
        Task CreateCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(Category category);
      
    }
}