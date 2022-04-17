using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WepApp.Models;
using WepApp.ViewModels;

namespace WepApp.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;
        public AdminController(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public  async Task<IActionResult> Users()
        {
           var users=_userManager.Users.ToList();
            return View(users);
        }


        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RoleCreate(RoleViewModel roleViewModel)
        {

            AppRole role = new AppRole();
            role.Name = roleViewModel.Name;
            IdentityResult result = _roleManager.CreateAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(roleViewModel);
        }
        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }
    }
}
