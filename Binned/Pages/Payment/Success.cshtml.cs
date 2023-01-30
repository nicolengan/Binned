using Binned.Model;
using Binned.Pages.Admin;
using Binned.Services;
using Microsoft.AspNetCore.Identity;
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
            var orderId = TempData["id"].ToString();

            _logger.LogInformation($"orderId: {orderId}");

            //var user = User.Identity.Name;

            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);


            //_orderService.AddOrder(newOrder);
            _orderService.UpdateStatusById(orderId, "Paid");

            return Page();
        }
    }
}
