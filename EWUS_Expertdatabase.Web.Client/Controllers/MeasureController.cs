﻿using EWUS_Expertdatabase.Business;
using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;

namespace EWUS_Expertdatabase.Web.Client.Controllers
{
    [System.Web.Http.RoutePrefix("Measure")]
    public class MeasureController : Controller
    {
        // GET: Measure
        public ActionResult MeasureIndex()
        {
            return View();
        }

        // GET: Measure
        public JsonResult GetMeasures()
        {
            var measureRepo = new MeasureRepository();
            List<Measure> measures = measureRepo.GetAllMeasures();

            List<MeasureViewModel> lstMeasureViewModel = new List<MeasureViewModel>();

            foreach (var measure in measures)
            {
                MeasureViewModel mvm = new MeasureViewModel();
                mvm.Id = measure.Id;
                mvm.Name = measure.Name;
                mvm.OperationType = measure.OperationType;
                mvm.OperationTypeId = measure.OperationTypeId;
                mvm.Saving = measure.Saving;
                mvm.SerialNumber = measure.SerialNumber;
                mvm.Description = measure.Description;
                mvm.InvestmentCost = measure.InvestmentCost;
                mvm.MeasureLinks = measure.MeasureLinks;
                mvm.DocumentItems = measure.DocumentItems;

                lstMeasureViewModel.Add(mvm);
            }

            return Json(lstMeasureViewModel, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Measure()
        {
            var measureRepo = new MeasureRepository();
            ViewBag.Id = 0;
            ViewBag.DocumentInstanceId = 0;
            ViewBag.Title = "Measure";
            ViewBag.TypeName = "Measure";
            ViewBag.SerialNumber = measureRepo.GetMaxSerialNumber();
            
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

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult MeasureEdit(string key)
        {
            var measureRepo = new MeasureRepository();
            Measure measure = measureRepo.GetMeasureById(key.ToLong(0)) as Measure;
                
            ViewBag.Input = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(measure)));

            ViewBag.Id = key.ToLong(0);
            ViewBag.Title = "Bearbeiten Kunden";
            ViewBag.TypeName = "Measure";
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsPopup = true;
                return PartialView("Measure");
            }
            else
            {
                ViewBag.IsPopup = false;
                return View("Measure");
            }
        }

        [HttpPost]
        public ActionResult SaveMeasure(MeasureViewModel model)
        {
            Measure measure = new Measure();
            measure.Id = model.Id;
            measure.Name = model.Name;
            measure.OperationTypeId = model.OperationTypeId;
            measure.Saving = model.Saving;
            measure.SerialNumber = model.SerialNumber;
            measure.Description = model.Description;
            measure.InvestmentCost = model.InvestmentCost;
            measure.MeasureLinks = model.MeasureLinks;
            measure.DocumentItems = model.DocumentItems;

            var measureRepo = new MeasureRepository();
            var item = measureRepo.SaveMeasure(measure);

            if (item.Success)
                return Json(item.Value);
            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}