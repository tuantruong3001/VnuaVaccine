﻿using DAL.Dao;
using DAL.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc;
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
                    var vaccine = _scheduleDao.GetVaccine((int)infor.IdVaccine);
                    var patient = _scheduleDao.GetPatient((int)infor.IdPatient);

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
                    var vaccine = _scheduleDao.GetVaccine((int)infor.IdVaccine);
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

            var vaccine = _scheduleDao.GetVaccine((int)schedule.IdVaccine);
            var patient = _scheduleDao.GetPatient((int)schedule.IdPatient);

            var user = _scheduleDao.GetUserEmailByPatientId((int)schedule.IdPatient);

            var vaccineModel = new InforScheduleModel
            {
                ID = id,
                IdPatient = schedule.IdPatient,
                IdVaccine = schedule.IdVaccine,
                Status = schedule.Status,
                Time = schedule.Time,
                Times = vaccine.Times,
                CreateAt = schedule.CreateAt,
                Quantity = 1,
                QuantityOld = schedule.Quantity,
                NameVaccine = vaccine != null ? vaccine.NameVaccine : string.Empty,
                NamePatient = patient != null ? patient.Name : string.Empty,
                Email = user ?? string.Empty,
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

                var existingSchedules = _scheduleDao.GetScheduleByPatientAndVaccine((int)schedule.IdPatient, (int)schedule.IdVaccine);
                int? scheduleQuantity = model.Quantity;
                int totalQuantity = 0;

                if (existingSchedules != null && existingSchedules.Count > 0)
                {
                    int maxExistingQuantity = existingSchedules.Max(s => (int)s.Quantity);
                    totalQuantity = (int)(maxExistingQuantity + scheduleQuantity);
                }

                schedule.Quantity = totalQuantity;

                ViewBag.SexOptions = new List<SelectListItem>
                  {
                      new SelectListItem { Value = "0", Text = "Chưa tiêm", Selected = model.Status == 0 },
                      new SelectListItem { Value = "1", Text = "Chưa tiêm đủ số mũi", Selected = model.Status == 1 },
                      new SelectListItem { Value = "2", Text = "Đã tiêm đủ số mũi", Selected = model.Status == 2 },
                      new SelectListItem { Value = "3", Text = "Đã tiêm liều tăng cường", Selected = model.Status == 3 },
                  };

                schedule.Status = model.Status;
                schedule.Time = model.Time;

                vaccineDao.Update(schedule);
                TempData["EditUserMessage"] = "Cập nhật thông tin thành công";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult CreatePatient()
        {
            var patientDao = new PatientDAO();
            var patients = patientDao.GetAll();

            var vaccineDao = new VaccineDAO();
            var vaccines = vaccineDao.GetAll();

            var viewModel = new CreateScheduleViewModel
            {
                PatientList = patients.Select(p => new SelectListItem
                {
                    Value = p.ID.ToString(),
                    Text = p.Name
                }),
                VaccineList = vaccines.Select(v => new SelectListItem
                {
                    Value = v.ID.ToString(),
                    Text = v.NameVaccine
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreatePatient(CreateScheduleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var vaccineDao = new ScheduleDAO();

                var existingSchedules = vaccineDao.GetScheduleByPatientAndVaccine(viewModel.SelectedPatientId, (int)viewModel.ScheduleModel.IdVaccine);
                var timeVaccine = vaccineDao.GetVaccine((int)viewModel.ScheduleModel.IdVaccine);
                int totalQuantity = 0;

                if (existingSchedules != null && existingSchedules.Count > 0)
                {
                    int maxExistingQuantity = existingSchedules.Max(s => (int)s.Quantity);
                    totalQuantity = maxExistingQuantity;
                }

                var schedule = new VaccinationSchedule
                {
                    IdPatient = viewModel.SelectedPatientId,
                    IdVaccine = viewModel.ScheduleModel.IdVaccine,
                    Time = viewModel.ScheduleModel.Time,
                    CreateAt = DateTime.Now,
                    Quantity = totalQuantity,
                    Status = 0,

                };

                vaccineDao.Insert(schedule);
                TempData["EditUserMessage"] = "Thêm mới lịch tiêm thành công";
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CheckVaccine(CreateScheduleViewModel viewModel)
        {
            var vaccineDao = new ScheduleDAO();

            var existingSchedules = vaccineDao.GetScheduleByPatientAndVaccine(viewModel.SelectedPatientId, (int)viewModel.ScheduleModel.IdVaccine);
            var timeVaccine = vaccineDao.GetVaccine((int)viewModel.ScheduleModel.IdVaccine);
            int totalQuantity = 0;

            if (existingSchedules != null && existingSchedules.Count > 0)
            {
                int maxExistingQuantity = existingSchedules.Max(s => (int)s.Quantity);
                totalQuantity = maxExistingQuantity;
            }

            if (timeVaccine.Times == totalQuantity || timeVaccine.Times < totalQuantity)
            {
                TempData["ConfirmationMessage"] = "Người này đã tiêm đủ liều vắc xin!";
            }

            return Json(new { confirmationMessage = TempData["ConfirmationMessage"] });
        }

        [HttpPost]
        public ActionResult Search(string name)
        {
            List<Patient> result = _scheduleDao.SearchByName(name);

            List<SelectListItem> dropdownList = result.Select(p => new SelectListItem
            {
                Value = p.ID.ToString(),
                Text = p.Name
            }).ToList();

            return Json(dropdownList);
        }
        [HttpPost]
        public ActionResult SearchVaccine(string name)
        {
            List<Vaccine> result = _scheduleDao.SearchByNameVaccine(name);

            List<SelectListItem> dropdownList = result.Select(p => new SelectListItem
            {
                Value = p.ID.ToString(),
                Text = p.NameVaccine
            }).ToList();

            return Json(dropdownList);
        }
        public ActionResult Detail(int id)
        {

            var vaccineDao = new ScheduleDAO();
            var schedule = vaccineDao.GetByID(id);

            var vaccine = _scheduleDao.GetVaccine((int)schedule.IdVaccine);
            var patient = _scheduleDao.GetPatient((int)schedule.IdPatient);

            var user = _scheduleDao.GetUserEmailByPatientId((int)schedule.IdPatient);

            var vaccineModel = new InforScheduleModel
            {
                ID = id,
                IdPatient = schedule.IdPatient,
                IdVaccine = schedule.IdVaccine,
                Status = schedule.Status,
                Time = schedule.Time,
                Times = vaccine.Times,
                CreateAt = schedule.CreateAt,
                Quantity = 1,
                QuantityOld = schedule.Quantity,
                NameVaccine = vaccine != null ? vaccine.NameVaccine : string.Empty,
                NamePatient = patient != null ? patient.Name : string.Empty,
                NameParent = patient != null ? patient.NameParent : string.Empty,
                Email = user ?? string.Empty,
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
        public ActionResult Detail(InforScheduleModel model)
        {

            string email = model.Email;
            string statusText = string.Empty;
            switch (model.Status)
            {
                case 0:
                    statusText = "Chưa tiêm";
                    break;
                case 1:
                    statusText = "Chưa tiêm đủ số mũi";
                    break;
                case 2:
                    statusText = "Đã tiêm đủ số mũi";
                    break;
                case 3:
                    statusText = "Đã tiêm liều tăng cường";
                    break;
            }
            if (string.IsNullOrEmpty(email))
            {

                return RedirectToAction("Index", "VaccineSchedule");
            }
            try
            {              
                MailMessage message = new MailMessage();
                message.From = new MailAddress("tuantruongvan3001@gmail.com");
                message.To.Add(email);
                message.Subject = "Thông tin lịch tiêm chủng";
                message.Body = $"Chúng tôi gửi thư này để cung cấp thông tin về lịch trình tiêm chủng của con bạn. Dưới đây là chi tiết về lịch trình tiêm chủng: \n\n" +
                               $"ID: {model.ID} \n" +
                               $"Họ tên bệnh nhân: {model.NamePatient} \n" +
                               $"Tên cha/mẹ bệnh nhân: {model.NameParent} \n" +
                               $"Vắc-xin: {model.NameVaccine} \n" +
                               $"Thời gian tạo lịch tiêm: {model.CreateAt} \n" +
                               $"Thời gian tiêm: {model.Time} \n" +
                               $"Trạng thái: {statusText} \n\n" +
                               $"Thông tin trên đã được xác nhận và đặt lịch trình tiêm chủng cho con bạn. Vui lòng đến địa điểm tiêm chủng vào thời gian đã được chỉ định. \nNếu có bất kỳ thay đổi nào về lịch trình, chúng tôi sẽ thông báo cho bạn kịp thời.";

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("tuantruongvan3001@gmail.com", "kjpjpfbvzovvlhnd");
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);

                TempData["SendEmail"] = "Gửi email thành công!";
                return RedirectToAction("Index", "VaccineSchedule");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "VaccineSchedule");
            }
        }
    }
}