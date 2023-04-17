using System;
using DAL.Dao;
using DAL.EF;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using VnuaVaccine.Areas.Admin.Models;

namespace VnuaVaccine.Areas.Admin.Controllers
{
    public class VaccineDataController : BaseController
    {
        // GET: Admin/VaccineData
        private readonly VaccineDAO _productDao = new VaccineDAO();

        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var model = _productDao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        { 
            return View();
        }
        [HttpPost]
        public ActionResult Create(VaccineModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }
            try
            {
                var vaccineDao = new VaccineDAO();
                var vaccine = new Vaccine
                {
                    NameVaccine = createModel.NameVaccine,
                    Munafacturer = createModel.Munafacturer,
                    Description = createModel.Description,
                    QuantityStock = createModel.QuantityStock,
                    Path = createModel.Path,
                    ProductionDate = createModel.ProductionDate,
                    ExpirationData = createModel.ExpirationData,
                };
                vaccineDao.Insert(vaccine);
                TempData["UserMessage"] = "Thêm mới thông tin Vaccine thành công";
                return RedirectToAction("Index", "VaccineData");
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
            var vaccineDao = new VaccineDAO();
            var vaccine = vaccineDao.GetByID(id);

            var vaccineModel = new VaccineModel
            {
                NameVaccine = vaccine.NameVaccine,
                Munafacturer = vaccine.Munafacturer,
                Description = vaccine.Description,
                QuantityStock = vaccine.QuantityStock,
                Path = vaccine.Path,
                ProductionDate = vaccine.ProductionDate,
                ExpirationData = vaccine.ExpirationData,
                Note = vaccine.Note,
            };
            return View(vaccineModel);
        }
        [HttpPost]
        public ActionResult Edit(VaccineModel vaccineModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var vaccineDao = new VaccineDAO();
                    var vaccine = new Vaccine
                    {
                        ID = vaccineModel.Id,
                        NameVaccine = vaccineModel.NameVaccine,
                        Munafacturer = vaccineModel.Munafacturer,
                        Description = vaccineModel.Description,
                        QuantityStock = vaccineModel.QuantityStock,
                        Path = vaccineModel.Path,
                        ProductionDate = vaccineModel.ProductionDate,
                        ExpirationData = vaccineModel.ExpirationData,
                        Note = vaccineModel.Note,
                    };
                    vaccineDao.Update(vaccine);
                    TempData["EditUserMessage"] = "Sửa thông tin Vaccine thành công";
                    return RedirectToAction("Index", "VaccineData");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Đã có lỗi xảy ra, vui lòng thử lại sau: {ex.Message}");
                    return View(vaccineModel);
                }
            }
            return View(vaccineModel);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                VaccineDAO vaccineDao = new VaccineDAO();
                bool success = vaccineDao.Delete(id);
                if (success)
                {
                    TempData["DeleteUserMessage"] = "Xoá thành công";
                }
                else
                {
                    TempData["DeleteUserMessage"] = "Xoá không thành công";
                }
                return RedirectToAction("Index", "VaccineData");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Đã có lỗi xảy ra, vui lòng thử lại sau!");
                return View("Index");
            }
        }
    }
}