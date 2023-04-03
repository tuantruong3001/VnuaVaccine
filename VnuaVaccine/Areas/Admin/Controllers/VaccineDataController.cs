using System;
using DAL.Dao;
using DAL.EF;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class VaccineDataController : BaseController
    {
        // GET: Admin/VaccineData
        private readonly VaccineDAO _productDao = new VaccineDAO();

        public ActionResult Index(string searchString, int page = 1, int pageSize = 15)
        {
            var model = _productDao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
    }
}