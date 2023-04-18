using DAL.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class PatientDataController : Controller
    {
        private readonly PatientDAO _patientDao = new PatientDAO();

        // GET: Admin/PatientData
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var model = _patientDao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
    }
}