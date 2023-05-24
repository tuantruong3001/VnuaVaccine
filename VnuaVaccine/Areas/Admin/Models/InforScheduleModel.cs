using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VnuaVaccine.Areas.Admin.Models
{
    public class InforScheduleModel
    {
        public int ID { get; set; }
        public int IdUserName { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public string NameVaccine { get; set; }
        public string Path { get; set; }
        public string NamePatient { get; set; }
        public string Munafacturer { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? Time { get; set; }
    }
}