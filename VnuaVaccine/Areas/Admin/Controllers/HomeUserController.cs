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
    public class HomeUserController : BaseController
    {
        // GET: Admin/HomeUser
        public ActionResult Index()
        {
            var userDao = new UserDAO();
            var model = (UserLogin)Session[SessionConstants.USER_SESSION];
            var user = userDao.GetById(model.UserID);

            var db = new VaccineDbContext();
            var profileModel = db.Users
                .Where(getUser => getUser.ID == user.ID)
                .Join(db.Patients, getUser => getUser.ID, getPatient => getPatient.IdUserName, (getUser, getPatient) => new ProfileModel
                {
                    ID = getUser.ID,
                    UserName = getUser.UserName,
                    Email = getUser.Email,
                    Password = getUser.Password,
                    Role = getUser.Role,

                    Age = getPatient.Age,
                    Sex = getPatient.Sex ?? 1,
                    Address = getPatient.Address ?? "",
                    Name = getPatient.Name,
                    Birthday = getPatient.Birthday,
                    PhoneNumber = getPatient.PhoneNumber
                })
                .SingleOrDefault();
            ViewBag.SexOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Nam", Selected = profileModel?.Sex == 1 },
                new SelectListItem { Value = "0", Text = "Nữ", Selected = profileModel?.Sex == 0 },
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
                        ViewBag.SexOptions = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "1", Text = "Nam", Selected = model.Sex == 1 },
                            new SelectListItem { Value = "0", Text = "Nữ", Selected = model.Sex == 0 },
                        };
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
                            Sex = model.Sex,
                            Name = model.Name,
                            PhoneNumber = model.PhoneNumber,
                            Birthday = model.Birthday,
                            Address = model.Address
                        };
                        patientDao.UpdateProfile(patient);

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