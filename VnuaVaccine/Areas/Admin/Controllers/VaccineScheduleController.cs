using DAL.Dao;
using DAL.EF;
using PagedList;
using System.Linq;
using System.Web.Mvc;
using VnuaVaccine.Areas.Admin.Models;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class VaccineScheduleController : BaseController
    {

        // GET: Admin/VaccineSchedule
        private readonly ScheduleDAO _scheduleDao = new ScheduleDAO();

        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var pagedModel = _scheduleDao.ListAllPaging(searchString, page, pageSize);
            var model = new StaticPagedList<InforScheduleModel>(
                pagedModel.Select(infor =>
                {
                    var vaccine = _scheduleDao.GetVaccineName((int)infor.IdVaccine);
                    var patient = _scheduleDao.GetPatientName((int)infor.IdPatient);

                    return new InforScheduleModel
                    {
                        ID = infor.ID,
                        IdPatient = infor.IdPatient,
                        IdVaccine = infor.IdVaccine,
                        Quantity = infor.Quantity,
                        Status = infor.Status,
                        CreateAt = infor.CreateAt,
                        Time = infor.Time,
                        NameVaccine = vaccine != null ? vaccine.NameVaccine : string.Empty,
                        NamePatient = patient != null ? patient.Name : string.Empty
                    };
                }),
                page,
                pageSize,
                pagedModel.TotalItemCount
            );
            ViewBag.SearchString = searchString;
            return View(model);
        }

    }
}