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

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await ingredientRepository.GetAllAsync();
        }

        // Get By Id

        public async Task CreateIngredientAysnc(Ingredient ingredient)
        {
            await ingredientRepository.AddAsync(ingredient);
        }

        // Update

        // Delete

    }
}
