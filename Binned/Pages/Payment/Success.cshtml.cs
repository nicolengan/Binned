using Binned.Model;
using Binned.Pages.Admin;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using Stripe.Checkout;

namespace Binned.Pages.Payment
{
    public class SuccessModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly ILogger<SuccessModel> _logger;

        public SuccessModel(OrderService orderService, ILogger<SuccessModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        public async Task<IActionResult> OnGet()
        {
            if (TempData.ContainsKey("sessionId"))
            {
                var id = TempData["sessionId"].ToString();

                _logger.LogInformation($"session: {id}");

                var newOrder = new Order
                {
                    //ProductId = "1234",
                    //UserId = "user1",
                    //Status = "shipped"
                    ProductId = "1111",
                    UserId = "user1",
                    Status = "shipped"
                };

                _orderService.AddOrder(newOrder);


            }

            return Page();
        }
    }
}
