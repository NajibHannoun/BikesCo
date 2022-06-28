using BikesTest.Exceptions;
using BikesTest.Interfaces;
using BikesTest.Models;
using BikesTest.Models.GlueModels;
using BikesTest.ServiceExtentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Controllers
{
    public class BicyclesController : Controller
    {

        private readonly IBicycleService<Bicycle> _bSrvc;
        private readonly IUserService<Admin> _aSrvc;
        private readonly IUserService<Customer> _cSrvc;
        public BicyclesController(IBicycleService<Bicycle> Bservice,
                                  IUserService<Customer> Cservice,
                                  IUserService<Admin> Aservice)
        {
            _bSrvc = Bservice;
            _cSrvc = Cservice;
            _aSrvc = Aservice;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            //List<Bicycle> listBicycle = _bSrvc.GetAll();
            //List<GlueBicycle> listGlueBicycle = _bSrvc.GlueBicycleList(listBicycle);

            return View(_bSrvc.GetAll());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            //return View(_bSrvc.GetGlueBicycleById(id));
            return View(_bSrvc.GetById(id));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            try
            {
                int currentUserId = Int32.Parse(User.Identities
                                                .FirstOrDefault().FindFirst("Id").Value);

   
                _aSrvc.CheckSuspended(currentUserId);
                return View();
            }
            catch (SuspendedAdminException e)
            {
                TempData["AdminSuspendedError"] = e.Message;
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Bicycle bicycle)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new InvalidOperationException("Bicycle model invalid at creation");

                _bSrvc.Create(bicycle);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
            
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                int currentAdminId = Int32.Parse(User.Identities.FirstOrDefault().FindFirst("Id").Value);
                _aSrvc.CheckSuspended(currentAdminId);
                return View(_bSrvc.GetById(id));
            }
            catch (SuspendedAdminException e)
            {
                TempData["AdminSuspendedError"] = e.Message;
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,  Bicycle bicycle)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new InvalidOperationException("Bicycle model invalid at edition");

                bicycle = _bSrvc.Update(bicycle);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return View();
            }

        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                int currentAdminId = Int32.Parse(User.Identities.FirstOrDefault().FindFirst("Id").Value);
                _aSrvc.CheckSuspended(currentAdminId);
                return View(_bSrvc.GetById(id));
            }
            catch (SuspendedAdminException e)
            {
                TempData["AdminSuspendedError"] = e.Message;
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Bicycle bicycle)
        {
            _bSrvc.Delete(bicycle);
            return RedirectToAction(nameof(Index));
        }
    }
}
