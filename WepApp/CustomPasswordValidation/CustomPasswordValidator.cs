using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WepApp.Models;

namespace WepApp.CustomPasswordValidation
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            //IdentityResult identityResult = new IdentityResult();
            List<IdentityError> errors = new List<IdentityError>();

            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError { Code = "PasswordContainsUserName", Description = "Şifre Kullanıcı Adı İçeremez" });
            }
            if (password.ToLower().Contains("1234"))
            {
                errors.Add(new IdentityError { Code = "PasswordContains1234", Description = "Şifre ardışık sayı içeremez" });
            }
            if (password.ToLower().Contains(user.Email.ToLower()))
            {
                errors.Add(new IdentityError { Code = "PasswordContainsEmail", Description = "Şifre Email  içeremez" });
            }

            if (errors.Count==0)
            {
                return IdentityResult.Success;//Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return IdentityResult.Failed(errors.ToArray()); //Task.FromResult( IdentityResult.Failed(errors.ToArray()));
            }
        }
    }
}
