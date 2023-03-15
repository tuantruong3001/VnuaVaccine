using DAL.EF;
using DAL.Dao;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using VnuaVaccine.Areas.Admin.Models;
using VnuaVaccine.Common;
using System.Web.Security;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            var userDao = new UserDao();
            var loginResult = userDao.Login(loginModel.Password, loginModel.Email);
            
            switch (loginResult)
            {
                case 1:
                    var user = userDao.GetByEmail(loginModel.Email);
                    var userSession = new UserLogin
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        UserID = user.ID
                    };
                   /* FormsAuthentication.RedirectFromLoginPage(loginModel.UserName, false);*/
                    if (user.Role == 0)
                    {
                        return RedirectToAction("Index", "HomeAdmin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "HomeUser");
                    }

                case 0:
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                    break;

                case -1:
                    ModelState.AddModelError("", "Tài khoản bị xoá!");
                    break;

                case -2:
                    ModelState.AddModelError("", "Mật khẩu không đúng!");
                    break;

                default:
                    ModelState.AddModelError("", "Thông tin đăng nhập không đúng!");
                    break;
            }
            return View("Index");
        }
    }
}