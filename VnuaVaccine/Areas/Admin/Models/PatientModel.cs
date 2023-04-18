using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VnuaVaccine.Areas.Admin.Models
{
    public class PatientModel
    {
        public string Name { get; set; }
        [DataType(DataType.Date)]

        public DateTime? Birthday { get; set; }

        public int? Sex { get; set; }

        public string Address { get; set; }

        public int? PhoneNumber { get; set; }

        public int? IdUserName { get; set; }

        public int? Age { get; set; }
    }
}