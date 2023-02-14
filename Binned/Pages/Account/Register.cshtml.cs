using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Binned.Model;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Binned.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Binned.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<BinnedUser> _signInManager;
        private readonly UserManager<BinnedUser> _userManager;
        private readonly IUserStore<BinnedUser> _userStore;
        private readonly IUserEmailStore<BinnedUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<BinnedUser> userManager,
            IUserStore<BinnedUser> userStore,
            SignInManager<BinnedUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {

            [Required]
            [StringLength(255)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(255)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }


            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string? ConfirmPassword { get; set; }


            [DataType(DataType.Text)]
            [Display(Name = "Admin Number (Enter 0 if don't have)")]
            public string AdmNo { get; set; }
        }



        // Roles management
        /*public class RolesManagement
        {
            public static async Task Initialize(MyDbContext context,
           UserManager<BinnedUser> userManager,
           RoleManager<IdentityRole> roleManager)
            {

                context.Database.EnsureCreated();

                string adminRole = "Admin";
                string memberRole = "Member";

                if (await roleManager.FindByNameAsync(adminRole) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(adminRole));
                }

                if (await roleManager.FindByNameAsync(memberRole) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(memberRole));
                }

                if (await userManager.FindByNameAsync("binned@binned.com") == null)
                {
                    var user = new BinnedUser
                    {
                        UserName = "binned@binned.com",
                        Email = "binned@binned.com",
                        FirstName = "Binned",
                        LastName = "Binned"
                    };

                    var result = await userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, adminRole);
                    }
                }
            }
        }*/




        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {

            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;

                IdentityRole role = await _roleManager.FindByIdAsync("Admin");
                
                if (role == null)
                {
                    IdentityResult result2 = await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    if (!result2.Succeeded)
                    {
                        ModelState.AddModelError("", "Create role admin failed");
                    }
                }

                IdentityRole userRole = await _roleManager.FindByIdAsync("Member");
                if (userRole == null)
                {
                    IdentityResult result3 = await _roleManager.CreateAsync(new IdentityRole("Member"));
                    if (!result3.Succeeded)
                    {
                        ModelState.AddModelError("", "Create role member failed");
                    }
                }



                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    /*var defaultrole = _roleManager.FindByNameAsync("Member").Result;

                    if (defaultrole != null)
                    {
                        IdentityResult roleresult = await _userManager.AddToRoleAsync(user, defaultrole.Name);
                    }*/

                    //Add users to role, incomplete
                    if (Input.AdmNo.ToLower() == "211717c" || Input.AdmNo.ToLower() == "214247c"|| Input.AdmNo.ToLower() == "213041g"|| Input.AdmNo.ToLower() == "213270u")
                    {
                        result = await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        result = await _userManager.AddToRoleAsync(user, "Member");
                    }
                    


                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(value: callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private BinnedUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<BinnedUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(BinnedUser)}'. " +
                    $"Ensure that '{nameof(BinnedUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<BinnedUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<BinnedUser>)_userStore;
        }
    }
}
