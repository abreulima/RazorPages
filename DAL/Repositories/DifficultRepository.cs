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

        public async Task AddAsync(Difficult difficult)
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

    }
}
