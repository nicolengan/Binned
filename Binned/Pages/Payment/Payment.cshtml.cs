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
using Binned.Services;

namespace Binned.Pages.Payment
{
    public class StripeOptions
    {
        public string option { get; set; }
    }
    public class PaymentModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly ILogger<PaymentModel> _logger;

        public PaymentModel(OrderService orderService, ILogger<PaymentModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            // to be added in cart code, from cart should be getting an array or whatever of product id then store that into the code, check if each item exist in stripe first
            var searchOptions = new ProductSearchOptions
            {
                Query = "active:'true' AND metadata['ProductId']:'1234'",
            };
            var searchService = new ProductService();
            var searched = searchService.Search(searchOptions);

            // need to check if null or else will crash
            _logger.LogInformation("search result {he}", searched.Data[0].DefaultPriceId);

            var domain = "https://localhost:7208";
            var sessionOptions = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    Price = $"{searched.Data[0].DefaultPriceId}",
                    Quantity = 1, // maybe add customer email so they dont have to retype
                  },
                },
                Mode = "payment",
                SuccessUrl = $"{domain}/Payment/Success",
                CancelUrl = domain + "/Payment/Failure",
            };
            var sessionService = new SessionService();
            Session session = sessionService.Create(sessionOptions);

            if (ModelState.IsValid)
            { // check if in database first
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = "order placed";
                TempData["sessionId"] = session.Id;
            }

            Response.Headers.Add("Location", $"{session.Url}");
            _logger.LogInformation($"json checkout: {session}, {session.PaymentIntentId}");

            return new StatusCodeResult(303);
        }
    }


}
