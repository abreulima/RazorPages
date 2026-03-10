using System.ComponentModel.DataAnnotations;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Admin
{
    public class ActiveUserModel : PageModel
    {

        private readonly CategoryService categoryService;

        [BindProperty]
        [Required]
        public string Name { get; set; }

        public ActiveUserModel(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult OnGet()
        {
            string? isAdmin = HttpContext.Session.GetString("IsAdmin");

            if (isAdmin != "True")
                return RedirectToPage("/Login");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            Console.WriteLine(Name);

            Category category = new Category
            {
                Name = Name,
            };

            await categoryService.CreateCategory(category);

            return Page();
        }
    }
}
