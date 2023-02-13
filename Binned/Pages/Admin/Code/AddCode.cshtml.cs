using Binned.Areas.Identity.Data;
using Binned.Model;
using Binned.Pages.Payment;
using Binned.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.Admin.Code
{
    public class AddCodeModel : PageModel
    {
        private readonly CodeService _codeService;
        private readonly ILogger<AddCodeModel> _logger;
        public AddCodeModel(CodeService codeService, ILogger<AddCodeModel> logger)
        {
            _codeService = codeService;
            _logger = logger;
        }
        [BindProperty]
        public PromoCode PromoCode { get; set; }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost()
        {
            _logger.LogInformation("hello");
            if (ModelState.IsValid)
            {
                _logger.LogInformation("valid");
                PromoCode.Active = true;
                _codeService.AddCode(PromoCode);

                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = "Code Added";
                return Redirect("/Admin/Code/PromoCode");
            }
            return Page();
        }
    }
}
