﻿using MVC.Samples.Web.Areas.Guru.Models;
using MVC.Samples.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Samples.Web.Areas.Guru.Controllers
{
    public class HomeController : Controller
    {
        // GET: Guru/Home
        [HttpGet]
        public ActionResult Index()
        {
            GuruModel guruModel = new GuruModel();
            try
            {
                return View(guruModel);
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message.ToString();
                return View("Error");
            }
            finally
            {
                guruModel = null;
            }
        }


        [HttpPost]
        [Authorize]
        public ActionResult Index(GuruModel guruModel)
        {
            //UserSessionHandler.AddUserSession(guruModel);

            Session["UserId"] = guruModel.Name;
            Session["Password"] = guruModel.Password;
            Session["EmployeeId"] = guruModel.EmployeeCode;
            Session["Role"] = guruModel.EmployeeCode;
            ViewBag.SuccessLogin = "Logged in Successfully";
            return RedirectToAction("Contact", "Home", new { area = "" });
            //DB Store the data.
        }

        private List<SelectListItem> GetCityList()
        {
            List<SelectListItem> cityList = new List<SelectListItem>();
            cityList.Add(new SelectListItem() { Text = "Tirunelveli", Value = "TEN" });
            cityList.Add(new SelectListItem() { Text = "Tuticorin", Value = "TUT" });
            cityList.Add(new SelectListItem() { Text = "Theni", Value = "THE" });
            return cityList;
        }

        private List<SelectListItem> GetIntrestedList()
        {
            List<SelectListItem> intrest = new List<SelectListItem>();
            intrest.Add(new SelectListItem() { Text = "DotNet", Value = "DOT" });
            intrest.Add(new SelectListItem() { Text = "React", Value = "REC" });
            intrest.Add(new SelectListItem() { Text = "Angulur", Value = "ANG" });
            return intrest;
        }
    }
}