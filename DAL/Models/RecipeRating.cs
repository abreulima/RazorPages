namespace DAL.Models
{
    public class RecipeRating
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Unity { get; set; }
        public string Quantity { get; set; }
    }
}
