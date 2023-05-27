using DAL.Dao;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnuaVaccine.Common;
using VnuaVaccine.Models;

namespace VnuaVaccine.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }

            return View(list);
        }

        public ActionResult AddToCart(int idVaccine, int quantity)
        {
            #region check login

            var userDao = new UserDAO();
            var userLogin = (UserLogin)Session[SessionConstants.USER_SESSION];
            if (userLogin == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }

            var user = userDao.GetById(userLogin.UserID);
            if (user == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }
            #endregion
            #region addtocart
            try
            {
                var vaccine = new VaccineDAO().ViewDetail(idVaccine);
                var cart = Session[CartSession];
                if (cart != null)
                {
                    var list = (List<CartItem>)cart;
                    if (list.Exists(x => x.Vaccine.ID == idVaccine))
                    {
                        foreach (var item in list)
                        {
                            if (item.Vaccine.ID == idVaccine)
                            {
                                item.Quantity += quantity;
                            }
                        }
                    }
                    else
                    {
                        var item = new CartItem();
                        item.IdPatient = user.ID;
                        item.Vaccine = vaccine;
                        item.Quantity = quantity;
                        item.Status = 0;
                        item.CreateAt = DateTime.Now;
                        list.Add(item);
                    }
                    Session[CartSession] = list;
                }
                else
                {
                    var item = new CartItem();
                    item.IdPatient = user.ID;
                    item.Vaccine = vaccine;
                    item.Quantity = quantity;
                    item.CreateAt = DateTime.Now;
                    item.Status = 0;
                    var list = new List<CartItem>();
                    list.Add(item);
                    Session[CartSession] = list;
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Đã có lỗi xảy ra, vui lòng thử lại sau!");
                return View("Index");
            }
            #endregion
        }

        public ActionResult RemoveFromCart(int id)
        {
            try
            {
                var cart = (List<CartItem>)Session[CartSession];
                if (cart != null)
                {
                    var item = cart.FirstOrDefault(i => i.ID == id);
                    if (item != null)
                    {
                        cart.Remove(item);
                    }
                }
                Session[CartSession] = cart;
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Đã có lỗi xảy ra, vui lòng thử lại sau!");
                return View("Index");
            }

        }

        public ActionResult SaveSchedule()
        {
            try
            {
                var cart = (List<CartItem>)Session[CartSession];
                if (cart != null && cart.Count > 0)
                {
                    var vaccineScheduleDao = new ScheduleDAO();

                    foreach (var cartItem in cart)
                    {
                        var schedule = new VaccinationSchedule
                        {
                            Quantity = cartItem.Quantity,
                            IdPatient = cartItem.IdPatient,
                            IdVaccine = cartItem.Vaccine.ID,
                            Status = 0,
                            CreateAt = DateTime.Now,
                        };
                        vaccineScheduleDao.Insert(schedule);
                    }

                    Session[CartSession] = null;
                }
                else
                {
                    ModelState.AddModelError("", "Giỏ hàng trống. Vui lòng thêm mục hàng trước khi lưu lịch tiêm chủng.");
                    return View("Index");
                }

                return RedirectToAction("SaveSuccess");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Đã có lỗi xảy ra, vui lòng thử lại sau!");
                return View("Index");
            }
        }

        public ActionResult SaveSuccess()
        {
            return View();
        }
    }
}