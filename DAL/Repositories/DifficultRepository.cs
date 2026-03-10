using DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace DAL.Repositories
{
    public class DifficultRepository
    {
        private readonly string _connString;

        public DifficultRepository(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Difficult>> GetAll()
        {
            List<Difficult> difficulties = new List<Difficult>();

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "SELECT ID, Difficult FROM Difficulties",
                conn
            );

            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                difficulties.Add(new Difficult
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return difficulties;
        }

        public async Task Add(Difficult difficult)
        {

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "INSERT INTO Difficulties (difficult)" +
                "VALUES (@Difficult)",
                conn
            );

            cmd.Parameters.AddWithValue("@Difficult", difficult.Name);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }


        public async Task<bool> IsRegistered(string name)
        {
            using SqlConnection conn = new SqlConnection(_connString);

            using SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Difficulties WHERE Dificult = @dificult",
                conn
            );

            cmd.Parameters.AddWithValue("@Dificult", name);

            await conn.OpenAsync();
            int count = (int)await cmd.ExecuteScalarAsync();
            return count > 0;

        }
    }
}
