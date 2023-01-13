using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Binned.Services;
using Binned.Models;
using System.Drawing;

namespace Binned.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        public IndexModel(ProductService productService, CartService cartService)
        {
            _cartService = cartService;
            _productService = productService;
        }
        public List<Product> ProductList { get; set; } = new();

        public Models.Product Product { get; set; }

        public void OnGet()
        {
            ProductList = _productService.GetAll();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            await _cartService.AddItem("test", productId);
            return RedirectToPage("/Cart");
        }
    }
}
