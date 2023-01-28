using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Binned.Model;

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

        public Model.Product Product { get; set; }

        public void OnGet()
        {
            ProductList = _productService.GetAll();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });
            //productId = 1;
            await _cartService.AddItem("test", productId);
            return RedirectToPage("/Cart");
        }
    }
    
}
