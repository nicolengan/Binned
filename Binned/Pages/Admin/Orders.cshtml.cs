using Binned.Model;
using Binned.Pages.User;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;

namespace Binned.Pages.Admin
{
    public class Status
    {
        public int StatusId { get; set; }
        public string Name { get; set; }

    }
    public class OrdersModel : PageModel
    {
        public static IEnumerable<Status> Statuses = new List<Status> {
            new Status {
                StatusId = 1,
                Name = "To Ship"
            },
            new Status {
                StatusId = 2,
                Name = "To receieve"
            },
            new Status {
                StatusId = 3,
                Name = "Delivered"
            }
        };
        private readonly OrderService _orderService;
        private readonly ILogger<OrdersModel> _logger;
        public string myStatus { get; } = "To Ship";

        public OrdersModel(OrderService orderService, ILogger<OrdersModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        public List<Order> OrderList { get; set; } = new();
        public void OnGet(int myStatus)
        {
            OrderList = _orderService.GetAll();
            _logger.LogInformation($"{myStatus}");

        }
        public IActionResult Onpost(int myStatus)
        {
            //if (ModelState.IsValid)
            //{
            //    _orderService.UpdateOrder();
            //    TempData["FlashMessage.Type"] = "success";
            //    TempData["FlashMessage.Text"] = string.Format(
            //    "Employee {0} is updated", MyEmployee.Name);
            //}
            _logger.LogInformation($"output{myStatus}");

            return Page();
        }
    }
}
