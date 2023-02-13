using Binned.Areas.Identity.Data;
using Binned.Model;
using Binned.Pages.Admin;
using Binned.Services;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sib_api_v3_sdk.Model;
using Stripe;
using Stripe.Checkout;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.Encodings.Web;

namespace Binned.Pages.Payment
{
    public class SuccessModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly CartService _cartService;
        private readonly UserManager<BinnedUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<SuccessModel> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private Model.Product OurProduct { get; set; } = new();

        public SuccessModel(UserManager<BinnedUser> userManager, OrderService orderService, ILogger<SuccessModel> logger, CartService cartService, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            _orderService = orderService;
            _logger = logger;
            _userManager = userManager;
            _cartService = cartService;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
        }
        [BindProperty]
        public Cart Cart { get; set; }
        public async Task<IActionResult> OnGet()
        {
            TempData["FlashMessage.Type"] = "success";
            TempData["FlashMessage.Text"] = "Payment successful, confirmation sent to your email.";

            var orderId = TempData["id"].ToString();

            _logger.LogInformation($"orderId: {orderId}");

            var user = await _userManager.GetUserAsync(User);
            var username = user.UserName;


            Cart = await _cartService.GetCartByUserName(username);

            _orderService.UpdateStatusById(orderId, "To Ship");
            await _cartService.ClearCart(username);

            var orderUrl = Url.Page(
                    "/User/OrderDetails",
                    pageHandler: "Email",
                    values: new { email = user.Email, orderId = orderId },
                    protocol: Request.Scheme);
            _cartService.ClearCart(username);
            OurProduct.Availability = "N";

            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\orderTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", user.FirstName).Replace("[ViewOrder]", HtmlEncoder.Default.Encode(orderUrl));

            var subject = "Order Confirmation";
            await _emailSender.SendEmailAsync(user.Email, subject, MailText);
            _logger.LogInformation("Order confirmation sent");
            return Page();
        }
    }
}
