using Binned.Model;
using Binned.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.Admin.Code
{
    [Authorize(Roles = "Admin")]
    public class CodeDetailsModel : PageModel
    {
        private readonly CodeService _codeService;
        private readonly ILogger<CodeDetailsModel> _logger;

        public CodeDetailsModel(CodeService codeService, ILogger<CodeDetailsModel> logger)
        {
            _codeService = codeService;
            _logger = logger;
        }
        [BindProperty]
        public PromoCode? Code { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Code = _codeService.GetCodeById(id);
            _logger.LogInformation($"id: {id}");
            if (Code != null)
            {
                return Page();
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("Code ID {0} not found", id);
                return Redirect("/Admin/Orders");
            }
        }
    }
}
