using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VnuaVaccine
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //handle routing if id vaccine detail= null
            routes.MapRoute(
              name: "Product Detail",
              url: "ListVaccine/Detail",
              defaults: new { controller = "ListVaccine", action = "Index", id = UrlParameter.Optional },
             namespaces: new[] { "VnuaVaccine.Controllers" }

              );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
