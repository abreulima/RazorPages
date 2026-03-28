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
        public string? Error { get; set; }


        private readonly UserRepository _userRepository;

        public LoginModel(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var user = _userRepository.GetUserLogin(Email, Password);

            if (user == null)
            {
                Error = "Email ou passe errada.";
                return Page();
            }

            if (!user.IsApproved)
            {
                Error = "Usuario pendente de ativacao.";
                return Page();
            }

            HttpContext.Session.SetString("Name", user.FirstName);
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

            return RedirectToPage("/Index");
        }
    }
}
