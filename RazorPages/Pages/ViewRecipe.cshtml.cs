using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages
{
    public class ViewRecipeModel : PageModel
    {

        private readonly RecipeService _recipeService;
        public Recipe? Recipe { get; set; }

        public ViewRecipeModel(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public IActionResult OnGet(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            
            Recipe = _recipeService.GetById(id, userId);
            if (Recipe == null)
                return RedirectToPage("/Index");


            return Page();
        }

        // Toggle do Favorito
        public IActionResult OnPostFavorite(int recipeId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            _recipeService.ToggleFavorite(userId.Value, recipeId);

            return RedirectToPage(new { id = recipeId });
        }

        // Atualiza ou Adiciona Rating
        public IActionResult OnPostRate(int recipeId, int rating)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            _recipeService.AddOrUpdateRating(userId.Value, recipeId, rating);

            return RedirectToPage(new { id = recipeId });
        }

        // Adiciona Commentario
        public IActionResult OnPostComment(int recipeId, string comment)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            _recipeService.AddComment(userId.Value, recipeId, comment);

            return RedirectToPage(new { id = recipeId });
        }
    }
}
