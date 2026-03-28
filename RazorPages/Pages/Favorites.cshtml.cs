using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages
{
    public class FavoritesModel : PageModel
    {


        private readonly RecipeService _recipeService;

        public FavoritesModel(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
        public IActionResult OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");
            
            Recipes = _recipeService.GetFavoritesByUser(userId);

            return Page();
        }

        public IActionResult OnPostFavorite(int recipeId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            _recipeService.ToggleFavorite(userId.Value, recipeId);

            return RedirectToPage();
        }
    }
}
