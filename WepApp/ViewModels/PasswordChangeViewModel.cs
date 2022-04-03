using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WepApp.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Display(Name ="Eski Şifreniz")]
        [Required(ErrorMessage ="Eski Şifreniz Gereklidir.")]
        [DataType(dataType:DataType.Password)]
        [MinLength(4,ErrorMessage ="Şifreniz en az 4 karakter olmalıdır.")]
        public string PasswordOld { get; set; }

        [Display(Name = "Yeni Şifreniz")]
        [Required(ErrorMessage = "Yeni Şifreniz Gereklidir.")]
        [DataType(dataType: DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakter olmalıdır.")]
        public string PasswordNew { get; set; }
        [Display(Name = " Onay Yeni Şifreniz")]
        [Required(ErrorMessage = " Onay Yeni Şifreniz Gereklidir.")]
        [DataType(dataType: DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakter olmalıdır.")]
        [Compare("PasswordNew",ErrorMessage ="Yeni şifreniz ve onay şifreniz birbirinden farklıdır.")]
        public string PsswordConfirm { get; set; }
    }
}
