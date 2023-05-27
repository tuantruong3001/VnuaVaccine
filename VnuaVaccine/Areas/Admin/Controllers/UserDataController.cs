using DAL.Dao;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Profile;
using VnuaVaccine.Areas.Admin.Models;
using VnuaVaccine.Common;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class UserDataController : BaseController
    {
        private readonly UserDAO _userDAO = new UserDAO();
        // GET: Admin/UserData
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var model = _userDAO.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var userDao = new UserDAO();
            var user = userDao.GetById(id);

            var userModel = new ProfileModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
            };
            return View(userModel);
        }

        [HttpPost]
        public ActionResult Edit(ProfileModel userModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userDao = new UserDAO();
                    var user = new User
                    {
                        ID = userModel.ID,
                        UserName = userModel.UserName,
                        Email = userModel.Email,
                        Role = userModel.Role,
                        Password = Encryptor.MD5Hash(userModel.Password),
                        UpdateAt = DateTime.Now,
                    };
                    userDao.Update(user);
                    TempData["EditUserMessage"] = "Sửa thông tin thành công";
                    return RedirectToAction("Index", "UserData");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Đã có lỗi xảy ra, vui lòng thử lại sau: {ex.Message}");
                    return View(userModel);
                }
            }
            return View(userModel);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                UserDAO userDao = new UserDAO();
                PatientDAO patientDao = new PatientDAO();

                patientDao.DeleteByUserId(id);

                bool success = userDao.Delete(id);
                if (success)
                {
                    TempData["DeleteUserMessage"] = "Xoá thành công";
                }
                else
                {
                    TempData["DeleteUserMessage"] = "Xoá không thành công";
                }
                return RedirectToAction("Index", "UserData");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Đã có lỗi xảy ra, vui lòng thử lại sau!");
                return View("Index");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProfileModel registerModel)
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
                        Name = newUser.UserName,
                        CreateAt = DateTime.Now,
                    };
                    patientDao.Insert(newPatient);

                    TempData["SuccessMessage"] = "Đăng ký thành công";
                    return RedirectToAction("Index", "UserData");
            }
            return View("Index");
        }
    }
}