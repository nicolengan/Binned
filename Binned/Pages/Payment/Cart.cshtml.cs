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
            // ideally want to add this everytime a new product is created but we'll jst put it here for now x(
            // creates product in stripe, finds if it exists or not by product id if not will create product
            // product id is saved into database (column id) and that is used to search
            // theres like a few sec delay but shouldnt be a prob when this is added to the add product admin side
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
                    DefaultPriceData = new ProductDefaultPriceDataOptions // adds price item
                    {
                        Currency = "SGD",
                        UnitAmount = 1200, // in cents so hv to add 2 more zeros after
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