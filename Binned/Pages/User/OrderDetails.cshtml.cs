using Binned.Areas.Identity.Data;
using Binned.Model;
using Binned.Pages.Payment;
using Binned.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.User
{
    public class OrderDetailsModel : PageModel
    {
        [BindProperty]
        public Order? OneOrder { get; set; }
        //[BindProperty]
        //public string Button { get; set; }
        private readonly OrderService _orderService;
        private readonly ILogger<OrderDetailsModel> _logger;
        private readonly UserManager<BinnedUser> userManager;
        public OrderDetailsModel(OrderService orderService, ILogger<OrderDetailsModel> logger, UserManager<BinnedUser> userManager)
        {
            this.userManager = userManager;
            _orderService = orderService;
            _logger = logger;
        }
        public async Task<IActionResult> OnGet(string id)
        {
            var user = await userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                OneOrder = _orderService.GetOrderById(id);
                if (user.Email == OneOrder.UserId)
                {
                    return Page();
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
            }
            return Redirect("/Account/Orders");

        }
        public void OnPost(string id)
        {
            OneOrder = _orderService.GetOrderById(id);
            _orderService.UpdateStatusById(id, "Received");
            TempData["FlashMessage.Type"] = "success";
            TempData["FlashMessage.Text"] = string.Format("Order status updated! Your order status has been changed to received.");
            _logger.LogInformation(id);
        }

        public async Task<IActionResult> OnGetEmail(string email, string orderId)
        {
            var user = await userManager.FindByEmailAsync(email);
            OneOrder = _orderService.GetOrderById(orderId);
            if (user == null! || email != OneOrder.UserId)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            return Page();
        }
    }
}
