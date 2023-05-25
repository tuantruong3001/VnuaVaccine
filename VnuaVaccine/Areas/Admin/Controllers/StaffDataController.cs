using DAL.Dao;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnuaVaccine.Areas.Admin.Models;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class StaffDataController : Controller
    {
        // GET: Admin/Staff
        private readonly StaffDAO _staffDao = new StaffDAO();

        // GET: Admin/PatientData
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var model = _staffDao.ListPatientPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(InforStaffModel createModel, int isSex)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }
            try
            {
                var patientDao = new StaffDAO();
                var patient = new MedicalStaff
                {
                    Name = createModel.Name,
                    Sex = isSex,
                    Address = createModel.Address,
                    PhoneNumber = (int)createModel.PhoneNumber,
                    Birthday = createModel.Birthday,
                    CreateAt = DateTime.Now,
                };
                patientDao.Insert(patient);
                TempData["UserMessage"] = "Thêm mới thông tin nhân viên thành công";
                return RedirectToAction("Index", "StaffData");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Đã có lỗi xảy ra, vui lòng thử lại sau: {ex.Message}");
                return View(createModel);
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var patientDao = new StaffDAO();
            var patient = patientDao.GetByID(id);

            var patientModel = new InforStaffModel
            {
                Name = patient.Name,
                Sex = patient.Sex,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber,
                Birthday = patient.Birthday,
            };
            ViewBag.SexOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Nam", Selected = patientModel?.Sex == 1 },
                new SelectListItem { Value = "0", Text = "Nữ", Selected = patientModel?.Sex == 0 },
            };
            return View(patientModel);
        }
        [HttpPost]
        public ActionResult Edit(InforStaffModel patientModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var patientDao = new StaffDAO();
                    var patient = new MedicalStaff
                    {
                        ID = (int)patientModel.ID,
                        Name = patientModel.Name,
                        Sex = (int)patientModel.Sex,
                        Address = patientModel.Address,
                        PhoneNumber = (int)patientModel.PhoneNumber,
                        Birthday = patientModel.Birthday,
                        UpdateAt = DateTime.Now,
                    };
                    ViewBag.SexOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "1", Text = "Nam", Selected = patientModel?.Sex == 1 },
                        new SelectListItem { Value = "0", Text = "Nữ", Selected = patientModel?.Sex == 0 },
                    };

                    patientDao.Update(patient);
                    TempData["EditUserMessage"] = "Sửa thông tin nhân viên thành công";
                    return RedirectToAction("Index", "StaffData");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Đã có lỗi xảy ra, vui lòng thử lại sau: {ex.Message}");
                    return View(patientModel);
                }
            }
            return View(patientModel);
        }
        [HttpGet]
        public ActionResult Detail(int id)
        {
            try
            {
                var patientDao = new StaffDAO();
                var patient = patientDao.GetByID(id);

                //list infor patient
                var db = new VaccineDbContext();
                var profileModel = new InforStaffModel();

                if (patient.IdUserName != null)
                {
                    profileModel = db.MedicalStaffs
                        .Where(getPatient => getPatient.IdUserName == patient.IdUserName)
                        .Join(db.Users, getPatient => getPatient.IdUserName, getStaff => getStaff.ID, (getPatient, getStaff) =>
                        new InforStaffModel
                        {
                            ID = getPatient.ID,
                            Name = getPatient.Name,
                            Sex = getPatient.Sex,
                            Address = getPatient.Address,
                            Birthday = getPatient.Birthday,
                            Age = getPatient.Age,
                            PhoneNumber = getPatient.PhoneNumber,
                            CreateAt = getPatient.CreateAt,
                            UpdateAt = getPatient.UpdateAt,
                            Email = getStaff.Email
                        })
                        .SingleOrDefault();
                }
                else
                {
                    profileModel = db.MedicalStaffs
                        .Where(getPatient => getPatient.ID == id)
                        .Select(getPatient => new InforStaffModel
                        {
                            ID = getPatient.ID,
                            Name = getPatient.Name,
                            Sex = getPatient.Sex,
                            Address = getPatient.Address,
                            Birthday = getPatient.Birthday,
                            Age = getPatient.Age,
                            PhoneNumber = getPatient.PhoneNumber,
                            CreateAt = getPatient.CreateAt,
                            UpdateAt = getPatient.UpdateAt,
                            Email = ""
                        })
                        .SingleOrDefault();
                }

                return View(profileModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return View();
            }
        }
    }
}