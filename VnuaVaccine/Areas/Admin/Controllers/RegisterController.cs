using DAL.Dao;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnuaVaccine.Areas.Admin.Models;
using VnuaVaccine.Common;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Admin/Register
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(ProfileModel registerModel)
        {

            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            var userDao = new UserDAO();
            var result = userDao.RegisterCheck(registerModel.Email);
           
            if (registerModel.ConfirmPassword != registerModel.Password)
            {
                ModelState.AddModelError("", "Xác nhận mật khẩu phải trùng mới mật khẩu!");
                return View("Index");
            }

            switch (result)
            {               
                case 0:
                    ModelState.AddModelError("", "Email đã tồn tại!");
                    break;
                case 1:
                    var newUser = new User
                    {
                        UserName = registerModel.UserName,
                        Email = registerModel.Email,
                        Password = Encryptor.MD5Hash(registerModel.Password),
                        CreateAt = DateTime.Now,
                        Role = 1,
                        Status = 1,
                    };
                    userDao.Insert(newUser);

                    var patientDao = new PatientDAO();
                    var newPatient = new Patient
                    {
                        IdUserName = newUser.ID,
                        NameParent = newUser.UserName,
                        CreateAt = DateTime.Now,
                    };
                    patientDao.Insert(newPatient);

                    TempData["SuccessMessage"] = "Đăng ký thành công";
                    return RedirectToAction("Index", "Login");
            }
            return View("Index", registerModel);
        }
    }
}