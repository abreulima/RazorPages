using DAL.Models;
using DAL.Repositories;

namespace DAL.Services
{
    public class RecipeService
    {
        private RecipeRepository _recipeRepository;
        private CategoryRepository _categoryRepository;
        private  DifficultRepository _difficultRepository;

        public RecipeService(RecipeRepository recipeRepository, CategoryRepository categoryRepository, DifficultRepository difficultRepository)
        {
            _recipeRepository = recipeRepository;
            _categoryRepository = categoryRepository;
            _difficultRepository = difficultRepository;
        }

        public List<Recipe> GetAll()
        {
            return _recipeRepository.GetAll();
        }

        public void Add(Recipe recipe)
        {
            _recipeRepository.Add(recipe);
        }

        // Update

        // Delete

    }
}
