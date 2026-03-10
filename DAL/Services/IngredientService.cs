using DAL.Models;
using DAL.Repositories;

namespace DAL.Services
{
    public class IngredientService
    {
        private readonly IngredientRepository ingredientRepository;

        public IngredientService(IngredientRepository ingredientRepository)
        {
            this.ingredientRepository = ingredientRepository;
        }

        public async Task<List<Ingredient>> GetAll()
        {
            return await ingredientRepository.GetAll();
        }

        public async Task CreateIngredient(Ingredient ingredient)
        {
            await ingredientRepository.Add(ingredient);

        }

        public async Task<bool> IsRegistered(string name)
        {
            return await ingredientRepository.IsRegistered(name);
        }

    }
}
