using System.ComponentModel.DataAnnotations;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Admin
{
    public class CategoryModel : PageModel
    {

        private readonly CategoryService categoryService;

        [BindProperty]
        [Required]
        public string Name { get; set; }

        public bool Error { get; set; } = false;

        public CategoryModel(CategoryService categoryService)
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

        public   IActionResult OnPost ()
        {

            bool isCategoryRegistered =  categoryService.IsRegistered(Name);

            if (isCategoryRegistered)
            {
                Error = true;
                return Page();
            }

            Category category = new Category
            {
                Name = Name,
            };

             categoryService.CreateCategory(category);

            return Page();
        }
    }
}
