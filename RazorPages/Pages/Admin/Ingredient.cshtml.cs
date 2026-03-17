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

        public bool Error { get; set; } = false;

        public IngredientModel(IngredientService _ingredientService)
        {
            ingredientService = _ingredientService;
        }

        public IActionResult OnGet()
        {
            string? isAdmin = HttpContext.Session.GetString("IsAdmin");

            if (isAdmin != "True")
                return RedirectToPage("/Login");

            return Page();
        }

        public   IActionResult OnPost ()
        {

            bool isIngredientRegistered =  ingredientService.IsRegistered(Name);

            if (isIngredientRegistered)
            {
                Error = true;
                return Page();
            }

            Ingredient ingredient = new Ingredient
            {
                Name = Name,
            };

             ingredientService.CreateIngredient(ingredient);

            return Page();
        }
    }
}
