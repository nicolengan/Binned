using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace Binned.Pages.Payment
{
    public class CartModel : PageModel
    {
        public void OnGet()
        {
            var options = new ProductCreateOptions
            {
                Name = "Product 1",
                DefaultPriceData =
                {
                    Currency = "SGD",
                    UnitAmount = 12,
                    TaxBehavior = "inclusive"
                    // maybe add an url and image?
                },
                Metadata = new Dictionary<string, string>
                {
                    { "ProductId", "1234" },
                }
            };
            var service = new ProductService();
            service.Create(options);
        }
    }
}
