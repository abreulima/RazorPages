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

        public List<Difficult> GetAll()
        {
            List<Difficult> difficulties = new List<Difficult>();

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "SELECT ID, Difficult FROM Difficulties",
                conn
            );

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                difficulties.Add(new Difficult
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return difficulties;
        }

        public void Add(Difficult difficult)
        {

            using SqlConnection conn = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand(
                "INSERT INTO Difficulties (difficult)" +
                "VALUES (@Difficult)",
                conn
            );

            cmd.Parameters.AddWithValue("@Difficult", difficult.Name);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }


        public bool IsRegistered(string name)
        {
            using SqlConnection conn = new SqlConnection(_connString);

            using SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Difficulties WHERE Dificult = @dificult",
                conn
            );

            cmd.Parameters.AddWithValue("@Dificult", name);

            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            return count > 0;

        }
    }
}
