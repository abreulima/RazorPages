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
    public class UserRepository
    {
        private readonly string _connString;

        public UserRepository(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<User>> GetAllAsync()
        {

            List<User> users = new List<User>();

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "SELECT Id, Name, Email, CreationDate FROM Users",
                conn
            );

            await conn.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2),
                        CreationDate = reader.GetDateTime(3)
                    }
                );

            }

            return users;
        }

        public async Task AddAsync(User user)
        {

            using SqlConnection conn = new SqlConnection("_connString");
            using SqlCommand cmd = new SqlCommand(
                "INSERT INTO Users (Name, Email, Password) FROM Users",
                conn
            );

            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", PasswordHelper.Md5(user.Password));

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

    }
}
