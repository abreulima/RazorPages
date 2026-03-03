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

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await categoryRepository.GetAllAsync();
        }

        // Get By Id

        public async Task CreateCategoryAysnc(Category category)
        {
            await categoryRepository.AddAsync(category);
        }

        // Update

        // Delete

    }
}
