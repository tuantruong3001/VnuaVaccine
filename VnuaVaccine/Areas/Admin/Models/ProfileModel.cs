using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VnuaVaccine.Areas.Admin.Models
{
    public class ProfileModel
    {
        [Required(ErrorMessage = "Email không được để trống!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "UserName không được để trống!")]
        //[RegularExpression(@"^.{2,10}$", ErrorMessage = "{0} chỉ từ 2 đến 10 ký tự.")]

        public string UserName { get; set; }

        //[RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,20}).*$", ErrorMessage = "{0} phải từ 8-20 kí tự bao gồm ít nhất 1 kí tự số, 1 kí tự viết hoa và 1 kí tự đặc biệt.")]
        //[Required(ErrorMessage = "Password không được để trống!")]
        public string Password { get; set; }

        //[RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,20}).*$", ErrorMessage = "{0} phải từ 8-20 kí tự bao gồm ít nhất 1 kí tự số, 1 kí tự viết hoa và 1 kí tự đặc biệt.")]
        //[Required(ErrorMessage = "ConfirmPassword không được để trống!")]
        public string ConfirmPassword { get; set; }
        public int ID { get; set; }
        public int? Role { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public string Address { get; set; }
        public int? PhoneNumber { get; set; }
        public int IdUserName { get; set; }
        public int Age { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{{0:dd/MM/yyyy}")]
        public DateTime? Birthday { get; set; }
        public DateTime? UpdateAt { get; set; }

    }
}