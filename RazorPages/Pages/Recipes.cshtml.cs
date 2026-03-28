using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages
{



    public class RecipesModel : PageModel
    {
        private readonly RecipeService _recipeService;
        public List<Recipe> Receitas { get; set; } = new List<Recipe>();
        public bool IsLogged { get; set; }

        // GET busca?
        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        [BindProperty]
        public int RecipeId { get; set; }

        public RecipesModel(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public void OnGet()
        {
            IsLogged = HttpContext.Session.GetInt32("UserId") != null;
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!string.IsNullOrEmpty(Search))
            {
                Receitas = _recipeService.SearchRecipes(Search, userId);
            }
            else
            {
                Receitas = _recipeService.GetTopRecipes(100, userId);
            }
        }

        public IActionResult OnPostFavorite()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            bool isFavorited = _recipeService.ToggleFavorite(userId.Value, RecipeId);

            // Inclui a busca na URL ao redirecionar
            if (!string.IsNullOrEmpty(Search))
            {
                return RedirectToPage("Recipes", new { search = Search });
            }
            else
            {
                return RedirectToPage("Recipes");
            }
        }


    }
}
