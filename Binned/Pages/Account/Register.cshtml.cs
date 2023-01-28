using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Binned.Model;

namespace Binned.Pages
{
	public class RegisterModel : PageModel
	{

		private UserManager<IdentityUser> userManager { get; }
		private SignInManager<IdentityUser> signInManager { get; }

		[BindProperty]
		public Register RModel { get; set; }

		public RegisterModel(UserManager<IdentityUser> userManager,
		SignInManager<IdentityUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}



		public void OnGet()
		{
		}


		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var user = new IdentityUser()
				{
					UserName = RModel.Username,
					Email = RModel.Email

				};
				var result = await userManager.CreateAsync(user, RModel.Password);
				if (result.Succeeded)
				{
					await signInManager.SignInAsync(user, false);
					return RedirectToPage("AccountProfile");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return Page();
		}







	}
}
