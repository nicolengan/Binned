using Binned.Areas.Identity.Data;
using Binned.Model;
using Binned.Pages.Admin;
using Binned.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using Stripe.Checkout;

namespace Binned.Pages.Payment
{
    public class SuccessModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly CartService _cartService;
        private readonly UserManager<BinnedUser> _userManager;
        private readonly ILogger<SuccessModel> _logger;

        public SuccessModel(UserManager<BinnedUser> userManager, OrderService orderService, ILogger<SuccessModel> logger, CartService cartService)
        {
            _orderService = orderService;
            _logger = logger;
            _userManager = userManager;
            _cartService = cartService;
        }
        [BindProperty]
        public Cart Cart { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var orderId = TempData["id"].ToString();

            _logger.LogInformation($"orderId: {orderId}");

            //var user = User.Identity.Name;

            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);


            //_orderService.AddOrder(newOrder);

            var user = await _userManager.GetUserAsync(User);
            var username = user.UserName;

            
            Cart = await _cartService.GetCartByUserName(username);

            _orderService.UpdateStatusById(orderId, "Paid");
            _cartService.ClearCart(username);


            return Page();
        }
    }
}
