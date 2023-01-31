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
        public void OnGet(string id)
        {

            OneOrder = _orderService.GetOrderById(id);
            //_logger.LogInformation(OneOrder.Products.Count().ToString());
            //if (OneOrder.Status == "To receive")
            //{
            //    Button = "Order received";
            //}

        }
        public IActionResult OnPost()
        {
            _logger.LogInformation("hi");
            return Redirect("/");
        }
    }
}
