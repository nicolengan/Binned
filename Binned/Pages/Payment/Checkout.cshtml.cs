using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using System.Threading;
using RestSharp.Authenticators;
using Azure.Core;
using static System.Net.WebRequestMethods;
using Stripe;
using Stripe.Checkout;
using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;

namespace Binned.Pages.Payment
{
    public class StripeOptions
    {
        public string option { get; set; }
    }
    public class CheckoutModel : PageModel
    {

        private readonly ILogger<CheckoutModel> _logger;

        public CheckoutModel(ILogger<CheckoutModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {

            var searchOptions = new ProductSearchOptions
            {
                Query = "active:'true' AND metadata['ProductId']:'1234'",
            };
            var searchService = new ProductService();
            var searched = searchService.Search(searchOptions);


            // need to check if null or else will crash
            _logger.LogInformation("search result {he}", searched.Data[0].DefaultPriceId);

            var domain = "http://localhost:7208";
            var sessionOptions = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    Price = $"{searched.Data[0].DefaultPriceId}",
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = domain + "/Checkout/Success",
                CancelUrl = domain + "/Checkout/Failure",
            };
            var sessionService = new SessionService();
            Session session = sessionService.Create(sessionOptions);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }


}
