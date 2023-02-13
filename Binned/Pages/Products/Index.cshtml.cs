using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Binned.Model;

namespace Binned.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly WishlistService _wishlistService;
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        public IndexModel(ProductService productService, CartService cartService, WishlistService wishlistService)
        {
            _wishlistService = wishlistService;
            _cartService = cartService;
            _productService = productService;
        }
        public List<Product> ProductList { get; set; } = new();

        public Model.Product Product { get; set; }

        public void OnGet()
        {
            ProductList = _productService.GetAll();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            await _cartService.AddItem("test", productId);
            return RedirectToPage("/User/Cart");
        }

        public async Task<IActionResult> OnPostAddToWishlistAsync(int productId)
        {
            await _wishlistService.AddItem("test", productId);
            return RedirectToPage("/User/Wishlist");
        }
    }
    
}
