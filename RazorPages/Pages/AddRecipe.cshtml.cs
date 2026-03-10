using System.ComponentModel.DataAnnotations;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace RazorPages.Pages
{
    public class IngredientInput
    {
        public int IngredientId { get; set; }
        public string Quantity { get; set; }
        public string Unity { get; set; }
    }

    public class AddRecipeModel : PageModel
    {

        private readonly IngredientService _ingredientService;
        private readonly RecipeService _recipeService;

        [BindProperty]
        [Required]
        public string Title { get; set; }

        [BindProperty]
        [Required]
        public string Preparation { get; set; }

        [BindProperty]
        [Required]
        public int CategoryId { get; set; }

        [BindProperty]
        [Required]
        public int DifficultId { get; set; }


        [BindProperty] public List<IngredientInput> ingredientesInput { get; set; } = new();


        public List<Ingredient> AvailableIngredients { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Difficult> Difficulties { get; set; } = new();
        public bool Error { get; set; } = false;


        public AddRecipeModel(RecipeService recipeService, IngredientService ingredientService)
        {
            _recipeService = recipeService;
            _ingredientService = ingredientService;
        }

        public async void OnGet()
        {


            string? name = HttpContext.Session.GetString("Name");
            Console.WriteLine("dsad" + name);
            if (name.IsNullOrEmpty)
            {
                RedirectToPage("/Login");
                return;
            }

            Console.WriteLine("Hello");

            AvailableIngredients = await _ingredientService.GetAll();


            foreach (var ingredient in AvailableIngredients)
            {
                Console.WriteLine(ingredient.Name);
            }

            //Categories = await _recipeService.GetAll();
            //Difficulties = await _recipeService.GetAll();

        }

        public async Task<IActionResult> OnPostAsync()
        {

            int? creatorId = HttpContext.Session.GetInt32("UserId");
            if (creatorId == null)
                return RedirectToPage("/Login");

            Recipe recipe = new Recipe
            {
                CreatorId = creatorId.Value,
                Title = Title,
                Preparation = Preparation,
                CategoryId = CategoryId,
                DifficultId = DifficultId,
                Ingredients = ingredientesInput.Select(i => new RecipeIngredient
                {
                    IngredientId = i.IngredientId,
                    Unity = i.Unity,
                    Quantity = i.Quantity
                }).ToList()
            };

            await _recipeService.Add(recipe);
            return RedirectToPage("/Index");

        }
    }
}
