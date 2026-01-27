using System.ComponentModel.DataAnnotations;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages
{
    public class RegisterModel : PageModel
    {

        private readonly UserService _userService;


        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        [BindProperty]
        [Required]
        public string Email { get; set; }

        public RegisterModel(UserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            Console.WriteLine(Username);
            Console.WriteLine(Password);

            User user = new User
            {
                Name = Username,
                Email = Email,
                Password = Password,
            };

            await _userService.CreateUserAysnc(user);

            return Page();
        }
    }
}
