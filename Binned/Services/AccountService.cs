using System;
using System.Linq;
using Binned.Areas.Identity.Data;
using Binned.Model;
using Binned.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Binned.Services
{
    public class AccountService
    {
        private readonly MyDbContext _context;
        private readonly UserManager<BinnedUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly UsersViewModel _usersViewModel;


        public AccountService(MyDbContext context, RoleManager<IdentityRole> roleManager, UserManager<BinnedUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            //_usersViewModel = usersViewModel;   
        }

        public List<BinnedUser> GetAll()
        {
            var userData = _context.Users.OrderBy(m => m.Id).ToList();
            //userData.Add(_roleManager.Roles.ToList());
            return userData;
        }

        public List<UsersViewModel> GetAllRoles()
        {
            /*var listado = (from user in _context.Users
                           join userRoles in _context.UserRoles on user.Id equals userRoles.UserId
                           join role in _context.Roles on userRoles.RoleId equals role.Id
                           select new { Email = user.UserName, RoleName = role.Name }).ToListAsync();*/
            /*var result = _context.Users.Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur }).Join(_context.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r }).Select(c => new UsersViewModel()
            {
                Username = c.ur.u.UserName,
                Email = c.ur.u.Email,
                Role = c.r.Name
            }).ToList().GroupBy(uv => new { uv.Username, uv.Email }).Select(r => new UsersViewModel()
            {
                Username = r.Key.Username,
                Email = r.Key.Email,
                Role = string.Join(",", r.Select(c => c.Role).ToArray())
            }).ToList();*/

            var result2 = _context.Users
        .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
        .Join(_context.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
        .ToList()
        .GroupBy(uv => new { uv.ur.u.Id, uv.ur.u.UserName, uv.ur.u.Email }).Select(r => new UsersViewModel()
        {
            Id = r.Key.Id,
            Username = r.Key.UserName,
            Email = r.Key.Email,
            Role = string.Join(",", r.Select(c => c.r.Name).ToArray())
        }).ToList();

            /*var users = _userManager.Users.Select(c => new UsersViewModel()
            {
                Username = c.UserName,
                Email = c.Email,
                Role = string.Join(",", _userManager.GetRolesAsync(c).Result.ToArray())
            }).ToList();*/

            //var listado = _context.Roles.OrderBy(a => a.Name).ToList();

            return result2;
        }


        public BinnedUser? GetAccountById(string id)
        {
            BinnedUser? binnedUser = _context.Users.FirstOrDefault(x => x.Id.Equals(id));
            return binnedUser;
        }

        public UsersViewModel GetAccount(string Id)
        {
            UsersViewModel abc = _context.Users.Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
        .Join(_context.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
        .ToList()
        .GroupBy(uv => new { uv.ur.u.Id, uv.ur.u.UserName, uv.ur.u.Email }).Select(r => new UsersViewModel()
        {
            Id = r.Key.Id,
            Username = r.Key.UserName,
            Email = r.Key.Email,
            Role = string.Join(",", r.Select(c => c.r.Name).ToArray())
        }).FirstOrDefault(x => x.Id.Equals(Id));
            return abc;
        }
        /*public void AddEmployee(BinnedUser binnedUser)
        {
            _context.Users.Add(binnedUser);
            _context.SaveChanges();
        }*/

        public void UpdateUser(BinnedUser binnedUser)
        {
            _context.Users.Update(binnedUser);
            _context.SaveChanges();
        }

        /*public void UpdateRoles(BinnedUser binnedRole)
        {
            var model = new UsersViewModel();
            MyDbContext myDbContext = new MyDbContext();
            var rolesFromDb = myDbContext.Roles.ToList();
            model.Role = rolesFromDb.Select(r => new IdentityRole { Id = r.Id, Name = r.Name }).ToList();


            BinnedUser qwerty = _context.Users.Update(binnedRole);
            _context.SaveChanges();
        }*/
    }
}
