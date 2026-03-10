using DAL.Models;
using DAL.Repositories;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;


namespace RazorPages.Pages
{
    public class LoginModel : PageModel
    {

        [BindProperty]
        [Required]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        public User? CurrentUser { get; set; }
        public bool Error { get; set; } = false;


        private readonly UserRepository _userRepository;

        public LoginModel(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {

            CurrentUser = await _userRepository.GetUserLogin(Email, Password);

            if (CurrentUser == null)
            {
                Error = true;
                return Page();
            }

            HttpContext.Session.SetString("Name", CurrentUser.FirstName);
            HttpContext.Session.SetString("UserId", CurrentUser.Id.ToString());
            HttpContext.Session.SetString("Email", CurrentUser.Email);
            HttpContext.Session.SetString("IsAdmin", CurrentUser.IsAdmin.ToString());

            return RedirectToPage("/Index");
        }
    }
}
