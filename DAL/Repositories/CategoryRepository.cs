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
    public class CategoryRepository
    {
        private readonly string _connString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Category>> GetAll()
        {
            List<Category> categories = new List<Category>();
            using SqlConnection conn = new SqlConnection(_connString);

            using SqlCommand cmd = new SqlCommand(
                "SELECT ID, Category FROM Categories",
                conn
            );

            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return categories;
        }


        public async Task Add(Category category)
        {

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "INSERT INTO Categories (category)" +
                "VALUES (@Category)",
                conn
            );

            cmd.Parameters.AddWithValue("@Category", category.Name);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }

        public async Task<bool> IsRegistered(string name)
        {
            using SqlConnection conn = new SqlConnection(_connString);

            using SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Categories WHERE Category = @Category",
                conn
            );

            cmd.Parameters.AddWithValue("@Category", name);

            await conn.OpenAsync();
            int count = (int)await cmd.ExecuteScalarAsync();
            return count > 0;
        }

    }
}
