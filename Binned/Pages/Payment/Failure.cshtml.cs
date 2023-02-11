using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.Payment
{
    public class FailureModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly ILogger<FailureModel> _logger;
        public FailureModel(OrderService orderService, ILogger<FailureModel> logger)
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
            _orderService.UpdateStatusById(orderId, "To Pay");

            return Page();

        }
    }
}
