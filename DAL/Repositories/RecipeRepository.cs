using DAL.Models;

// Palavras chaves sao armazenadas criptografadas, com md5 (nao eh + recomandavel)
using DAL.Helpers;

// Para uso do Banco de Dadose
using Microsoft.Data.SqlClient;

// Para ler do arquivo settings.json
using Microsoft.Extensions.Configuration;

/*
O que temos?
Pegar todos Users
Adicionar User

O que falta?
Get By Id
Delete (?)
Update
*/

namespace DAL.Repositories
{
    public class RecipeRepository
    {
        private readonly string _connString;

        public RecipeRepository(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Recipe> GetAll()
        {
            List<Recipe> recipe = new List<Recipe>();

            return recipe;
        }

        public void Add(Recipe recipe)
        {

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "INSERT INTO Recipes (creator, title, preparation, categoryID, difficultID)" +
                " VALUES (@Creator, @Title, @Preparation, @CategoryId, @DifficultId)",
                conn
            );

            cmd.Parameters.AddWithValue("@Creator", recipe.CreatorId);
            cmd.Parameters.AddWithValue("@Title", recipe.Title);
            cmd.Parameters.AddWithValue("@Preparation", recipe.Preparation);
            cmd.Parameters.AddWithValue("@CategoryId", recipe.CategoryId);
            cmd.Parameters.AddWithValue("@DifficultId", recipe.DifficultId);

            conn.Open();
            int recipeId = Convert.ToInt32(cmd.ExecuteScalar());

            foreach (var ingredient in recipe.Ingredients)
            {
                using SqlCommand ingCmd = new SqlCommand(
                    "INSERT INTO IngredientsRecipes (recipeID, ingredientId, unity, quantity) " +
                    "VALUES (@RecipeId, @IngredientId, @Unity, @Quantity)",
                    conn
                );
                ingCmd.Parameters.AddWithValue("@RecipeId", recipeId);
                ingCmd.Parameters.AddWithValue("@IngredientId", ingredient.IngredientId);
                ingCmd.Parameters.AddWithValue("@Unity", ingredient.Unity);
                ingCmd.Parameters.AddWithValue("@Quantity", ingredient.Quantity);
                ingCmd.ExecuteNonQuery();
            }

            conn.Close();
        }

    }
}
