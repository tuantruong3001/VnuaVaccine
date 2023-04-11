﻿using DAL.EF;
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
using System.Security.Principal;
using System.Web.UI.WebControls;

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
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index");
                }

                var userDao = new UserDAO();
                var loginResult = userDao.Login(loginModel.Password, loginModel.Email);

                switch (loginResult)
                {
                    case 1:
                        var user = userDao.GetByEmail(loginModel.Email);
                        var userSession = new UserLogin
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            UserID = user.ID,
                            RoleId = user.Role
                        };
                        Session[SessionConstants.USER_SESSION] = userSession;
                        // redict by role
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
            catch (Exception ex)
            {
                // handle the exception here
                ViewBag.ErrorMessage = "Đã có lỗi xảy ra, vui lòng thử lại sau " + ex.Message;
                return RedirectToAction("Login", "Login");
               // return View("Index");
            }
        }
        public ActionResult Logout()
        {
            try
            {
                // Xóa session hiện tại
                Session.Clear();
                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã có lỗi xảy ra, vui lòng thử lại sau " + ex.Message;
                return View("Index");
            }
        }      

    }
}