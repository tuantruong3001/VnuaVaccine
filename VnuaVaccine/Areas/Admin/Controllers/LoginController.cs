using DAL.Dao;
using System;
using System.Web.Mvc;
using VnuaVaccine.Areas.Admin.Models;
using VnuaVaccine.Common;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {          
            return View("Index");
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
                var loginResult = userDao.Login(Encryptor.MD5Hash(loginModel.Password), loginModel.Email);

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
                            return RedirectToAction("Index", "PatientData");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home", new { area = "" });
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
                ViewBag.ErrorMessage = "Đã có lỗi xảy ra, vui lòng thử lại sau " + ex.Message;
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult Logout()
        {
            try
            {
                Session?.Clear();
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