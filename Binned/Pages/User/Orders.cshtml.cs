using Binned.Model;
using Binned.Pages.Payment;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.User
{
    public class OrdersModel : PageModel
    {
        [BindProperty]
        public List<Order> OrderList { get; set; }
        private readonly ILogger<OrdersModel> _logger;
        private readonly OrderService _orderService;
        public OrdersModel(OrderService orderService, ILogger<OrdersModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        public void OnGet()
        {
            OrderList = _orderService.GetOrderByUserId("hello@gmail.com");
            foreach (var i in OrderList)
            {
                _logger.LogInformation($"{i.Products}");
                foreach (var b in i.Products)
                {
                    _logger.LogInformation($"{b.ProductName}");
                }
            }
        }
    }
}