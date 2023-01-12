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
        public int orderid { get; set; }

        public IActionResult OnGet(int id)
        {
            Order? order = _orderService.GetOrderById(id);
            _logger.LogInformation($"id: {id}");
            if (order != null)
            {
                orderid = id;
                _logger.LogInformation($"order id{id}");
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
            _logger.LogInformation($"2nd id{orderid}");
            Order? order = _orderService.GetOrderById(orderid);


            if (ModelState.IsValid)
            {
                order.Status = status;
                _logger.LogInformation($"{order.ProductId}");

                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Order {0} is updated", order.OrderId);
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Order {0} is updated", order.OrderId);
            }
            return Page();
        }
    }
}
    