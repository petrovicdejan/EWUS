using AutoMapper;
using EWUS_Expertdatabase.Business;
using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EWUS_Expertdatabase.Web.Client
{
    //[RoutePrefix("Party")]
    public class PartyController : Controller
    {
        // GET: Party
        [Route("wartungsfirmen")]
        public ActionResult MaintenanceCompanyIndex()
        {
            return View();
        }

        [Route("kunden")]
        public ActionResult CustomerIndex()
        {
            return View();
        }

        // GET: Measure
        public JsonResult GetAllMaintenanceCompanies()
        {
            var partyRepo = new PartyRepository();
            List<MaintenanceCompany> maintenanceCompanies = partyRepo.GetAllMaintenanceCompanies();

            List<MaintenanceCompanyViewModel> lstMaintenanceCompanyViewModel = new List<MaintenanceCompanyViewModel>();
            lstMaintenanceCompanyViewModel = Mapper.Map<List<MaintenanceCompany>, List<MaintenanceCompanyViewModel>>(maintenanceCompanies);

            return Json(lstMaintenanceCompanyViewModel, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult MaintenanceCompany()
        {
            ViewBag.Id = 0;
            ViewBag.Title = "Wartungsfirma";
            ViewBag.TypeName = "MaintenanceCompany";
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsPopup = true;
                return PartialView();
            }
            else
            {
                ViewBag.IsPopup = false;
                return View();
            }
        }

        // GET: Measure
        public JsonResult GetAllCustomers()
        {
            var partyRepo = new PartyRepository();
            List<Customer> customers = partyRepo.GetAllCustomers();

            List<CustomerViewModel> lstCustomerViewModel = new List<CustomerViewModel>();
            lstCustomerViewModel = Mapper.Map<List<Customer>, List<CustomerViewModel>>(customers);

            return Json(lstCustomerViewModel, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Customer()
        {
            ViewBag.Id = 0;
            ViewBag.Title = "Kunden";
            ViewBag.TypeName = "MaintenanceCompany";
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsPopup = true;
                return PartialView();
            }
            else
            {
                ViewBag.IsPopup = false;
                return View();
            }
        }

        [HttpPost]
        public ActionResult SaveMaintenanceCompany(MaintenanceCompanyViewModel model)
        {
            MaintenanceCompany maintenanceCompany = new MaintenanceCompany();

            maintenanceCompany = Mapper.Map<MaintenanceCompanyViewModel, MaintenanceCompany>(model);

            var partyRepo = new PartyRepository();
            var item = partyRepo.SaveMaintenanceCompany(maintenanceCompany);

            if (item.Success)
                return Json(item.Value);
            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "An error has occured !!!");
        }

        [HttpPost]
        public ActionResult SaveCustomer(CustomerViewModel model)
        {
            Customer customer = new Customer();

            customer = Mapper.Map<CustomerViewModel, Customer>(model);

            var partyRepo = new PartyRepository();
            var item = partyRepo.SaveCustomer(customer);

            if (item.Success)
                return Json(item.Value);
            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "An error has occured !!!");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EditMaintenanceCompany(string key)
        {
            var partyRepo = new PartyRepository();
            MaintenanceCompany maintenanceCompany = partyRepo.GetMaintenanceCompanyById(key.ToLong(0));

            ViewBag.Input = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(maintenanceCompany)));

            ViewBag.Id = key.ToLong(0);
            ViewBag.Title = "Bearbeiten Wartungsfirma";
            ViewBag.TypeName = "MaintenanceCompany";
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsPopup = true;
                return PartialView("MaintenanceCompany");
            }
            else
            {
                ViewBag.IsPopup = false;
                return View("MaintenanceCompany");
            }
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EditCustomer(string key)
        {
            var partyRepo = new PartyRepository();
            Customer customer = partyRepo.GetCustomerById(key.ToLong(0));

            ViewBag.Input = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(customer)));

            ViewBag.Id = key.ToLong(0);
            ViewBag.Title = "Bearbeiten Kunden";
            ViewBag.TypeName = "Customer";
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsPopup = true;
                return PartialView("Customer");
            }
            else
            {
                ViewBag.IsPopup = false;
                return View("Customer");
            }
        }
        
        [HttpPost]
        public ActionResult DeleteCustomer(int Id)
        {
            if (Id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var partyRepo = new PartyRepository();
            var item = partyRepo.DeleteCustomerById(Id);

            if (item.Success)
                return Json(item);
            else
                return new HttpStatusCodeResult(Utils.ToHttpCode(item.Status), item.ExceptionMessage);
        }

        [HttpPost]
        public ActionResult DeleteMaintenanceCompany(int Id)
        {
            if (Id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var partyRepo = new PartyRepository();
            var item = partyRepo.DeleteMaintenanceCompanyById(Id);

            if (item.Success)
                return Json(item);
            else
                return new HttpStatusCodeResult(Utils.ToHttpCode(item.Status), item.ExceptionMessage);
        }
    }
}