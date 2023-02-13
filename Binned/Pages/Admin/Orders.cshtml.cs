using Binned.Model;
using Binned.Pages.User;
using Binned.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
using IronPdf;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Binned.Pages.Admin
{
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
        public List<Order> TotalOrders { get; set; }
        public List<Order> CurrentOrders { get; set; }
        public List<Order> LastOrders { get; set; }
        [BindProperty]
        public int increase { get; set; }
        public void OnGet(int myStatus)
        {

            var now = DateTime.Now;
            var lastMonth = now.AddMonths(-1);
            LastOrders = _orderService.GetOrderByMonth(lastMonth.Month, lastMonth.Year);
            CurrentOrders = _orderService.GetOrderByMonth(now.Month, now.Year);
            TotalOrders = _orderService.GetAll();
            _logger.LogInformation($"current month = {CurrentOrders.Count()}");
            _logger.LogInformation($"current month = {LastOrders.Count()}");
            _logger.LogInformation($"{myStatus}");

            try
            {
                increase = (CurrentOrders.Count() / LastOrders.Count()) * 100;
            }
            catch (DivideByZeroException d)
            {
                increase = 0;
            }

        }
        public IActionResult OnPost(int myStatus)
        {
            _logger.LogInformation($"output{myStatus}");
            return Page();
        }
    }
}
