using Binned.Areas.Identity.Data;
using Binned.Services;
using Binned.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.Admin
{
    public class AccountDetailsModel : PageModel
    {

        private readonly AccountService _accountService;
        private IWebHostEnvironment _environment;
        private readonly UserManager<BinnedUser> _userManager;



        public AccountDetailsModel(AccountService accountService, IWebHostEnvironment environment, UserManager<BinnedUser> userManager)
        {
            _accountService = accountService;
            _environment = environment;
            _userManager = userManager;
        }

        [BindProperty]
        public BinnedUser MyBinnedUser { get; set; } = new();

        [BindProperty]
        public IFormFile? Upload { get; set; }

        [BindProperty]
        public UsersViewModel MyUsers { get; set; }

        public IActionResult OnGet(string id)
        {

            BinnedUser? user = _accountService.GetAccountById(id);
            UsersViewModel abc = _accountService.GetAccount(id);
            if (user != null)
            {
                MyBinnedUser = user;
                if (abc != null)
                {
                    MyUsers = abc;
                }
                else
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = string.Format("User ID {0} not found", id);
                    return Redirect("/Admin/Roles");
                }

                return Page();
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("User ID {0} not found", id);
                return Redirect("/Admin/Roles");
            }

            


        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }

                    var uploadsFolder = "uploads";
                    if (MyBinnedUser.ImageURL != null)
                    {
                        var oldImageFile = Path.GetFileName(MyBinnedUser.ImageURL);
                        var oldImagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, oldImageFile);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    MyBinnedUser.ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);

                }

                var result = await _userManager.FindByEmailAsync(MyUsers.Email);
                MyUsers.Id = result.Id;
                MyUsers.Username = result.UserName;
                MyUsers.Email = result.Email;

                if (result != null)
                {
                    
                    if (MyUsers.Role != null)
                    {
                        if (MyUsers.Role == "Member")
                        {
                            await _userManager.AddToRoleAsync(MyBinnedUser, "Member");
                        }
                        else if (MyUsers.Role == "Admin")
                        {
                            await _userManager.AddToRoleAsync(MyBinnedUser, "Admin");
                        }
                        //_accountService.UpdateRoles(MyUsers.Role);
                        
                    }
                    
                    return RedirectToPage("/Admin/Roles");
                }




                _accountService.UpdateUser(MyBinnedUser);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("User {0} is updated", MyBinnedUser.FirstName);
            }
            return Page();
        }


    }
}
