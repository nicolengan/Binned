using Binned.Model;
using Binned.Pages.Payment;
using Binned.Services;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Stripe;

namespace Binned.Pages.Admin.Code
{
    [Authorize(Roles = "Admin")]
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
        [BindProperty]
        public AddInput AddInput { get; set; }
        [BindProperty]
        public EditInput? EditInput { get; set; }
        public PromoCode NewCode { get; set; }
        public PromoCode EditCode { get; set; }
        public void OnGet()
        {
            CodeList = _codeService.GetAll();
        }
        public async Task<IActionResult> OnPost()
        {
            _logger.LogInformation("hello");
            if (ModelState.IsValid)
            {
                _logger.LogInformation("valid");
                NewCode = new PromoCode
                {
                    Name = AddInput.Name,
                    ExpiryDate = (DateTime)AddInput.ExpiryDate,
                    Discount = (double)AddInput.Discount,
                    Active = true
                };
                _codeService.AddCode(NewCode);

                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = "Code Added";
                CodeList = _codeService.GetAll();
                return Redirect("/Admin/Code/PromoCode");
            }
            return Page();
        }

        public async Task<IActionResult> OnGetEditCode(int id)
        {
            CodeList = _codeService.GetAll();
            EditCode = _codeService.GetCodeById(id);
            if (EditCode == null)
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = "Code not found";
                return new JsonResult(null);
            }
            return new JsonResult(new { EditCode });
        }
        public async Task OnPostEditCode(int id)
        {
            if (ModelState.IsValid)
            {
                EditCode = _codeService.GetCodeByName(EditInput.Name);
                EditCode.Name = EditInput.Name;
                EditCode.ExpiryDate = (DateTime)EditInput.ExpiryDate;
                EditCode.Discount = (double)EditInput.Discount;
                _logger.LogInformation(EditCode.Name);
                _codeService.UpdateCode(EditCode);
                CodeList = _codeService.GetAll();
            }
        }
    }
    public class AddInput
    {
        public string? Name { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }
        public double? Discount { get; set; }
    }
    public class EditInput
    {
        public string? Name { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }
        public double? Discount { get; set; }
    }
}
