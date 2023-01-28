using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Binned.Model;

namespace Binned.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly ProductService _productService;

        public IndexModel(ProductService productService)
        {
            _productService = productService;
        }

        public List<Product> ProductList { get; set; } = new();

        public void OnGet()
        {
            ProductList = _productService.GetAll();
        }
    }
}
