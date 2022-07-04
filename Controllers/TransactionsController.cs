using BikesTest.Exceptions;
using BikesTest.Interfaces;
using BikesTest.Models;
using BikesTest.Models.GlueModels;
using BikesTest.ServiceExtentions;
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
    public class TransactionsController : Controller
    {
        private readonly ITransactionService<Transaction> _tService;
        private readonly IUserService<Admin> _aService;
        private readonly IUserService<Customer> _cService;
        private readonly IBicycleService<Bicycle> _bService;
        public TransactionsController(ITransactionService<Transaction> tService,
                                      IUserService<Admin> aService,
                                      IUserService<Customer> cService,
                                      IBicycleService<Bicycle> bService)
        {
            _tService = tService;
            _aService = aService;
            _cService = cService;
            _bService = bService;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(_tService.GetAll());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeletedIndex()
        {
            return View(_tService.GetAllDeleted());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeletedDetails(int id)
        {
            return View(_tService.GetByDeletedId(id));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            return View(_tService.GetById(id));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                int currentAdminId = Int32.Parse(User.Identities.FirstOrDefault().FindFirst("Id").Value);
                _aService.CheckSuspended(currentAdminId);
                return View(_tService.GetById(id));
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
        public ActionResult Edit(Transaction transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new InvalidOperationException("Transaction invalid");

                transaction = _tService.Update(transaction);

                return RedirectToAction("Details", "Transactions", new { id = transaction.id });
            }
            catch (CustomerDoesntExistException e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("customer_Id", e.Message);
                return View(_tService.GetById(transaction.id));
            }
            catch (BikeDoesntExistExeption e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("bicycle_Id", e.Message);
                return View(_tService.GetById(transaction.id));
            }
            catch (AdminDoesntExistException e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("admin_Id", e.Message);
                return View(_tService.GetById(transaction.id));
            }
            catch (CurrentlyBikingException e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("customer_Id", e.Message);
                return View(_tService.GetById(transaction.id));
            }
            catch (CurrentlyRentException e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("bicycle_Id", e.Message);
                return View(_tService.GetById(transaction.id));
            }
            catch (SuspendedAdminException e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("admin_Id", e.Message);
                return View(_tService.GetById(transaction.id));
            }
            catch (Exception e)
            {
                ViewData["error"] = e.Message;
                return View(_tService.GetById(transaction.id));
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetTransaction()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTransaction(string username)
        {
            try
            {
                if (_cService.IsUsernameExist(username))
                {
                    if (!_cService.GetById(_tService.GetByUsername(username).customer.id).isCurrentlyBiking)
                        throw new CustomerDidntRentException("This customer did not rent");
                    if (!_bService.GetById(_tService.GetByUsername(username).bicycle.id).isCurrentlyRented)
                        throw new UnrentBikeExcpeiton("This bicycle has not been rented");

                    return View("Create", _tService.GetByUsername(username));
                }
                else
                    throw new InvalidUsernameException("This username Does not Exist");
            }
            catch (UnrentBikeExcpeiton e)
            {
                ViewData["error"] = e.Message;
                return View();
            }
            catch (CustomerDidntRentException e)
            {
                ViewData["error"] = e.Message;
                return View();
            }
            catch (InvalidUsernameException e)
            {
                ViewData["error"] = e.Message;
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            try
            {
                int currentAdminId = Int32.Parse(User.Identities.FirstOrDefault().FindFirst("Id").Value);
                _aService.CheckSuspended(currentAdminId);

                ViewBag.usernames = _cService.GetUsernamesAndIds();
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
        public ActionResult Create([FromForm] Transaction transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new InvalidOperationException("Transaction invalid");

                transaction = _tService.Create(transaction);

                return RedirectToAction(nameof(Index));
            }
            catch (CustomerDoesntExistException e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("customer_Id", e.Message);
                return View(transaction);
            }
            catch (BikeDoesntExistExeption e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("bicycle_Id", e.Message);
                return View(transaction);
            }
            catch (AdminDoesntExistException e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("admin_Id", e.Message);
                return View(transaction);
            }
            catch (CurrentlyBikingException e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("customer_Id", e.Message);
                return View(transaction);
            }
            catch (CurrentlyRentException e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("bicycle_Id", e.Message);
                return View(transaction);
            }
            catch (SuspendedAdminException e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("admin_Id", e.Message);
                return View(transaction);
            }
            catch (CustomerDidntRentException e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("customer_Id", e.Message);
                return View(transaction);
            }
            catch (UnrentBikeExcpeiton e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("bicycle_Id", e.Message);
                return View(transaction);
            }
            catch (BikeMissmatchCustomerException e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("cutomer_Id", e.Message);
                return View(transaction);
            }
            catch (Exception e)
            {
                ViewBag.usernames = _cService.GetUsernamesAndIds();
                ModelState.AddModelError("Transaction state invalid", e.Message);
                return View(transaction);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                int currentAdminId = Int32.Parse(User.Identities.FirstOrDefault().FindFirst("Id").Value);
                _aService.CheckSuspended(currentAdminId);
                return View(_tService.GetById(id));
            }
            catch(SuspendedAdminException e)
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
        public ActionResult Delete(int id, Transaction transaction)
        {
            try
            {
                _tService.Delete(transaction);
                return RedirectToAction(nameof(Index));
            }
            catch (CustomerDoesntExistException e)
            {
                ModelState.AddModelError("customerId", e.Message);
                return View(id);
            }
            catch (BikeDoesntExistExeption e)
            {
                ModelState.AddModelError("bicycleId", e.Message);
                return View(id);
            }
            catch (AdminDoesntExistException e)
            {
                ModelState.AddModelError("adminId", e.Message);
                return View(id);
            }
            catch (Exception e)
            {
                return View(id);
            }
        }
    }
}
