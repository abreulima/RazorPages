using System.ComponentModel.DataAnnotations;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Admin
{
    public class ActiveUserModel : PageModel
    {

        private readonly UserService _userService;

        [BindProperty]
        public int UserId { get; set; }


        public List<User> Users { get; set; } = new List<User>();

        public ActiveUserModel(UserService userService)
        {
            this._userService = userService;
        }

        public IActionResult OnGet()
        {

            string? isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "True")
                return RedirectToPage("/Login");

            Users = _userService.GetAll();

            return Page();
        }

        public IActionResult OnPost ()
        {

            var user = _userService.GetUserById(UserId);

            if (user != null)
            {
                user.IsApproved = !user.IsApproved;
                _userService.UpdateUser(user);
            }


            return RedirectToPage();
        }
    }
}
