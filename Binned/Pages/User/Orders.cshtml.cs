using Binned.Areas.Identity.Data;
using Binned.Model;
using Binned.Pages.Payment;
using Binned.Services;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<BinnedUser> userManager;
        public OrdersModel(UserManager<BinnedUser> userManager, OrderService orderService, ILogger<OrdersModel> logger)
        {
            this.userManager = userManager;
            _orderService = orderService;
            _logger = logger;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            var username = user.UserName;
            OrderList = _orderService.GetOrderByUserId(username);
            foreach (var i in OrderList)
            {
                _logger.LogInformation($"{i.Products}");
                foreach (var b in i.Products)
                {
                    _logger.LogInformation($"{b.ProductName}");
                }
            }
            return Page();
        }
    }
}