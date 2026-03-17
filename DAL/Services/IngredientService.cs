using DAL.Models;
using DAL.Repositories;

namespace DAL.Services
{
    public class IngredientService
    {
        private IngredientRepository ingredientRepository;

        public IngredientService(IngredientRepository ingredientRepository)
        {
            this.ingredientRepository = ingredientRepository;
        }

        public List<Ingredient> GetAll()
        {
            return ingredientRepository.GetAll();
        }

        public void CreateIngredient(Ingredient ingredient)
        {
            ingredientRepository.Add(ingredient);
        }

        public bool IsRegistered(string name)
        {
            return ingredientRepository.IsRegistered(name);
        }

    }
}
