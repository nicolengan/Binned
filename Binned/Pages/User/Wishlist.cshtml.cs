using Binned.Model;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.User
{
    public class WishlistModel : PageModel
    {
        private readonly WishlistService _wishlistService;
        private readonly ProductService _productService;
        public WishlistModel(ProductService productService, WishlistService wishlistService)
        {
            _wishlistService = wishlistService;
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
    }
}
