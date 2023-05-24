using DAL.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VnuaVaccine.Controllers
{
    public class ListVaccineController : Controller
    {
        // GET: ListVaccine
        public ActionResult Index(string searchString, int page = 1, int pageSize = 6)
        {
            try
            {
                var productDao = new VaccineDAO();
                var model = productDao.ListAllPaging(searchString, page, pageSize);

                ViewBag.SearchString = searchString;
                return View(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ActionResult Detail(int id)
        {
            try
            {
                var vaccineDAO = new VaccineDAO();
                var vaccine = vaccineDAO.ViewDetail(id);
                if (vaccine == null)
                {
                    return RedirectToAction("Index");
                }
                return View(vaccine);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}