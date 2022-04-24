using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WepApp.Models;
using WepApp.ViewModels;
using Mapster;

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

        public IActionResult RoleDelete(string id)
        {
            AppRole role = _roleManager.FindByIdAsync(id).Result;
            if (role!=null)
            {
                IdentityResult result = _roleManager.DeleteAsync(role).Result;
            }
            return RedirectToAction("Roles");
        }

        public  IActionResult RoleUpdate(string id)
        {
            AppRole role = _roleManager.FindByIdAsync(id).Result;
            if (role!=null)
            {
                return View(role.Adapt<RoleViewModel>());
            }
            return RedirectToAction("Roles");
        }
        [HttpPost]
        public IActionResult RoleUpdate(RoleViewModel roleViewModel)
        {
            AppRole role = _roleManager.FindByIdAsync(roleViewModel.Id).Result;
            if (role != null)
            {
                role.Name = roleViewModel.Name;
                IdentityResult result = _roleManager.UpdateAsync(role).Result;

                if (result.Succeeded)
                {
                  return  RedirectToAction("Roles");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Güncelleme İşlemi Başarısız OLdu");
            }
            return View(roleViewModel);
        }


        public IActionResult RoleAssign(string id)
        {
            TempData["userId"] = id;
            AppUser user = _userManager.FindByIdAsync(id).Result;
            ViewBag.userName = user.UserName;
            var roles = _roleManager.Roles;
            List<string> userroles = _userManager.GetRolesAsync(user).Result as List<string>;
            List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();
            foreach (var item in roles)
            {
                RoleAssignViewModel r = new RoleAssignViewModel();
                r.RoleId = item.Id;
                r.RoleName = item.Name;
                if (userroles.Contains(item.Name))
                {
                    r.Exist = true;
                }
                else
                {
                    r.Exist = false;
                }
                roleAssignViewModels.Add(r);
            }
            return View(roleAssignViewModels);

        }
        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels)
        {

            AppUser user = _userManager.FindByIdAsync(TempData["userId"].ToString()).Result;
            foreach (var item in roleAssignViewModels)
            {
                if (item.Exist==true)
                {
                  await  _userManager.AddToRoleAsync(user, item.RoleName);
                }
                else
                {
                   await _userManager.RemoveFromRoleAsync(user, item.RoleName);
                }

            }
            return RedirectToAction("Users");
        }
    }
}
