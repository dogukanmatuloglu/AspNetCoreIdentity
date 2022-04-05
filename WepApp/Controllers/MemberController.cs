using Mapster;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class MemberController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
          
            AppUser user = _userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();
            return View(userViewModel);
        }


        public IActionResult UserEdit()
        {
            AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();


            return View(userViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> UserEdit(UserViewModel userViewModel)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                user.UserName = userViewModel.UserName;
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.Email = userViewModel.Email;
               IdentityResult result=  await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    
                  await   _userManager.UpdateSecurityStampAsync(user);
                   await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, true);
                    ViewBag.succes = "true";
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View(userViewModel);
        }
        public IActionResult PasswordChange()
        {

           
            return View();
        }
        [HttpPost]
        public IActionResult PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                if (User!=null)
                {
                    bool exist = _userManager.CheckPasswordAsync(user, passwordChangeViewModel.PasswordOld).Result;
                    if (exist)
                    {
                        IdentityResult ıdentityResult = _userManager.ChangePasswordAsync(user, passwordChangeViewModel.PasswordOld, passwordChangeViewModel.PasswordNew).Result;
                        if (ıdentityResult.Succeeded)
                        {
                           
                            _userManager.UpdateSecurityStampAsync(user);
                            _signInManager.SignOutAsync();
                            _signInManager.PasswordSignInAsync(user, passwordChangeViewModel.PasswordNew, true, false);
                            ViewBag.succes = "true";
                        }
                        else
                        {
                            foreach (var item in ıdentityResult.Errors)
                            {
                                ModelState.AddModelError("", item.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Eski Şifreniz yanlıştır");
                    }
                }

            }

            return View();
        }

        public void LogOut()
        {
            _signInManager.SignOutAsync();

            
        }
    }
}
