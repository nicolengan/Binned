using Binned.Pages.Payment;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.Account
{
    public class OrderDetailsModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly ILogger<OrderDetailsModel> _logger;
        public OrderDetailsModel(OrderService orderService, ILogger<OrderDetailsModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        public void OnGet(string id)
        {
            _logger.LogInformation(id);
        }
    }
}
