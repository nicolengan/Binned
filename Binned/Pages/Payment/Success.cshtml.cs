using Binned.Areas.Identity.Data;
using Binned.Model;
using Binned.Pages.Admin;
using Binned.Services;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        private readonly WishlistService _wishlistService;
        private readonly UserManager<BinnedUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<SuccessModel> _logger;
        private Model.Product OurProduct { get; set; } = new();

        public SuccessModel(UserManager<BinnedUser> userManager, OrderService orderService, ILogger<SuccessModel> logger, CartService cartService, WishlistService wishlistService, IEmailSender emailSender)
        {
            _orderService = orderService;
            _logger = logger;
            _userManager = userManager;
            _cartService = cartService;
            _wishlistService = wishlistService;
            _emailSender = emailSender;
            _emailSender = emailSender;
        }
        [BindProperty]
        public Cart Cart { get; set; }
        public Wishlist Wishlist { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var orderId = TempData["id"].ToString();

            _logger.LogInformation($"orderId: {orderId}");

            var user = await _userManager.GetUserAsync(User);
            var username = user.UserName;


            Cart = await _cartService.GetCartByUserName(username);

            Wishlist = await _wishlistService.GetWishlistByUserName(username);

            _orderService.UpdateStatusById(orderId, "Paid");
            _cartService.UpdateAvailabilityById(orderId, "N");
            await _cartService.ClearCart(username);
            await _wishlistService.ClearCart(username);

            var orderUrl = Url.Page(
                    "/User/OrderDetails",
            pageHandler: null,
                    values: new { email = user.Email, orderId = orderId },
                    protocol: Request.Scheme);

            byte[] fileContent = System.IO.File.ReadAllBytes(@"wwwroot/uploads/EmailImage.png");
            string base64 = Convert.ToBase64String(fileContent);

            var htmlMsg = @$"<html><body><h1>Your order has been confirmed!<img src=""data:image/png;base64,{base64}"" alt=""Confirmation Image""><br>Please allow up to 3-5 days processing time before your order ships. You will receive a shipment confirmation email when your order has shipped from our warehouse. Thank you for your patience.<br>
                </h1><a href='{HtmlEncoder.Default.Encode(orderUrl)}'>Click here</a></h1></body>";


            var subject = "Order Confirmation";
            await _emailSender.SendEmailAsync(user.Email, subject, htmlMsg);
            _logger.LogInformation("Order confirmation sent");

            TempData["FlashMessage.Type"] = "success";
            TempData["FlashMessage.Text"] = "Order confirmation sent to your email.";

            return Page();
        }
    }
}
