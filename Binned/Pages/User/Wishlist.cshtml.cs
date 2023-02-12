using Binned.Areas.Identity.Data;
using Binned.Model;
using Binned.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.User
{
    [Authorize]
    public class WishlistModel : PageModel
    {
        private readonly WishlistService _wishlistService;
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        private readonly UserManager<BinnedUser> _userManager;
        public WishlistModel(UserManager<BinnedUser> userManager, ProductService productService, CartService cartService, WishlistService wishlistService)
        {
            _wishlistService = wishlistService;
            _cartService = cartService;
            _productService = productService;
            _userManager = userManager;
        }

        public Model.Wishlist Wishlist { get; set; } = new Model.Wishlist();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var username = user.UserName;
            Wishlist = await _wishlistService.GetWishlistByUserName(username);

            return Page();
        }
        public async Task<IActionResult> OnPostRemoveFromWishlistAsync(int WishlistItemId)
        {
            await _wishlistService.RemoveItem(WishlistItemId);

            return RedirectToPage("/User/Wishlist");
        }
        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var username = user.UserName;
            await _cartService.AddItem(username, productId);
            //await _cartService.AddItem("test", productId);
            //if (ModelState.IsValid)
            //{
            //    await _cartService.AddItem("test", productId);
            //    if (productId != null)
            //    {
            //        TempData["FlashMessage.Type"] = "danger";
            //        TempData["FlashMessage.Text"] = string.Format("Product is in cart.");
            //    }
            //}                
            return RedirectToPage("/User/Cart");
        }
    }
}