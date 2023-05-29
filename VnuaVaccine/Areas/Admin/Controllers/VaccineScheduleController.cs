using DAL.Dao;
using DAL.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Profile;
using VnuaVaccine.Areas.Admin.Models;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class VaccineScheduleController : BaseController
    {
        // GET: Admin/VaccineSchedule
        private readonly ScheduleDAO _scheduleDao = new ScheduleDAO();

        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var pagedModel = _scheduleDao.ListAllPaging(searchString, page, pageSize);
            var model = new StaticPagedList<InforScheduleModel>(
                pagedModel.Select(infor =>
                {
                    var vaccine = _scheduleDao.GetVaccineName((int)infor.IdVaccine);
                    var patient = _scheduleDao.GetPatientName((int)infor.IdPatient);

                    return new InforScheduleModel
                    {
                        ID = infor.ID,
                        IdPatient = infor.IdPatient,
                        IdVaccine = infor.IdVaccine,
                        Quantity = infor.Quantity,
                        Status = infor.Status,
                        CreateAt = infor.CreateAt,
                        Time = infor.Time,
                        NameVaccine = vaccine != null ? vaccine.NameVaccine : string.Empty,
                        NamePatient = patient != null ? patient.Name : string.Empty,
                        Times = vaccine.Times

                    };
                }),
                page,
                pageSize,
                pagedModel.TotalItemCount);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        public ActionResult UpdateAll()
        {
            try
            {
                var allSchedules = _scheduleDao.GetAllSchedules();

                foreach (var infor in allSchedules)
                {
                    var vaccine = _scheduleDao.GetVaccineName((int)infor.IdVaccine);
                    var status = infor.Status;

                    if (vaccine != null)
                    {
                        if (infor.Quantity == vaccine.Times)
                        {
                            status = 2;
                        }
                        else if (infor.Quantity > vaccine.Times && infor.Quantity != 0)
                        {
                            status = 3;
                        }
                        else if (infor.Quantity < vaccine.Times && infor.Quantity != 0)
                        {
                            status = 1;
                        }
                        else if (infor.Quantity == 0)
                        {
                            status = 0;
                        }
                    }
                    infor.Status = status;
                }

                _scheduleDao.UpdateAllSchedules(allSchedules);
                TempData["UpdateAllMessage"] = "Cập nhật thông tin tất cả lịch tiêm thành công";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã có lỗi xảy ra, vui lòng thử lại sau: {ex.Message}";

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                ScheduleDAO vaccineDao = new ScheduleDAO();
                bool success = vaccineDao.Delete(id);
                if (success)
                {
                    TempData["DeleteUserMessage"] = "Xoá thành công";
                }
                else
                {
                    TempData["DeleteUserMessage"] = "Xoá không thành công";
                }
                return RedirectToAction("Index", "VaccineSchedule");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Đã có lỗi xảy ra, vui lòng thử lại sau!");
                return View("Index");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var vaccineDao = new ScheduleDAO();
            var schedule = vaccineDao.GetByID(id);

            var vaccine = _scheduleDao.GetVaccineName((int)schedule.IdVaccine);
            var patient = _scheduleDao.GetPatientName((int)schedule.IdPatient);

            var vaccineModel = new InforScheduleModel
            {
                ID = id,
                IdPatient = schedule.IdPatient,
                IdVaccine = schedule.IdVaccine,
                Status = schedule.Status,
                Time = schedule.Time,
                Times = vaccine.Times,
                CreateAt = schedule.CreateAt,
                Quantity = schedule.Quantity,
                NameVaccine = vaccine != null ? vaccine.NameVaccine : string.Empty,
                NamePatient = patient != null ? patient.Name : string.Empty
            };
            ViewBag.SexOptions = new List<SelectListItem>
            {
                    new SelectListItem { Value = "0", Text = "Chưa tiêm", Selected = vaccineModel?.Status == 0 },
                    new SelectListItem { Value = "1", Text = "Chưa tiêm đủ số mũi", Selected = vaccineModel?.Status == 1 },
                    new SelectListItem { Value = "2", Text = "Đã tiêm đủ số mũi", Selected = vaccineModel?.Status == 2 },
                    new SelectListItem { Value = "3", Text = "Đã tiêm liều tăng cường", Selected = vaccineModel?.Status == 3 },
            };
            return View(vaccineModel);
        }
        [HttpPost]
        public ActionResult Edit(InforScheduleModel model)
        {
            if (ModelState.IsValid)
            {
                var vaccineDao = new ScheduleDAO();
                var schedule = vaccineDao.GetByID(model.ID);
                ViewBag.SexOptions = new List<SelectListItem>
                 {
                     new SelectListItem { Value = "0", Text = "Chưa tiêm", Selected = model.Status == 0 },
                     new SelectListItem { Value = "1", Text = "Chưa tiêm đủ số mũi", Selected = model.Status == 1 },
                     new SelectListItem { Value = "2", Text = "Đã tiêm đủ số mũi", Selected = model.Status == 2 },
                     new SelectListItem { Value = "3", Text = "Đã tiêm liều tăng cường", Selected = model.Status == 3 },
                 };

                schedule.Status = model.Status;
                schedule.Time = model.Time;
                schedule.Quantity = model.Quantity;

                vaccineDao.Update(schedule);
                TempData["EditUserMessage"] = "Cập nhật thông tin thành công";
                return RedirectToAction("Index");
            }

            return View(model);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(InforScheduleModel model)
        {
            if (ModelState.IsValid)
            {
            }

            return View(model);
        }

        private bool CheckVaccinationConditions(int patientId, int vaccineId, int times)
        {

            var vaccinationCount = _scheduleDao.GetVaccinationCount(patientId, vaccineId);

            if (vaccinationCount >= times)
            {
                return true; 
            }

            return false; 
        }


    }
}