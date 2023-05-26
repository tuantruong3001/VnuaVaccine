using System.Web.Mvc;

namespace VnuaVaccine.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Page404()
        {
            Response.StatusCode = 404;
            return View("Page404");
        }
    }
}