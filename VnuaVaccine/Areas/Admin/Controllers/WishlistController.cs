using DAL.Dao;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            var pagedModel = _scheduleDao.ListWishlist(searchString, page, pageSize);
            var model = new StaticPagedList<InforScheduleModel>(
                pagedModel.Select(infor =>
                {
                    var vaccine = _scheduleDao.GetVaccineName((int)infor.IdVaccine);
                    var patient = _scheduleDao.GetPatientName((int)infor.IdPatient);

                    return new InforScheduleModel
                    {
                        ID = infor.ID,
                        IdPatient = (int)infor.IdPatient,
                        IdVaccine = (int)infor.IdVaccine,
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