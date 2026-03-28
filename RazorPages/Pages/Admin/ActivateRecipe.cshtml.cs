using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Admin
{
    public class ActivateRecipeModel : PageModel
    {

        private readonly RecipeService _recipeService;

        [BindProperty]
        public int RecipeId { get; set; }

        public List<Recipe> Recipes { get; set; } = new();

        public ActivateRecipeModel(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }


        public IActionResult OnGet()
        {
            string? isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "True")
                return RedirectToPage("/Login");

            Recipes = _recipeService.GetAllShort(true);

            return Page();
        }

        public IActionResult OnPost()
        {
            string? isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "True")
                return RedirectToPage("/Login");

            _recipeService.ApproveRecipe(RecipeId);
            return RedirectToPage();
        
        }
    }
}
