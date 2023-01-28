using Binned.Model;
using Binned.Pages.Admin;
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
        [BindProperty]
        public Order MyOrder { get; set; } = new();
        public IActionResult OnGet(int id)
        {
            Order? order = _orderService.GetOrderById(id);
            if (order != null)
            {
                MyOrder = order;
                _logger.LogInformation($"order id{MyOrder.OrderId}");
                ViewData["id"] = id;
                return Page();
            }
            else
            {

                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Order ID {0} not found", id);
                return Redirect("/User/Orders");
            }
        }
        public IActionResult OnPost()
        {

            // theres an error here x(
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            _logger.LogInformation($"{errors}");

            var id = Convert.ToInt32(TempData["id"]);
            _logger.LogInformation($"{id}");
            Order? order = _orderService.GetOrderById(id);

            if (order != null)
            {

                order.Status = "Delivered";
                _logger.LogInformation($"{order.ProductId}");
                _orderService.UpdateOrder(order);

                TempData["flashmessage.type"] = "success";
                TempData["flashmessage.text"] = string.Format("delivery for order {0} confirmed, thank you!", order.OrderId);
            }
            else
            {
                TempData["flashmessage.type"] = "danger";
                TempData["flashmessage.text"] = string.Format("order {0} cannot be updated", order.OrderId);
            }
            return Redirect("/User/Orders");
        }
    }

}

