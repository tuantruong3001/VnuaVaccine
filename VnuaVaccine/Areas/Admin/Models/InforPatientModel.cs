using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VnuaVaccine.Areas.Admin.Models
{
    public class InforPatientModel
    {
        public string Email { get; set; }      
        public string UserName { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public int? Sex { get; set; }
        public string Address { get; set; }

        public int? PhoneNumber { get; set; }
        public int IdUserName { get; set; }
        public int? Age { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
       
    }
}
