using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace Binned.Pages.Payment
{
    public class CartModel : PageModel
    {
        private readonly ILogger<CartModel> _logger;

        public CartModel(ILogger<CartModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            // ideally want to add this everytime a new product is created but we'll jst put it here x(
            var searchOptions = new ProductSearchOptions
            {
                Query = "active:'true' AND metadata['ProductId']:'1234'",
            };
            var searchService = new ProductService();
            var searched = searchService.Search(searchOptions);
            var ifNull = searched.FirstOrDefault();

            _logger.LogInformation("search null or not {s}", ifNull);

            if (ifNull == null)
            {
                var options = new ProductCreateOptions
                {
                    Name = "Product 1",
                    DefaultPriceData = new ProductDefaultPriceDataOptions
                    {
                        Currency = "SGD",
                        UnitAmount = 1200,
                        TaxBehavior = "inclusive"
                        // maybe add an url and image ?
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "ProductId", "1234" },
                    }
                };
                var service = new ProductService();
                var l = service.Create(options);
                _logger.LogInformation("hello prod created {l}", l);
            }
            else
            {
                _logger.LogInformation("hello prod alr created");
            }
        }
    }
}
