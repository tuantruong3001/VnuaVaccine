using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VnuaVaccine.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email không được để trống!")]
        [EmailAddress(ErrorMessage = "Email chưa đúng định dạng.")]
        [RegularExpression(@"^.{10,30}$", ErrorMessage = "{0} chỉ từ 10 đến 30 ký tự.")]
        public string Email { get; set; }

       /* [RegularExpression(@"^.{8,20}$", ErrorMessage = "Mật khẩu chỉ từ 8 đến 20 ký tự.")]*/
        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}