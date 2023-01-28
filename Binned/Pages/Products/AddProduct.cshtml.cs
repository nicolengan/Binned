using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Binned.Model;

namespace Binned.Pages.Products
{
    public class AddProductModel : PageModel
    {
        private readonly ProductService _productService;
        //private IWebHostEnvironment _environment;

        public AddProductModel(ProductService productService)
        {
            _productService = productService;
            //_environment = environment;
        }

        [BindProperty]
        public Product OurProduct { get; set; } = new();
        //[BindProperty]
        //public IFormFile? Upload { get; set; }

        //public void OnGet()
        //{update
        //    DepartmentList = _departmentService.GetAll();
        //}

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Product? product = _productService.GetProductById(OurProduct.ProductId);
                if (product != null)
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format("Product ID {0} alreay exists", OurProduct.ProductId);
                    return Page();
                }

                //if (Upload != null)
                //{
                //    if (Upload.Length > 2 * 1024 * 1024)
                //    {
                //        ModelState.AddModelError("Upload",
                //        "File size cannot exceed 2MB.");
                //        return Page();
                //    }
                //    var uploadsFolder = "uploads";
                //    var imageFile = Guid.NewGuid() + Path.GetExtension(
                //    Upload.FileName);
                //    var imagePath = Path.Combine(_environment.ContentRootPath,
                //    "wwwroot", uploadsFolder, imageFile);
                //    using var fileStream = new FileStream(imagePath,
                //    FileMode.Create);
                //    await Upload.CopyToAsync(fileStream);
                //    OurProduct.ImageURL = string.Format("/{0}/{1}", uploadsFolder,
                //    imageFile);
                //}

                _productService.AddProduct(OurProduct);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Product {0} is added", OurProduct.ProductName);
                return Redirect("/Products/Index");
            }
            return Page();
        }


    }
}