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

        public async Task<List<User>> GetAllAsync()
        {

            List<User> users = new List<User>();

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "SELECT ID, FirstName, LastName, Email, CreationDate FROM Users",
                conn
            );

            await conn.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        CreationDate = reader.GetDateTime(4)
                }
                );

            }

            return users;
        }

        public async Task AddAsync(Ingredient ingredient)
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

    }
}
