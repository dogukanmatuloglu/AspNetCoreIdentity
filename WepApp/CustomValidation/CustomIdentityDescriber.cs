﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WepApp.CustomValidation
{
    public class CustomIdentityDescriber:IdentityErrorDescriber
    {
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = "InvalidUserName",
                Description = $" Bu {userName} geçersizdir"
            };

           
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = "DuplicateError",
                Description = $"Bu {email} kullanılmaktadır"
            };
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = "PasswordTooShort",
                Description = $"Şifreniz en az {length} karakterde olmalıdır"
            };
        }
    }
}
