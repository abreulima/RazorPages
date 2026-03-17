using DAL.Models;
using DAL.Repositories;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;


namespace RazorPages.Pages
{
    public class AdminModel : PageModel
    {

        public AdminModel()
        {

        }

        public IActionResult OnGet()
        {
            string? isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "True")
                return RedirectToPage("/Login");

            return Page();
        }

    }
}
