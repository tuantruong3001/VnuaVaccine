using DAL.Dao;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VnuaVaccine.Areas.Admin.Models;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class PatientDataController : BaseController
    {
        private readonly PatientDAO _patientDao = new PatientDAO();
        private readonly ScheduleDAO _scheduleDao = new ScheduleDAO();

        // GET: Admin/PatientData
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var model = _patientDao.ListPatientPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(PatientModel createModel, int isSex)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }
            try
            {
                var patientDao = new PatientDAO();
                var patient = new Patient
                {
                    Name = createModel.Name,
                    Sex = isSex,
                    NameParent = createModel.NameParent,
                    Address = createModel.Address,
                    PhoneNumber = createModel.PhoneNumber,
                    Birthday = createModel.Birthday,
                    CreateAt = DateTime.Now,
                };
                patientDao.Insert(patient);
                TempData["UserMessage"] = "Thêm mới thông tin bệnh nhân thành công";
                return RedirectToAction("Index", "PatientData");
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
            var patientDao = new PatientDAO();
            var patient = patientDao.GetByID(id);

            var patientModel = new PatientModel
            {
                Name = patient.Name,
                Sex = patient.Sex,
                NameParent = patient.NameParent,
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
        public ActionResult Edit(PatientModel patientModel)
        {

            try
            {
                var patientDao = new PatientDAO();
                var patient = new Patient
                {
                    ID = (int)patientModel.Id,
                    Name = patientModel.Name,
                    NameParent = patientModel.NameParent,
                    Sex = patientModel.Sex,
                    Address = patientModel.Address,
                    PhoneNumber = patientModel.PhoneNumber,
                    Birthday = patientModel.Birthday,
                    UpdateAt = DateTime.Now,
                };
                ViewBag.SexOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "1", Text = "Nam", Selected = patientModel?.Sex == 1 },
                        new SelectListItem { Value = "0", Text = "Nữ", Selected = patientModel?.Sex == 0 },
                    };

                patientDao.Update(patient);
                TempData["EditUserMessage"] = "Sửa thông tin bệnh nhân thành công";
                return RedirectToAction("Index", "PatientData");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Đã có lỗi xảy ra, vui lòng thử lại sau: {ex.Message}");
                return View(patientModel);
            }

        }
        [HttpGet]
        public ActionResult Detail(int id)
        {
            try
            {
                var patientDao = new PatientDAO();
                var patient = patientDao.GetByID(id);

                //list infor patient
                var db = new VaccineDbContext();
                var profileModel = new InforPatientModel();
                var viewModel = new DetailViewModel();

                int loggedInUserId = patient.ID;

                var patientSchedule = _scheduleDao.GetPatientSchedule(loggedInUserId);
                var nameVaccineList = new List<string>();

                foreach (var schedule in patientSchedule)
                {
                    var vaccine = _scheduleDao.GetVaccineName((int)schedule.IdVaccine);
                                       
                    if (vaccine != null)
                    {
                        var vaccineInfo = $"{vaccine.NameVaccine} - Quantity: {schedule.Quantity} - Time: {schedule.Time}";
                        nameVaccineList.Add(vaccineInfo);
                    }

                }
                if (patient.IdUserName != null)
                {
                    profileModel = db.Patients
                      .Where(getPatient => getPatient.IdUserName == patient.IdUserName)

                      .Join(db.Users, getPatient => getPatient.IdUserName, getStaff => getStaff.ID, (getPatient, getStaff) =>
                      new InforPatientModel
                      {
                          ID = getPatient.ID,
                          Name = getPatient.Name,
                          NameParent = getPatient.NameParent,
                          Sex = getPatient.Sex,
                          Address = getPatient.Address,
                          Birthday = getPatient.Birthday,
                          Age = getPatient.Age,
                          AgeMonth = getPatient.AgeMonth,
                          PhoneNumber = getPatient.PhoneNumber,
                          CreateAt = getPatient.CreateAt,
                          UpdateAt = getPatient.UpdateAt,
                          Email = getStaff.Email,
                          
                      })
                      .SingleOrDefault();
                }
                else
                {
                    profileModel = db.Patients
                        .Where(getPatient => getPatient.ID == id)
                        .Select(getPatient => new InforPatientModel
                        {
                            ID = getPatient.ID,
                            Name = getPatient.Name,
                            NameParent = getPatient.NameParent,
                            Sex = getPatient.Sex,
                            Address = getPatient.Address,
                            Birthday = getPatient.Birthday,
                            Age = getPatient.Age,
                            AgeMonth = getPatient.AgeMonth,
                            PhoneNumber = getPatient.PhoneNumber,
                            CreateAt = getPatient.CreateAt,
                            UpdateAt = getPatient.UpdateAt,
                            Email = "",                           
                        })
                        .SingleOrDefault();
                }
                viewModel.ProfileModel = profileModel; 
                viewModel.NameVaccineList = nameVaccineList; 

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return View();
            }
        }
        public ActionResult UpdateAll()
        {
            try
            {
                var allPatients = _patientDao.GetAllPatients();

                _patientDao.UpdateAllPatients(allPatients);

                TempData["UpdateAllMessage"] = "Cập nhật thông tin tất cả bệnh nhi thành công";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã có lỗi xảy ra, vui lòng thử lại sau: {ex.Message}";

                return RedirectToAction("Index");
            }
        }
    }
}