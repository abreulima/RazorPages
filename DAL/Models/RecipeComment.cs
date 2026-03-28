public class RecipeComment
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}