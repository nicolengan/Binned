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
        public string status { get; set; }

        [BindProperty]
        public string orderid { get; set; }
        [BindProperty]
        public Order? Order { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Order = _orderService.GetOrderById(id);
            _logger.LogInformation($"id: {id}");
            orderid = id;
            if (Order != null)
            {
                _logger.LogInformation($"order id{orderid}");
                TempData["id"] = id;
                return Page();
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Order ID {0} not found", id);
                return Redirect("/Admin/Orders");
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // order not found here
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            _logger.LogInformation($"{status}");
            var id = TempData["id"].ToString();
            Order? order = _orderService.GetOrderById(id);

            order.Status = status;
            //_logger.LogInformation($"{order.ProductId}");
            _orderService.UpdateOrder(order);

            TempData["flashmessage.type"] = "success";
            TempData["flashmessage.text"] = string.Format("order {0} is updated", order.OrderId);

            //if (ModelState.IsValid && order != null)
            //{

            //    order.Status = status;
            //    //_logger.LogInformation($"{order.ProductId}");
            //    _orderService.UpdateOrder(order);

            //    TempData["flashmessage.type"] = "success";
            //    TempData["flashmessage.text"] = string.Format("order {0} is updated", order.OrderId);
            //}
            //else
            //{
            //    TempData["flashmessage.type"] = "danger";
            //    TempData["flashmessage.text"] = string.Format("order {0} cannot be updated", order.OrderId);
            //}
            return Redirect("/Admin/Orders");
        }
    }
}
