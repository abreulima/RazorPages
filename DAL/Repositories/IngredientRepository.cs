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

        public List<Ingredient> GetAll()
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "SELECT ID, Ingredient FROM Ingredients",
                conn
            );

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ingredients.Add(new Ingredient
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return ingredients;
        }

        public void Add(Ingredient ingredient)
        {

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "INSERT INTO Ingredients (ingredient)" +
                "VALUES (@Ingredient)",
                conn
            );

            cmd.Parameters.AddWithValue("@Ingredient", ingredient.Name);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public bool IsRegistered(string name)
        {
            using SqlConnection conn = new SqlConnection(_connString);

            using SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Ingredients WHERE Ingredient = @Ingredient",
                conn
            );

            cmd.Parameters.AddWithValue("@Ingredient", name);

            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

    }
}
