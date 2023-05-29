using System.Collections.Generic;

namespace VnuaVaccine.Areas.Admin.Models
{
    public class DetailViewModel
    {
        public InforPatientModel ProfileModel { get; set; }
        public List<string> NameVaccineList { get; set; }
    }

}