using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Binned.Model;
using Binned.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Binned.Pages.User
{
    public class productDisplayModel : PageModel
    {
        private readonly WishlistService _wishlistService;
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        private readonly UserManager<BinnedUser> userManager;
        public productDisplayModel(UserManager<BinnedUser> userManager, ProductService productService, CartService cartService, WishlistService wishlistService)
        {
            this.userManager = userManager;
            _wishlistService = wishlistService;
            _cartService = cartService;
            _productService = productService;
        }
        public List<Product> ProductList { get; set; } = new();

        public Model.Product Product { get; set; }

        public void OnGet()
        {
            ProductList = _productService.GetAvailProducts();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            var user = await userManager.GetUserAsync(User);
            var username = user.UserName;
            await _cartService.AddItem(username, productId);
            //await _cartService.AddItem("test", productId);
            return RedirectToPage("/User/Cart");
        }

        public async Task<IActionResult> OnPostAddToWishlistAsync(int productId)
        {
            var user = await userManager.GetUserAsync(User);
            var username = user.UserName;
            await _wishlistService.AddItem(username, productId);
            return RedirectToPage("/User/Wishlist");
        }
    }

}
