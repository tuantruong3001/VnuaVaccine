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
                //list infor staff
                var db = new VaccineDbContext();
                var profileModel = db.Users
                    .Where(getUser => getUser.ID == user.ID)
                    .Join(db.MedicalStaffs, getUser => getUser.ID, getStaff => getStaff.IdUserName, (getUser, getStaff) => new ProfileModel
                    {
                        ID = getUser.ID,
                        UserName = getUser.UserName,
                        Email = getUser.Email,
                        Password = getUser.Password,
                        Role = getUser.Role,

                        Age = getStaff.Age,
                        Sex = getStaff.Sex,
                        Address = getStaff.Address,
                        Name = getStaff.Name,
                        PhoneNumber = getStaff.PhoneNumber
                    })
                    .SingleOrDefault();
                ViewBag.SexOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Nam", Selected = profileModel?.Sex == 1 },
                    new SelectListItem { Value = "0", Text = "Nữ", Selected = profileModel?.Sex == 0 },
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
                    var staffDao = new StaffDAO();
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

                        var staff = new MedicalStaff
                        {
                            IdUserName = user.ID,
                            Age = model.Age,
                            Sex = model.Sex,
                            Name = model.Name,
                            PhoneNumber = (int)model.PhoneNumber,
                            Address = model.Address
                        };
                        staffDao.Update(staff);

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