using DAL.Dao;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnuaVaccine.Areas.Admin.Models;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Admin/Register
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register(ProfileModel registerModel)
        {

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var userDao = new UserDAO();
            var result = userDao.RegisterCheck(registerModel.Email, registerModel.UserName);
           
            if (registerModel.ConfirmPassword != registerModel.Password)
            {
                ModelState.AddModelError("", "ConfirmPassword phải trùng với Password");
                return View("Index");
            }

            switch (result)
            {
                case -1:
                    ModelState.AddModelError("", "UserName đã tồn tại!");
                    break;
                case 0:
                    ModelState.AddModelError("", "Email đã tồn tại!");
                    break;
                case 1:
                    var newUser = new User
                    {
                        UserName = registerModel.UserName,
                        Email = registerModel.Email,
                        Password = registerModel.Password,
                        Role = 1,
                        Status = 1,
                    };
                    userDao.Insert(newUser);

                    var patientDao = new PatientDAO();
                    var newPatient = new Patient
                    {
                        IdUserName = newUser.ID,
                        CreateAt = DateTime.Now,
                    };
                    patientDao.Insert(newPatient);

                    TempData["SuccessMessage"] = "Đăng ký thành công";
                    return RedirectToAction("Index", "Login");
            }
            return View("Index");
        }
    }
}