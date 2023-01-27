using Binned.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.Account
{
	public class AccountProfileModel : PageModel
	{

		/*[BindProperty]
		public Login LModel { get; set; }

		private readonly SignInManager<IdentityUser> signInManager;
		public AccountProfileModel(SignInManager<IdentityUser> signInManager)
		{
			this.signInManager = signInManager;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			if (ModelState.IsValid)
			{
				var identityResult = await signInManager.PasswordSignInAsync(LModel.Username, LModel.Password,
				LModel.RememberMe, false);
				if (identityResult.Succeeded)
				{
					var username = LModel.Username;
				}
				ModelState.AddModelError("", "Username or Password incorrect");
			}
			return Page();
		}*/



		public void OnGet()
        {

        }
    }
}
