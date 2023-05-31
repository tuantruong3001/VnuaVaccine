using System.Collections.Generic;
using System.Web.Mvc;

namespace VnuaVaccine.Areas.Admin.Models
{
    public class CreateScheduleViewModel
    {
        public int SelectedPatientId { get; set; }
        public IEnumerable<SelectListItem> PatientList { get; set; }
        public IEnumerable<SelectListItem> VaccineList { get; set; }
        public InforScheduleModel ScheduleModel { get; set; }
    }
}