using DAL.Models;
using DAL.Repositories;

namespace DAL.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository categoryRepository;

        public CategoryService(CategoryRepository _categoryRepository)
        {
            this.categoryRepository = _categoryRepository;
        }

        public async Task<List<Category>> GetAll()
        {
            return await categoryRepository.GetAll();
        }

        public async Task CreateCategory(Category category)
        {
            await categoryRepository.Add(category);
        }

        public async Task<bool> IsRegistered(string name)
        {
            return await categoryRepository.IsRegistered(name);
        }

        // Update

        // Delete

    }
}
