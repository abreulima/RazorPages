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

        public List<User> GetAll()
        {

            List<User> users = new List<User>();

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "SELECT ID, FirstName, LastName, Email, CreationDate FROM Users",
                conn
            );

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
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

        public void Add(User user)
        {

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "INSERT INTO Users (firstname, lastName, email, password)" +
                " VALUES (@FirstName, @LastName, @Email, @Password)",
                conn
            );

            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            //cmd.Parameters.AddWithValue("@Password", PasswordHelper.Md5(user.Password));
            cmd.Parameters.AddWithValue("@Password", user.Password);


            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public User? GetUserLogin(string email, string password)
        {
            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "SELECT ID, FirstName, LastName, Email, isAdmin " +
                "FROM Users WHERE Email = @Email AND Password = @Password",
                conn
            );
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);
            //cmd.Parameters.AddWithValue("@Password", PasswordHelper.Md5(password));

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    IsAdmin = reader.GetBoolean(4),
                    //CreationDate = reader.GetDateTime(4)
                };
            }
            return null;

        }

        public bool IsRegistered(string email)
        {
            using SqlConnection conn = new SqlConnection(_connString);

            using SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Users WHERE Email = @Email",
                conn
            );

            cmd.Parameters.AddWithValue("@Email", email);

            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }


    }
}
