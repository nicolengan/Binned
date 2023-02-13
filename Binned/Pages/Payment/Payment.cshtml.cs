using Binned.Areas.Identity.Data;
using Binned.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;

namespace Binned.Pages.Payment
{
    [Authorize]
    public class PaymentModel : PageModel
    {
        private readonly ILogger<PaymentModel> _logger;
        private readonly OrderService _orderService;
        private readonly UserManager<BinnedUser> userManager;
        public PaymentModel(OrderService orderService, ILogger<PaymentModel> logger, UserManager<BinnedUser> userManager)
        {
            this.userManager = userManager;
            _logger = logger;
            _orderService = orderService;
        }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await userManager.GetUserAsync(User);
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = "Order not found";
                return Redirect("/User/Orders");
            }
            if (user.Email != order.UserId)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            var port = HttpContext.Features.Get<IHttpConnectionFeature>()?.LocalPort;
            var domain = $"https://localhost:{port}";

            var sessionOptions = new SessionCreateOptions
            {
                Metadata = new Dictionary<string, string>
                    {
                        { "OrderId", $"{id}" }
                    },
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long?)(order.Amount * 100),
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
                //_logger.LogInformation($"{session.Metadata["OrderId"]}");
                TempData["id"] = session.Metadata["OrderId"];
            }

            Response.Headers.Add("Location", $"{session.Url}");
            //_logger.LogInformation($"json checkout: {session}, {session.PaymentIntentId}");
            return new StatusCodeResult(303);
        }
    }
}
