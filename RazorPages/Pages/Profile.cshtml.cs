using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace RazorPages.Pages
{
    public class ProfileModel : PageModel
    {

        private readonly UserService userService;

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }


        [BindProperty]
        public string Email { get; set; }

        public bool Error { get; set; } = false;

        public ProfileModel(UserService _userService)
        {
            userService = _userService;
        }

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            var user = userService.GetUserById(userId.Value);

            if (user == null)
                return RedirectToPage("/Login");

            // Preencha os campos
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;

            return Page();
        }

        public IActionResult OnPost()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) 
                return RedirectToPage("/Login");

            var user = userService.GetUserById(userId.Value);
            if (user == null) 
                return RedirectToPage("/Login");

            // Emails nao podem ser repetido, ao menos que seja o email do proprio usuario
            if (Email != user.Email && userService.IsRegistered(Email))
            {
                Error = true;
                return Page();
            }

            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Email = Email;

            userService.UpdateUser(user);

            return RedirectToPage("/Index");
        }



    }
}
