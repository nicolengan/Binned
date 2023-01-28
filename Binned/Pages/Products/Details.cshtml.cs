using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Binned.Model;

namespace Binned.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly ProductService _productService;
        public DetailsModel(ProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Product OurProduct { get; set; } = new();

        public IActionResult OnGet(int id)
        {

            Product? product = _productService.GetProductById(id);
            if (product != null)
            {
                OurProduct = product;
                return Page();
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Product ID {0} not found", id);
                return Redirect("/Products");
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(OurProduct);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Product {0} is updated", OurProduct.ProductName);
                return Redirect("/Products/Index");
            }
            return Page();
        }
    }
}
