using System.ComponentModel.DataAnnotations;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages
{
    public class RegisterModel : PageModel
    {

        private readonly UserService userService;


        [BindProperty]
        [Required]
        public string firstName { get; set; }

        [BindProperty]
        [Required]
        public string lastName { get; set; }

        [BindProperty]
        [Required]
        public string password { get; set; }

        [BindProperty]
        [Required]
        public string email { get; set; }

        public bool Error { get; set; } = false;

        public RegisterModel(UserService _userService)
        {
            userService = _userService;
        }

        public void OnGet()
        {
        }

        public   IActionResult OnPost ()
        {

            bool isEmailRegistered =  userService.IsRegistered(email);

            if (isEmailRegistered)
            {
                Error = true;
                return Page();
            }

            User user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Password = password,
                Email = email,
            };

             userService.CreateUser(user);

            return RedirectToPage("/Login");
        }
    }
}
