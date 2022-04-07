using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WepApp.Enums;

namespace WepApp.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage ="Kullanıcı İsmi Gereklidir.")]
        [Display(Name ="Kullanıcı Adı")]
        public string UserName { get; set; }
        [Display(Name ="Tel No")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email Gereklidir.")]
        [Display(Name = "Email Adresi")]
        [EmailAddress(ErrorMessage = "Email adresiniz doğru formatta değil")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifreniz Gereklidir.")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime? BirthDay { get; set; }
        [Display(Name = "Resim")]
        public string Picture { get; set; }
        [Display(Name = "Şehir")]
        public string City { get; set; }
        [Display(Name = "Cinsiyet")]
        public Gender Gender { get; set; }
    }
}
