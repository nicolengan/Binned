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
        public int id { get; set; }

        public CheckoutModel(CartService cartService, OrderService orderService, ILogger<CheckoutModel> logger)
        {
            _cartService = cartService;
            _orderService = orderService;
            _logger = logger;
        }
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task<IActionResult> OnGetAsync()
        {
            //var user = User.Identity.Name;
            OneCart = await _cartService.GetCartByUserName("test");
            _logger.LogInformation($"cart {OneCart.Items}");
            id = OneCart.Id;
            foreach (var i in OneCart.Items)
            {
                ProductList.Add(i.Product);
            }
            //await _cartService.RemoveItem(id);
            NewOrder = new Order
            {
                OrderId = RandomString(10),
                Products = ProductList,
                UserId = "test",
                Status = "Processing",
                PaymentStatus = false,
            };
            TempData["orderId"] = NewOrder.OrderId;
            _logger.LogInformation($"cart {ProductList.Count}");

            _orderService.AddOrder(NewOrder);
            _orderService.CalculateTotal(NewOrder.OrderId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var orderId = TempData["orderId"].ToString();
            _logger.LogInformation($"hello2 {orderId}");
            var totalAmt = _orderService.GetOrderById(orderId).Amount;
            _logger.LogInformation($"hello2 {totalAmt}");
            // to be added in cart code, from cart should be getting an array or whatever of product id then store that into the code, check if each item exist in stripe first
            //var searchOptions = new ProductSearchOptions
            //{
            //    Query = $"active:'true' AND metadata['OrderId']:'{orderId}'",
            //};
            //var searchService = new Stripe.ProductService();
            //var searched = searchService.Search(searchOptions);

            // need to check if null or else will crash
            //_logger.LogInformation("search result {he}", searched.Data[0].DefaultPriceId);
            var port = HttpContext.Features.Get<IHttpConnectionFeature>()?.LocalPort;
            var domain = $"https://localhost:{port}";

            var sessionOptions = new SessionCreateOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    { "OrderId", $"{orderId}" }
                },
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long?)(totalAmt * 100),
                        Currency = "SGD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name= "name",
                        },
                    },
                    Quantity = 1,
                    // maybe add customer email so they dont have to retype
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
                TempData["FlashMessage.Text"] = "Order placed";
                //_logger.LogInformation($"{session.Metadata["OrderId"]}");
                TempData["id"] = session.Metadata["OrderId"];
            }

            Response.Headers.Add("Location", $"{session.Url}");
            //_logger.LogInformation($"json checkout: {session}, {session.PaymentIntentId}");

            return new StatusCodeResult(303);
        }
    }
}
