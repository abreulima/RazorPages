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

        /*  Classe redudante, claramente nao estou a implementar a camada de servicos como devem ser
         *  1. Retirar do repositorio a responsabilidade de realizar a logico do negocio
         *  o repositorio, tem como objetivo ser um repositorio! -> Lugar onde se deposita ou guarda alguma coisa.
         * 
         *  2. Usa o Linq para dessasociar (um pouco mais) o SQL Server da nossa aplicacao. O repositorio fornece as receitas, 
         *  e depois e utilizada o linq para filtrar os dados na camada de servico.
         */

        public List<Recipe> GetAllShort(bool isPending)
        {
            return _recipeRepository.GetAllShort(isPending);
        }

        public void Add(Recipe recipe)
        {
            _recipeRepository.Add(recipe);
        }

        public Recipe? GetById(int id, int? userId = null)
        {
            return _recipeRepository.GetById(id, userId);
        }

        public List<Recipe> GetTopRecipes(int count, int? userId = null)
        {
            return _recipeRepository.GetTopRecipes(count, userId);
        }

        public bool ToggleFavorite(int userId, int recipeId)
        {
            return _recipeRepository.ToggleFavorite(userId, recipeId);
        }

        public List<Recipe> GetFavoritesByUser(int? userId)
        {
            return _recipeRepository.GetFavoritesByUser(userId);
        }

        public void AddOrUpdateRating(int userId, int recipeId, int rating)
        {
            // A UI deixa apenas o usuario selecionar entre 1 a 5, 
            // maas ele poderia facilmente enviar qualquer coisa 
            if (rating < 0 || rating > 5)
                return;

            _recipeRepository.AddOrUpdateRating(userId, recipeId, rating);
        }

        public void AddComment(int userId, int recipeId, string comment)
        {
            _recipeRepository.AddComment(userId, recipeId, comment);
        }

        public void ApproveRecipe(int id)
        {
            _recipeRepository.ApproveRecipe(id);
        }

        public List<Recipe> SearchRecipes(string search, int? userId)
        {
            return _recipeRepository.Search(search, userId);
        }


    }
}
