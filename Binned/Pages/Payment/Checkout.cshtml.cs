using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Binned.Services;

namespace Binned.Pages.Payment
{
    public class CheckoutModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly OrderService _orderService;
        public CheckoutModel(OrderService orderService, CartService cartService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        [BindProperty]

        public Models.Order Order { get; set; } = new Models.Order();
        public Models.Cart Cart { get; set; } = new Models.Cart();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _cartService.GetCartByUserName("test");
            return Page();
        }
    }
}