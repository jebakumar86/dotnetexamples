﻿using MVC.Samples.BLL.Interfaces.Ravi;
using MVC.Samples.Data;
using MVC.Samples.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Samples.Web.Controllers;


namespace MVC.Samples.Web.Areas.Ravi.Controllers
{
    public class CrudController : BaseController
    {
        MyDatabase myDatabase = new MyDatabase();
        private readonly IRegistration registration;

        public CrudController(IRegistration registration) {
            this.registration = registration;
        }

        // GET: Ravi/Crud

        public ActionResult Index()
        {
            try
            {
                return View(myDatabase.userRegistrations.ToList());
            }
            catch(Exception ex)
            {
                //Log the errors
                ViewBag.Message = "Sorry, Some error occured while processing Main page.";
                return ErrorView(ex);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserRegistration objEmp)
        {
            try {
                string errorMessage = registration.BasicValidations(objEmp);
                string name = objEmp.Name;
                string empCode = objEmp.EmpCode;

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        if (registration.UserNameValidation(name) != false && registration.EmpCodeValidation(empCode) != false)
                        {
                            registration.SaveUser(objEmp);
                            return RedirectToAction("About", "Home", new { area = "Ravi" });

                        }
                        else { ViewBag.Error = "User Name or employee code already exit"; }
                    }
                    else { ViewBag.Message = "Password Should be minimum 4 and not null"; return View(); }
                }
                else { ViewBag.ErrorMessage = "Age should be above 18 and below 100"; return View(); }
                return View();
            } 
            catch (Exception err)
            {
                return ErrorView(err);
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
            return RedirectToAction("Details", "Crud", new { area = "Ravi" , id = userReg.Id });
        }

        public ActionResult Delete(string id)
        {
            int empId = Convert.ToInt32(id);
            var emp = myDatabase.userRegistrations.Find(empId);
            return View(emp);
        }

        [HttpPost]
        public ActionResult Delete(string id,UserRegistration userReg)
        {
            try {
                int empId = Convert.ToInt32(id);
                var emp = myDatabase.userRegistrations.Find(empId);
                myDatabase.userRegistrations.Remove(emp);
                myDatabase.SaveChanges();
                return RedirectToAction("Index", "Login", new { area = "Ravi" });
            } 
            catch(Exception err) {
                return ErrorView(err);
            }
            
        }


    }
}