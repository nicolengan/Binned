using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages
{
    public class CartModel : PageModel
    {

        private readonly CartService _cartService;
        private readonly ProductService _productService;
        public CartModel(ProductService productService, CartService cartService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        public Model.Cart Cart { get; set; } = new Model.Cart();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _cartService.GetCartByUserName("test");

            return Page();
        }
        public async Task<IActionResult> OnPostRemoveFromCartAsync(int CartItemId)
        {
            await _cartService.RemoveItem(CartItemId);

            return RedirectToPage("/Cart");
        }
    }
}