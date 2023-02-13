using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Binned.Model;
using Binned.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Binned.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly WishlistService _wishlistService;
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        private readonly UserManager<BinnedUser> _userManager;
        public IndexModel(UserManager<BinnedUser> userManager, ProductService productService, CartService cartService, WishlistService wishlistService)
        {
            _wishlistService = wishlistService;
            _cartService = cartService;
            _productService = productService;
            _userManager = userManager;
        }
        public List<Product> ProductList { get; set; } = new();

        public Model.Product Product { get; set; }

        public void OnGet()
        {
            ProductList = _productService.GetAll();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var username = user.UserName;
            await _cartService.AddItem(username, productId);
            return RedirectToPage("/User/Cart");
        }

        public async Task<IActionResult> OnPostAddToWishlistAsync(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var username = user.UserName;
            await _wishlistService.AddItem(username, productId);
            return RedirectToPage("/User/Wishlist");
        }
        public async Task<IActionResult> OnPostDeleteProductAsync(Product product)
        {
            await _productService.RemoveItem(product);

            return RedirectToPage("/Products/Index");
        }
    }

}