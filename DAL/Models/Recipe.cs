namespace DAL.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string CreatorName {get; set ;}
        public string Title { get; set; }
        public string Preparation { get; set; }
        public int CategoryId { get; set; }
        public int DifficultId { get; set; }
        public string CategoryName { get; set; }
        public string DifficultyName { get; set; }
        public bool isFavoritedByUser { get; set; }
        public bool isRatedByUser {get; set ;}
        public int? UserRating { get; set; }
        public double CalculatedRatings { get; set; }
        public int TotalRatings { get; set; }
        public bool IsApproved { get; set; }
        public List<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
        public List<RecipeRating> Ratings { get; set; } = new List<RecipeRating>();
        public List<RecipeComment> Comments { get; set; } = new List<RecipeComment>();
        public DateTime CreationDate { get; set; }
    }
}
