using Binned.Model;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;
using Stripe;
using Microsoft.AspNetCore.Http.Features;
using Stripe.Issuing;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Binned.Areas.Identity.Data;

namespace Binned.Pages.Payment
{
    public class StripeOptions
    {
        public string option { get; set; }
    }

    public class ShippingInfo
    {
        public string Address { get; set; }
        public string? Address2 { get; set; }
        [RegularExpression(@"^\d{6}$",
        ErrorMessage = "Postal Code should be 6 numbers long")]
        public string PostalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class CheckoutModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly ILogger<CheckoutModel> _logger;
        private readonly OrderService _orderService;
        private readonly UserManager<BinnedUser> userManager;
        public List<Model.Product>? ProductList { get; set; } = new List<Model.Product>();
        public Cart OneCart { get; set; }
        public Order NewOrder { get; set; }
        public int id { get; set; }
        [BindProperty]
        public ShippingInfo ShippingInfo { get; set; }

        public CheckoutModel(UserManager<BinnedUser> userManager, CartService cartService, OrderService orderService, ILogger<CheckoutModel> logger)
        {
            this.userManager = userManager;
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
        public async Task OnGetAsync()
        {
            //var user = User.Identity.Name;
            var user = await userManager.GetUserAsync(User);
            var username = user.UserName;
            OneCart = await _cartService.GetCartByUserName(username);
            _logger.LogInformation($"cart {OneCart.Items}");
            id = OneCart.Id;
            foreach (var i in OneCart.Items)
            {
                ProductList.Add(i.Product);
            }
            _logger.LogInformation($"cart {ProductList.Count}");
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var username = user.UserName;
            _logger.LogInformation($"user {username}");
            OneCart = await _cartService.GetCartByUserName(username);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            foreach (var i in OneCart.Items)
            {
                ProductList.Add(i.Product);
            }
            _logger.LogInformation($"cart {ProductList.Count}");

            NewOrder = new Order
            {
                OrderId = RandomString(10),
                Products = ProductList,
                Status = "Processing",
                UserId = username,
                Address = ShippingInfo.Address,
                Address2 = ShippingInfo.Address2,
                PostalCode = Int32.Parse(ShippingInfo.PostalCode),
                FirstName = ShippingInfo.FirstName,
                LastName = ShippingInfo.LastName,
            };
            _orderService.AddOrder(NewOrder);
            var totalAmt = await _orderService.CalculateTotal(NewOrder.OrderId);

            _logger.LogInformation($"hello2 {NewOrder.OrderId}");

            var port = HttpContext.Features.Get<IHttpConnectionFeature>()?.LocalPort;
            var domain = $"https://localhost:{port}";

            var sessionOptions = new SessionCreateOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    { "OrderId", $"{NewOrder.OrderId}" }
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
                CancelUrl = $"{domain}/Payment/Failure",
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
