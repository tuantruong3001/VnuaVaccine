using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VnuaVaccine.Areas.Admin.Models
{
    public class InforScheduleModel
    {
        public InforScheduleModel()
        {
            
        }
        public string NameVaccine { get; set; }
        public string NamePatient { get; set; }
        public int ID { get; set; }
        public int? IdPatient { get; set; }
        public int? Quantity { get; set; }
        public int? IdVaccine { get; set; }
        public int? Status { get; set; }
        public int? Times { get; set; }
        public DateTime? Time { get; set; }
        public DateTime? CreateAt { get; set; }
    }

}