using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages
{

    public class IndexModel : PageModel
    {
    public string Teste { get; set; } = "Ivan";
        public void OnGet()
        {

        }
    }
}
