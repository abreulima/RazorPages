namespace DAL.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Title { get; set; }
        public string Preparation { get; set; }
        public int CategoryId { get; set; }
        public int DifficultId { get; set; }
        public DateTime CreationDate { get; set; }
        public List<RecipeIngredient> Ingredients { get; set; } = new();
    }
}
