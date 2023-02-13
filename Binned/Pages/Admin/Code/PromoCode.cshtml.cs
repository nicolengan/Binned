using Binned.Model;
using Binned.Pages.Payment;
using Binned.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Binned.Pages.Admin.Code
{
    public class PromoCodeModel : PageModel
    {
        private readonly CodeService _codeService;
        private readonly ILogger<PromoCodeModel> _logger;
        public PromoCodeModel(CodeService codeService, ILogger<PromoCodeModel> logger)
        {
            _codeService = codeService;
            _logger = logger;
        }
        [BindProperty]
        public List<PromoCode> CodeList { get; set; }
        public void OnGet()
        {
            CodeList = _codeService.GetAll();
        }
    }
}
