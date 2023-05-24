using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VnuaVaccine.Models
{
    public class CartItem
    {
        public Vaccine Vaccine { get; set; }
        public int ID { get; set; }
        public DateTime? Time { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? Quantity { get; set; }
        public int? IdPatient { get; set; }
        public int? Status { get; set; }
        public int? IdVaccine { get; set; }
    }

}