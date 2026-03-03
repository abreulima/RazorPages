using System.ComponentModel.DataAnnotations;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Admin
{
    public class IngredientModel : PageModel
    {

        private readonly IngredientService ingredientService;

        [BindProperty]
        [Required]
        public string Name { get; set; }

        public IngredientModel(IngredientService _ingredientService)
        {
            ingredientService = _ingredientService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            Console.WriteLine(Name);

            Ingredient ingredient = new Ingredient
            {
                Name = Name,
            };

            await ingredientService.CreateIngredientAysnc(ingredient);

            return Page();
        }
    }
}
