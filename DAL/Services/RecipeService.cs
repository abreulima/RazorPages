using DAL.Models;
using DAL.Repositories;

namespace DAL.Services
{
    public class RecipeService
    {
        private readonly RecipeRepository _recipeRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly DifficultRepository _difficultRepository;

        public RecipeService(RecipeRepository _recipeRepository, CategoryRepository _categoryRepository, DifficultRepository _difficultRepository)
        {
            _recipeRepository = _recipeRepository;
            _categoryRepository = _categoryRepository;
            _difficultRepository = _difficultRepository;
        }

        public async Task<List<Recipe>> GetAll()
        {
            return await _recipeRepository.GetAll();
        }

        public async Task Add(Recipe recipe)
        {
            await _recipeRepository.Add(recipe);
        }

        // Update

        // Delete

    }
}
