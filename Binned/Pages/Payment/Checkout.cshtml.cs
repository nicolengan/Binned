using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using System.Threading;
using RestSharp.Authenticators;
using Azure.Core;
using static System.Net.WebRequestMethods;
using Stripe;
using Stripe.Checkout;

namespace Binned.Pages.Payment
{
    public class StripeOptions
    {
        public string option { get; set; }
    }
    public class CheckoutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {

            var searchOptions = new ProductSearchOptions
            {
                Query = "active:'true' AND metadata['ProductId']:'1234'",
            };
            var searchService = new ProductService();
            searchService.Search(searchOptions);

            Console.WriteLine(searchService.Search(searchOptions));

            var domain = "http://localhost:7208";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    Price = "price_1MLiHmDRJAN9sJBkj9XQxSq9",
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = domain + "/Checkout/Success",
                CancelUrl = domain + "/Checkout/Failure",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }


}
