using MongoDB.Driver;
using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public class CategoryRepositoryMongoDB : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryRepositoryMongoDB(IMongoDatabase database)
        {
            _categories = database.GetCollection<Category>("Category"); // Samlingens navn i MongoDB
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _categories.Find(category => true).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            return await _categories.Find(category => category.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categories.InsertOneAsync(category);
        }

        public async Task UpdateCategoryAsync(string id, Category updatedCategory)
        {
            await _categories.ReplaceOneAsync(category => category.Id == id, updatedCategory);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _categories.DeleteOneAsync(category => category.Id == id);
        }
    }
}