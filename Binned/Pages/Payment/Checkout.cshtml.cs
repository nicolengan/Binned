using Binned.Model;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;
using Stripe;
using Microsoft.AspNetCore.Http.Features;
using Stripe.Issuing;

namespace Binned.Pages.Payment
{
    public class StripeOptions
    {
        public string option { get; set; }
    }

    public class CheckoutModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly ILogger<CheckoutModel> _logger;
        private readonly OrderService _orderService;
        [BindProperty]
        public List<Model.Product> ProductList { get; set; } = new List<Model.Product>();
        public Cart OneCart { get; set; }
        public Order NewOrder { get; set; }
        public CheckoutModel(CartService cartService, OrderService orderService, ILogger<CheckoutModel> logger)
        {
            _cartService = cartService;
            _orderService = orderService;
            _logger = logger;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            //var user = User.Identity.Name;
            OneCart = await _cartService.GetCartByUserName("test");
            _logger.LogInformation($"cart {OneCart.Items}");
            foreach (var i in OneCart.Items)
            {
                ProductList.Add(i.Product);
            }
            NewOrder = new Order
            {
                Products = ProductList,
                UserId = "test",
                Status = "Processing",
                PaymentStatus = false

            };
            _orderService.AddOrder(NewOrder);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation($"cart {NewOrder.OrderId}");
            // to be added in cart code, from cart should be getting an array or whatever of product id then store that into the code, check if each item exist in stripe first
            var searchOptions = new ProductSearchOptions
            {
                Query = "active:'true' AND metadata['ProductId']:'1234'",
            };
            var searchService = new Stripe.ProductService();
            var searched = searchService.Search(searchOptions);

            // need to check if null or else will crash
            //_logger.LogInformation("search result {he}", searched.Data[0].DefaultPriceId);
            var port = HttpContext.Features.Get<IHttpConnectionFeature>()?.LocalPort;
            var domain = $"https://localhost:{port}";

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
            //_logger.LogInformation($"json checkout: {session}, {session.PaymentIntentId}");

            return new StatusCodeResult(303);
        }
    }
}
