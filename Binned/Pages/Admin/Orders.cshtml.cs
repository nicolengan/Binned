using Binned.Model;
using Binned.Pages.User;
using Binned.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Binned.Pages.Admin
{
    public class Status
    {
        public string Value { get; set; }
        public string Name { get; set; }

        public static IEnumerable<Status> Statuses = new List<Status> {
            new Status {
                Value = "To Pay",
                Name = "To Pay"
            },
            new Status {
                Value = "To Ship",
                Name = "To Ship"
            },
            new Status {
                Value = "To receive",
                Name = "To receive"
            },
            new Status {
                Value = "Delivered",
                Name = "Delivered"
            }
        };

    }
    [Authorize(Roles = "Admin")]
    public class OrdersModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly ILogger<OrdersModel> _logger;
        public string myStatus { get; } = "To Ship";
        //public string jsonString { get; set; }

        public OrdersModel(OrderService orderService, ILogger<OrdersModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        public List<Order> OrderList { get; set; } = new();
        public void OnGet(int myStatus)
        {
            OrderList = _orderService.GetAll();
            //jsonString = JsonSerializer.Serialize(OrderList);
            _logger.LogInformation($"{myStatus}");

        }
        public IActionResult OnPost(int myStatus)
        {

            _logger.LogInformation($"output{myStatus}");

            return Page();
        }
    }
}
