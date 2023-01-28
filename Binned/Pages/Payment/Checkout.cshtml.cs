using Binned.Model;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.Payment
{
    public class CheckoutModel : PageModel
    {
        public string hello = "";

        private readonly CartService _cartService;
        private readonly OrderService _orderService;
        public CheckoutModel(OrderService orderService, CartService cartService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        [BindProperty]

        public Model.Order Order { get; set; } = new Model.Order();
        public Model.Cart Cart { get; set; } = new Model.Cart();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _cartService.GetCartByUserName("test");
            return Page();
        }

        public IActionResult OnPost()
        {
            return Redirect("/Payment/Payment");
        }
    }
}
