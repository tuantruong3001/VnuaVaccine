﻿using System;

namespace VnuaVaccine.Areas.Admin.Models
{
    public class InforPatientModel
    {
        public string Email { get; set; }      
        public string UserName { get; set; }
        public int? ID { get; set; }
        public string Name { get; set; }
        public string NameParent { get; set; }
        public int? Sex { get; set; }
        public string Address { get; set; }
       
        public int? PhoneNumber { get; set; }
        public int? IdUserName { get; set; }
        public int? Age { get; set; }
        public int? AgeMonth { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public int? IdPatient { get; set; }
        public int? IdVaccine { get; set; }
        public string NameVaccine { get; set; }
        public string NamePatient { get; set; }

    }
}
