using Binned.Services;
using Binned.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.Products
{
    public class AddModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        public AddModel(ProductService productService, CartService cartService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        [BindProperty]
        public Product MyProduct { get; set; } = new();
        public static List<Cart> CartList { get; set; } = new();
        public void OnGet()
        {
            CartList = _cartService.GetAll();
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Product? product = _productService.GetProductById(
                MyProduct.ProductId);
                if (product != null)
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format("Product ID {0} already exists", MyProduct.ProductId);
                    return Page();
                }
                _productService.AddProduct(MyProduct);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Product {0} is added", MyProduct.ProductName);
                return Redirect("/Products");
            }
            return Page();
        }
    }
}

