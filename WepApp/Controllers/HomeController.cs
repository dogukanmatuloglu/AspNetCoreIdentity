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
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser();
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;

               IdentityResult result=await _userManager.CreateAsync(user, userViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("LogIn");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            return View(userViewModel);
        }

        public  IActionResult LogIn(string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByEmailAsync(loginViewModel.Email);
                if (appUser!=null)
                {

                    if (await _userManager.IsLockedOutAsync(appUser))
                    {
                        ModelState.AddModelError("", "Hesabınız bir süreliğine kilitlenmiştir. Daha sonra tekrar deneyiniz");
                    }
                    await _signInManager.SignOutAsync();
                 Microsoft.AspNetCore.Identity.SignInResult result=await _signInManager.PasswordSignInAsync(appUser,loginViewModel.PassWord,loginViewModel.RememberMe,false);

                    if (result.Succeeded)
                    {

                        await _userManager.ResetAccessFailedCountAsync(appUser);

                        if (TempData["ReturnUrl"]!=null)
                        {
                            var d = Redirect(TempData["ReturnUrl"].ToString());
                            return d;
                        }
                        return RedirectToAction("Index", "Member");
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(appUser);
                        int fail = await _userManager.GetAccessFailedCountAsync(appUser);

                        if (fail>=3)
                        {
                            await _userManager.SetLockoutEndDateAsync(appUser, new System.DateTimeOffset(DateTime.Now.AddMinutes(20)));

                            ModelState.AddModelError("", "Hesabınız 3 başarısız girişten dolayı 20 dakika süreyle kitlenmiştir.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Geçersiz Kullanıcı adı veya şifresi"); 
                        }
                    }

                }
            }
            else
            {
                ModelState.AddModelError("", "Bu email adresine kayıtlı kullanıcı bulanamamaıştır.");
            }
            return View(loginViewModel);
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {

            var user = _userManager.FindByEmailAsync(passwordResetViewModel.Email).Result;

            if (user!=null)
            {
                string passwordResetToken = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                string passwordResetLink = Url.Action("ResetPasswordConfirm", "Home", new
                {
                    userId = user.Id,
                    token = passwordResetToken,

                },HttpContext.Request.Scheme) ;
                Helper.PasswordReset.PasswordResetSendEmail(passwordResetLink, passwordResetViewModel.Email, "");
                ViewBag.status = "succes";
            }
            else
            {
                ModelState.AddModelError("", "sistemde kayıtlı e posta adresi bulunamamıştır.");
            }
            return View(passwordResetViewModel);
        }


        public IActionResult ResetPasswordConfirm(string userId,string token)
        {

            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("NewPassWord")]PasswordResetViewModel passwordResetViewModel)
        {
            string token = TempData["token"].ToString();
            string userId = TempData["userId"].ToString();
            var user = await _userManager.FindByIdAsync(userId);
            if (user!=null)
            {
                IdentityResult result = await _userManager.ResetPasswordAsync(user, token, passwordResetViewModel.NewPassWord);

                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                  
                    ViewBag.status = "succes";
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
                ModelState.AddModelError("", "Hata meydana gelmiştir");
            }

            return View(passwordResetViewModel);
        }

    }
}
