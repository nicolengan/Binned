using Binned.Areas.Identity.Data;
using Binned.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages
{
    [Authorize]
    public class CartModel : PageModel
    {

        private readonly CartService _cartService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly UserManager<BinnedUser> _userManager;
        public CartModel(UserManager<BinnedUser> userManager, ProductService productService, CartService cartService, OrderService orderService)
        {
            _cartService = cartService;
            _userManager = userManager;
            _productService = productService;
            _orderService = orderService;
        }

        public Model.Cart Cart { get; set; } = new Model.Cart();
        public bool valid { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var username = user.UserName;
            Cart = await _cartService.GetCartByUserName(username);
            Console.WriteLine(username);

            foreach (var i in Cart.Items)
            {
                if (i.Product.Availability == "N")
                {
                    Console.WriteLine(i.Product.Availability);
                    valid = false;
                    Console.WriteLine(valid);
                }
                else
                {
                    valid = true;
                }
            }

            return Page();
        }
        public async Task<IActionResult> OnPostRemoveFromCartAsync(int CartItemId)
        {
            await _cartService.RemoveItem(CartItemId);

            return RedirectToPage("/User/Cart");
        }

    }
}