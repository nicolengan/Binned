using Binned.Model;
using Binned.Pages.Payment;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.Admin
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

        [BindProperty]
        public Order MyOrder { get; set; } = new();
        public IActionResult OnGet(int id)
        {
            Order? order = _orderService.GetOrderById(id);
            if (order != null)
            {
                MyOrder = order;
                return Page();
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Order ID {0} not found", id);
                return Redirect("/Admin/Orders");
            }
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _orderService.UpdateOrder(MyOrder);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Order {0} is updated", MyOrder.OrderId);
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Order {0} is updated", MyOrder.OrderId);
            }
            return Page();
        }
    }
}
