using Binned.Model;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.User
{
    public class WishlistModel : PageModel
    {
        private readonly WishlistService _wishlistService;
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        public WishlistModel(ProductService productService, CartService cartService, WishlistService wishlistService)
        {
            _wishlistService = wishlistService;
            _cartService = cartService;
            _productService = productService;
        }

        public Model.Wishlist Wishlist { get; set; } = new Model.Wishlist();

        public async Task<IActionResult> OnGetAsync()
        {
            Wishlist = await _wishlistService.GetWishlistByUserName("test");

            return Page();
        }
        public async Task<IActionResult> OnPostRemoveFromWishlistAsync(int WishlistItemId)
        {
            await _wishlistService.RemoveItem(WishlistItemId);

            return RedirectToPage("/User/Wishlist");
        }
        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            await _cartService.AddItem("test", productId);
            //if (ModelState.IsValid)
            //{
            //    await _cartService.AddItem("test", productId);
            //    if (productId != null)
            //    {
            //        TempData["FlashMessage.Type"] = "danger";
            //        TempData["FlashMessage.Text"] = string.Format("Product is in cart.");
            //    }
            //}                
            return RedirectToPage("/Cart");
        }
    }
}
