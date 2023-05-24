using DAL.Dao;
using System.Web.Mvc;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class VaccineScheduleController : BaseController
    {
       
        // GET: Admin/VaccineSchedule
        private readonly ScheduleDAO _scheduleDao = new ScheduleDAO();

        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var model = _scheduleDao.ListAllPaging(page, pageSize);
            return View(model);
        }

    }
}