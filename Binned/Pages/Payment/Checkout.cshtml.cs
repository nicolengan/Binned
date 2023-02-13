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
using Microsoft.AspNetCore.Authorization;

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
        [EmailAddress]
        public string Email { get; set; }
    }
    [Authorize]
    public class CheckoutModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly CodeService _codeService;
        private readonly OrderService _orderService;
        private readonly UserManager<BinnedUser> userManager;
        private readonly ILogger<CheckoutModel> _logger;
        public List<Model.Product>? ProductList { get; set; } = new List<Model.Product>();
        public Cart OneCart { get; set; }
        public Order NewOrder { get; set; }
        [BindProperty]
        public double totalAmt { get; set; }
        [BindProperty]
        public ShippingInfo ShippingInfo { get; set; }
        [BindProperty]
        public string? promoCode { get; set; }
        public CheckoutModel(UserManager<BinnedUser> userManager, CartService cartService, OrderService orderService, ILogger<CheckoutModel> logger, CodeService codeService)
        {
            this.userManager = userManager;
            _cartService = cartService;
            _orderService = orderService;
            _codeService = codeService;
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
            var user = await userManager.GetUserAsync(User);
            var username = user.UserName;
            OneCart = await _cartService.GetCartByUserName(username);
            if (OneCart.Items.Count <= 0)
            {
                return Redirect("/User/Cart");
            }
            return Page();
        }

        public async Task<IActionResult> OnGetCode(string name)
        {
            OneCart = await _cartService.GetCartByUserName("test");
            _logger.LogInformation($"cart {OneCart.Items}");

            var code = _codeService.GetCodeByName(name);
            TempData["code"] = name;
            if (code != null)
            {
                return new JsonResult(new { code = code.Discount });
            }
            return new JsonResult(new { code = 0 });
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            //_logger.LogInformation(TempData["code"].ToString());
            if (totalAmt <= 0)
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = "Cannot place an order with a negative total.";
                return Redirect("/Cart");
            }
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

            NewOrder = new Order
            {
                OrderId = RandomString(10),
                Products = ProductList,
                Status = "To Pay",
                UserId = username,
                Address = ShippingInfo.Address,
                Address2 = ShippingInfo.Address2,
                PostalCode = Int32.Parse(ShippingInfo.PostalCode),
                FirstName = ShippingInfo.FirstName,
                LastName = ShippingInfo.LastName,
            };

            _orderService.AddOrder(NewOrder);

            var port = HttpContext.Features.Get<IHttpConnectionFeature>()?.LocalPort;
            var domain = $"https://localhost:{port}";

            var
                sessionOptions = new SessionCreateOptions
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
