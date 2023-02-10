using Binned.Areas.Identity.Data;
using Binned.Model;
using Binned.Pages.Payment;
using Binned.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.User
{
    [IgnoreAntiforgeryToken]
    public class OrdersModel : PageModel
    {
        [BindProperty]
        public List<Order> OrderList { get; set; }
        private readonly ILogger<OrdersModel> _logger;
        private readonly OrderService _orderService;
        private readonly UserManager<BinnedUser> userManager;
        public OrdersModel(UserManager<BinnedUser> userManager, OrderService orderService, ILogger<OrdersModel> logger)
        {
            this.userManager = userManager;
            _orderService = orderService;
            _logger = logger;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            //_logger.LogInformation($"Status: {OrderList.Count()}");
            //var user = await userManager.GetUserAsync(User);
            //var username = user.UserName;
            //OrderList = _orderService.GetOrderByUserId(username);
            //foreach (var i in OrderList)
            //{
            //    _logger.LogInformation($"{i.Products}");
            //    foreach (var b in i.Products)
            //    {
            //        _logger.LogInformation($"{b.ProductName}");
            //    }
            //}
            return Page();
        }
        public async Task<ActionResult> OnGetOrderPartial(string status)
        {
            _logger.LogInformation("hello2");
            var user = await userManager.GetUserAsync(User);
            var username = user.UserName;
            if (status == "All Orders" || status == null)
            {
                _logger.LogInformation("all orders");
                OrderList = _orderService.GetOrderByUserId(username);
            }
            else if (status != null)
            {
                _logger.LogInformation($"status not null {status}");
                OrderList = _orderService.FilterOrder(username, status);
                _logger.LogInformation($"length {OrderList.Count()}");
            }
            return Partial("_OrdersPartial", OrderList);
        }

    }
}