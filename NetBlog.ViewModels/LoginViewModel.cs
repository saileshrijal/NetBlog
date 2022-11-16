using NetBlog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetBlog.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public LoginViewModel() { }

        public LoginViewModel(ApplicationUser model)
        {
            UserName = model.UserName;
        }

        public ApplicationUser ConvertViewModel(LoginViewModel model)
        {
            return new ApplicationUser
            {
                UserName = model.UserName
            };
        }

    }
}
