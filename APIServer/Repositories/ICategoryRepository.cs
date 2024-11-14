using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(string id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(string id, Category updatedCategory);
        Task DeleteCategoryAsync(string id);
    }
}