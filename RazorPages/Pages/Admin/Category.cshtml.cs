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

        public CategoryModel(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            Console.WriteLine(Name);

            Category category = new Category
            {
                Name = Name,
            };

            await categoryService.CreateCategoryAysnc(category);

            return Page();
        }
    }
}
