using Binned.Model;
using Binned.Pages.Payment;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.User
{
    public class OrdersModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly ILogger<OrdersModel> _logger;

        public OrdersModel(OrderService orderService, ILogger<OrdersModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        public List<Order> OrderList { get; set; } = new();
        public void OnGet()
        {
            OrderList = _orderService.GetOrderByUserId("user1");
        }
    }
}
