using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WepApp.ViewModels
{
    public class RoleViewModel
    {
        [Display(Name="Role İsmi")]
        [Required(ErrorMessage ="Role ismi gereklidir.")]
        public string Name { get; set; }
        public int Id { get; set; }

    }
}
