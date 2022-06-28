using BikesTest.Exceptions;
using BikesTest.Interfaces;
using BikesTest.Models;
using BikesTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Controllers
{
    public class AdminsController : Controller
    {        
        private IUserService<Admin> _service;


        public AdminsController(IUserService<Admin> service)
        {
            _service = service;
            
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(_service.GetAll());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {   
            return View(_service.GetById(id));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin admin)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new InvalidOperationException("Admin model invalid at creation");                   
                
                _service.Create(admin);
                return RedirectToAction(nameof(Index));
            }
            catch(ExistingUsernameException e)
            {
                ViewData["UsernameError"] = e.Message;
                return View();
            }
            catch (ExistingEmailException e)
            {
                ViewData["EmailError"] = e.Message;
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View(_service.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Admin admin)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new InvalidOperationException("Admin model invalid at edition");

                _service.Update(admin);

                return RedirectToAction(nameof(Index));
            }
            catch 
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return View(_service.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Admin admin)
        {
            try
            {
                _service.Delete(admin);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
