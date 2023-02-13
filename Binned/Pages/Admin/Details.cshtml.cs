using Binned.Areas.Identity.Data;
using Binned.Model;
using Binned.Pages.Payment;
using Binned.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Encodings.Web;

namespace Binned.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly ILogger<DetailsModel> _logger;
        private readonly UserManager<BinnedUser> _userManager;
        private readonly IEmailSender _emailSender;

        public DetailsModel(OrderService orderService, ILogger<DetailsModel> logger, UserManager<BinnedUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _orderService = orderService;
            _logger = logger;
            _emailSender = emailSender;
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
            var user = await _userManager.GetUserAsync(User);
            var username = user.UserName;
            //_logger.LogInformation($"{order.ProductId}");
            if (status == "To receive")
            {
                order.ShipDate = DateTime.Now;
                var orderUrl = Url.Page(
                "/User/OrderDetails",
                pageHandler: "Email",
                values: new { email = order.UserId, orderId = order.OrderId },
                protocol: Request.Scheme);

                string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\shippingTemplate.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText.Replace("[ViewOrder]", HtmlEncoder.Default.Encode(orderUrl));

                var subject = "Order Confirmation";
                await _emailSender.SendEmailAsync(user.Email, subject, MailText);
                _logger.LogInformation("Shipping confirmation sent");
            }
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
