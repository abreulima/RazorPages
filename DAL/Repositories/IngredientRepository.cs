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
    public class IngredientRepository
    {
        private readonly string _connString;

        public IngredientRepository(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Ingredient>> GetAll()
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "SELECT ID, Ingredient FROM Ingredients",
                conn
            );

            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                ingredients.Add(new Ingredient
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return ingredients;
        }

        public async Task Add(Ingredient ingredient)
        {

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "INSERT INTO Ingredients (ingredient)" +
                "VALUES (@Ingredient)",
                conn
            );

            cmd.Parameters.AddWithValue("@Ingredient", ingredient.Name);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }

        public async Task<bool> IsRegistered(string name)
        {
            using SqlConnection conn = new SqlConnection(_connString);

            using SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Ingredients WHERE Ingredient = @Ingredient",
                conn
            );

            cmd.Parameters.AddWithValue("@Ingredient", name);

            await conn.OpenAsync();
            int count = (int)await cmd.ExecuteScalarAsync();
            return count > 0;
        }

    }
}
