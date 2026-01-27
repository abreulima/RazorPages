using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace RazorPages.Pages
{
    public class LoginModel : PageModel
    {

        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }


        public void OnGet()
        {
        }

        public void OnPost()
        {

            Console.WriteLine(Username);
            Console.WriteLine(Password);
        }
    }
}
