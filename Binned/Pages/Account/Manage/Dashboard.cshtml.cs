// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Serialization;
using Binned.Areas.Identity.Data;

namespace Binned.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {

        private readonly UserManager<BinnedUser> _userManager;
        private readonly SignInManager<BinnedUser> _signInManager;

        public IndexModel(
            UserManager<BinnedUser> userManager,
            SignInManager<BinnedUser> signInManager)
            
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }
        //public string Firstname { get; set; }
        //public string Lastname { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }


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
        public class InputModel
        {
            /*public class binnedAccountUser : IdentityUser
            {
                public string FirstName { get; set; }
                public string LastName { get; set; }
            }*/
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Text), MinLength(1, ErrorMessage = "Enter at least 1 character."), MaxLength(255)]
            [Display(Name = "FirstName")]
            public string FirstName { get; set; }

            [DataType(DataType.Text), MinLength(1, ErrorMessage = "Enter at least 1 character."), MaxLength(255)]
            [Display(Name = "LastName")]
            public string LastName { get; set; }
        }

        private async Task LoadAsync(BinnedUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
           
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.FirstName!= user.FirstName)
            {
                if (Input.FirstName != null)
                {
                    user.FirstName = Input.FirstName;
                }            
            }
            if (Input.LastName != user.LastName)
            {
                if (Input.LastName != null) {
                    user.LastName = Input.LastName;
                }
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
