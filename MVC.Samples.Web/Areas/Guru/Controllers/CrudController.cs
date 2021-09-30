﻿using MVC.Samples.BLL.Interfaces.Guru;
using MVC.Samples.BLL.Services.Guru;
using MVC.Samples.Data;
using MVC.Samples.Data.Models;
using MVC.Samples.Web.Helper;
using MVC.Samples.Web.Controllers;
using MVC.Samples.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Samples.Web.Areas.Guru.Controllers
{
    public class CrudController : BaseController
    {
        MyDatabase myDatabase = new MyDatabase();
        private readonly IRegistration registration;
        public CrudController(IRegistration registration)
        {
            //this.registration = new RegistrationService();
            this.registration = registration;
        }

        // GET: Guru/Crud
        public ActionResult Index()
        {
            try
            {
                return View(myDatabase.userRegistrations.ToList());
            }
            catch (Exception ex)
            {
                //Log the errors
                ViewBag.Message = "Sorry, Some error occured while processing Main page.";
                return ErrorView(ex);
            }
        }

        public ActionResult Create()
        {

            if (!Session.IsNewSession) { Session.Clear(); }
            return View();
        }

        [HttpPost]
        public JsonResult Create(UserRegistration objEmp)
        {
            try
            {
                string message = registration.BasicValidations(objEmp);
                if (message != "") { ViewBag.ErrorMessage = message; return Json(new { Status = false, Message = message }); }
                Session["Role"] = objEmp.Role;
                Session["User_Name"] = objEmp.Name;
                registration.SaveUser(objEmp);

                if (objEmp.Role == "Admin")
                {
                    UserSessionHandler.AddRoleSession(new MenuModel()
                    {
                        MenuName = "MasterScreen",
                        Action = "Index",
                        ControllerName = "Crud"
                    });
                    UserSessionHandler.AddRoleSession(new MenuModel()
                    {
                        MenuName = "MyProfile",
                        Action = "Details",
                        ControllerName = "Crud"
                    });

                }
                else if (objEmp.Role == "EndUser")
                {
                    UserSessionHandler.AddRoleSession(new MenuModel()
                    {
                        MenuName = "MyProfile",
                        Action = "Details",
                        ControllerName = "Crud"
                    });
                }
                return Json(new { Status = true, result = "Redirect", url = Url.Action("About", "Home", new { area = "Guru" }) });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.Message, result = "Redirect", url = Url.Action("About", "Home", new { area = "Guru" }) });
            }
            finally
            {

            }
        }

        public ActionResult Details(string id)
        {
            int empId = Convert.ToInt32(id);
            var emp = myDatabase.userRegistrations.Find(empId);
            return View(emp);
        }

        public ActionResult Edit(string id)
        {
            int empId = Convert.ToInt32(id);
            var emp = myDatabase.userRegistrations.Find(empId);
            return View(emp);
        }

        [HttpPost]
        public ActionResult Edit(UserRegistration userReg)
        {
            registration.UpdateUser(userReg);
            return RedirectToAction("Details", "Crud", new { area = "Guru", id = userReg.Id });
        }

        public ActionResult Delete(string id)
        {
            int empId = Convert.ToInt32(id);
            var emp = myDatabase.userRegistrations.Find(empId);
            return View(emp);
        }

        [HttpPost]
        public ActionResult Delete(string id, UserRegistration userReg)
        {
            try
            {
                int empId = Convert.ToInt32(id);
                var emp = myDatabase.userRegistrations.Find(empId);
                myDatabase.userRegistrations.Remove(emp);
                myDatabase.SaveChanges();
                return RedirectToAction("Index", "Login", new { area = "Guru" });
            }
            catch (Exception err)
            {
                return ErrorView(err);
            }

        }

    }
}