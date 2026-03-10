namespace DAL.Models
{
    public class RecipeIngredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public string Unity { get; set; }
        public string Quantity { get; set; }
    }
}
