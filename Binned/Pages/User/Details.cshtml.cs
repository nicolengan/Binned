using Binned.Model;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.User
{
    public class DetailsModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(OrderService orderService, ILogger<DetailsModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        public Order MyOrder { get; set; } = new();
        public IActionResult OnGet(int id)
        {
            Order? order = _orderService.GetOrderById(id);
            if (order != null)
            {
                MyOrder = order;
                _logger.LogInformation($"order id{MyOrder.OrderId}");
                ViewData["id"] = "";
                return Page();
            }
            else
            {

                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Order ID {0} not found", id);
                return Redirect("/Admin/Orders");
            }
        }
    }
}
