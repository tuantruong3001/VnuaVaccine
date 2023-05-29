using DAL.Dao;
using PagedList;
using System.Collections.Generic;
using System.Web.Mvc;
using VnuaVaccine.Areas.Admin.Models;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class WishlistController : BaseController
    {
        // GET: Admin/Wishlist
        private readonly ScheduleDAO _scheduleDao = new ScheduleDAO();
      
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var session = (Common.UserLogin)Session[Common.SessionConstants.USER_SESSION];
            int loggedInUserId = session.UserID;

            var userNameId = _scheduleDao.GetUserNameId(loggedInUserId);         
            var patientSchedule = _scheduleDao.GetPatientSchedule(userNameId);

            var patientScheduleInfoList = new List<InforScheduleModel>();

            foreach (var schedule in patientSchedule)
            {
                var vaccine = _scheduleDao.GetVaccineName((int)schedule.IdVaccine);
                var patient = _scheduleDao.GetPatientName((int)schedule.IdPatient);

                var scheduleInfo = new InforScheduleModel
                {
                    ID = schedule.ID,
                    IdPatient = schedule.IdPatient,
                    IdVaccine = schedule.IdVaccine,
                    Quantity = schedule.Quantity,
                    Status = schedule.Status,
                    Times = vaccine.Times,
                    CreateAt = schedule.CreateAt,
                    Time = schedule.Time,
                    NameVaccine = vaccine != null ? vaccine.NameVaccine : string.Empty,
                    NamePatient = patient != null ? patient.Name : string.Empty
                };

                patientScheduleInfoList.Add(scheduleInfo);
            }

            var pagedModel = new StaticPagedList<InforScheduleModel>(
                patientScheduleInfoList.ToPagedList(page, pageSize),
                page,
                pageSize,
                patientScheduleInfoList.Count
            );

            ViewBag.SearchString = searchString;
            return View(pagedModel);
        }

    }
}