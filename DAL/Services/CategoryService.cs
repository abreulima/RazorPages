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

        public List<Category> GetAll()
        {
            return categoryRepository.GetAll();
        }

        public void CreateCategory(Category category)
        {
            categoryRepository.Add(category);
        }

        public bool IsRegistered(string name)
        {
            return categoryRepository.IsRegistered(name);
        }

        // Update

        // Delete

    }
}
