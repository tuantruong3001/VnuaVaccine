﻿using System.Web.Mvc;
using System.Web.Routing;
using VnuaVaccine.Common;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
       
        // trỏ các trang về trang admin login khi session null
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin)Session[SessionConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new
                    {
                        controller = "Login",
                        action = "Index",
                        Area = "Admin"
                    }));
                base.OnActionExecuting(filterContext);
            }
        }
    }
}