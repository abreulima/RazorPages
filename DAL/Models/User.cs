namespace DAL.Models
{
    public class User
    {
        public int      Id { get; set; }
        public string   FirstName { get; set; }
        public string   LastName { get; set; }
        public string   Password { get; set; }
        public string   Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
