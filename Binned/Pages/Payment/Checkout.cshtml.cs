using Binned.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.Payment
{
    public class CheckoutModel : PageModel
    {
        public string hello = "";

        public IActionResult OnPost()
        {
            return Redirect("/Payment/Payment");
        }
    }
}
