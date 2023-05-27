using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VnuaVaccine.Areas.Admin.Models
{
    public class VaccineModel
    {
        public int Id { get; set; }
        public string NameVaccine { get; set; }
        public string Munafacturer { get; set; }
        public string Description { get; set; }
        public int? QuantityStock { get; set; }
        public int? Times { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ProductionDate { get; set; }
        public DateTime? ExpirationData { get; set; }
        public string Path { get; set; }
        public string Note { get; set; }
    }
}