﻿using DAL.Dao;
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
    public class HomeUserController : BaseController
    {
        // GET: Admin/HomeUser
        public ActionResult Index()
        {
            var userDao = new UserDAO();
            var model = (UserLogin)Session[SessionConstants.USER_SESSION];
            var user = userDao.GetById(model.UserID);

            var patientDao = new PatientDAO();
            var patient = patientDao.GetByUserName(user.UserName);

            var profileModel = new ProfileModel
            {
                ID = user.ID,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Age = patient.Age,
                Address = patient.Address
            };

            return View(profileModel);
        }

        [HttpPost]
        public ActionResult Index(ProfileModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userDao = new UserDAO();
                    var patientDao = new PatientDAO();
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
                       
                        var patient = new Patient
                        {
                            IdUserName = user.ID,
                            Age = model.Age,
                            Address = model.Address
                        };
                        patientDao.Update(patient);

                        TempData["EditUserMessage"] = "Sửa thông tin thành công";
                        return RedirectToAction("Index", "HomeUser");
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