
using Binned.Areas.Identity.Data;
using Binned.Model;
using Binned.Services;
using Binned.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Binned.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class RolesModel : PageModel
    {
        private readonly AccountService _accountService;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly UsersViewModel _usersViewModel;
        private readonly MyDbContext _context;

        public RolesModel(AccountService accountService, RoleManager<IdentityRole> roleManager, MyDbContext context)
        {
            _accountService = accountService;
            //_roleManager = roleManager;
            //_usersViewModel = usersViewModel;
            _context = context; 
        }

        //public List<BinnedUser> AccountList { get; set; } = new();
        public List<UsersViewModel> RoleList { get; set; } = new();

        public void OnGet()
        {
            //AccountList = _accountService.GetAll();
            RoleList = _accountService.GetAllRoles();
        }
    }
}
