using DAL.Dao;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnuaVaccine.Areas.Admin.Models;
using VnuaVaccine.Common;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class HomeAdminController : BaseController
    {
        public ActionResult Index()
        {
            try
            {
                var userDao = new UserDAO();
                var userLogin = (UserLogin)Session[SessionConstants.USER_SESSION];
                var user = userDao.GetById(userLogin.UserID);
                var profileModel = new ProfileModel
                {
                    ID = user.ID,
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = user.Password,
                    Role = user.Role,
                   
                };

                return View(profileModel);
            }
            catch (Exception)
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Index(ProfileModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userDao = new UserDAO();
                    var userByEmail = userDao.GetByEmail(model.Email);
                    model.ID = userByEmail.ID;
                    bool isUserNameAvailable;

                    if (model.ConfirmPassword != model.Password)
                    {
                        ModelState.AddModelError("", "ConfirmPassword và Password không khớp");
                        return View("Index");
                    }

                    var userOld = userDao.GetById(model.ID);
                    if (model.UserName == userOld.UserName)
                    {
                        isUserNameAvailable = true;
                    }
                    else
                    {
                        isUserNameAvailable = userDao.CheckUserName(model.UserName);
                    }

                    if (isUserNameAvailable)
                    {
                        var user = new User
                        {
                            ID = model.ID,
                            Email = model.Email,
                            UserName = model.UserName,
                            Password = model.Password,
                            Role = model.Role
                        };
                        userDao.Update(user);
                        TempData["EditUserMessage"] = "Sửa thông tin thành công";
                        return RedirectToAction("Index", "HomeAdmin");
                    }

                    ModelState.AddModelError("", "UserName đã tồn tại");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Đã có lỗi xảy ra, vui lòng thử lại sau: {ex.Message}");
                return View(model);
            }
        }
    }

}