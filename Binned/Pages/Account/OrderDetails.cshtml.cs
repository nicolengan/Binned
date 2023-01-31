using Binned.Model;
using Binned.Pages.Payment;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.Account
{
    public class OrderDetailsModel : PageModel
    {
        [BindProperty]
        public Order? OneOrder { get; set; }
        //[BindProperty]
        //public string Button { get; set; }
        private readonly OrderService _orderService;
        private readonly ILogger<OrderDetailsModel> _logger;
        public OrderDetailsModel(OrderService orderService, ILogger<OrderDetailsModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        public IActionResult OnGet(string id)
        {
            if (ModelState.IsValid)
            {
                OneOrder = _orderService.GetOrderById(id);
                return Page();
            }
            return Redirect("/Account/Orders");

        }
        public void OnPost(string id)
        {
            OneOrder = _orderService.GetOrderById(id);
            _orderService.UpdateStatusById(id, "Received");
            TempData["FlashMessage.Type"] = "success";
            TempData["FlashMessage.Text"] = string.Format("Order status updated! Your order status has been changed to received.");
            _logger.LogInformation(id);
        }
    }
}
