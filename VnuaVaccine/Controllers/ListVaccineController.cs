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
        public ActionResult Index(string searchString, int page = 1, int pageSize = 9)
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
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public ActionResult Detail(int id)
        {
            try
            {
                var dao = new VaccineDAO();
                var product = dao.ViewDetail(id);
                return View(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}