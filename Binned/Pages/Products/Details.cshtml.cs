using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Binned.Model;

namespace Binned.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly ProductService _productService;
        private IWebHostEnvironment _environment;

        public DetailsModel(ProductService productService, IWebHostEnvironment environment)
        {
            _productService = productService;
            _environment = environment;
        }

        [BindProperty]
        public Product OurProduct { get; set; } = new();

        [BindProperty]
        public IFormFile? Upload { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }

                    var uploadsFolder = "uploads";
                    if (OurProduct.ImageURL != null)
                    {
                        var oldImageFile = Path.GetFileName(OurProduct.ImageURL);
                        var oldImagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, oldImageFile);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    OurProduct.ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                }
                _productService.UpdateProduct(OurProduct);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Product {0} is updated", OurProduct.ProductName);
                return Redirect("/Products/Index");
            }
            return Page();
        }
    }
}
