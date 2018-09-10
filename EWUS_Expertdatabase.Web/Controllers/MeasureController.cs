using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace EWUS_Expertdatabase.Web.Controllers
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
            var measure = new MeasureRepository();
            return Json(measure.GetMeasures(), JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Measure()
        {
            ViewBag.Id = 0;
            ViewBag.DocumentInstanceId = 0;
            ViewBag.Title = "Measure";
            ViewBag.TypeName = "Measure";
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

        //[System.Web.Http.Route("Create")]
        //[System.Web.Http.HttpPost]
        //public ActionResult Create(string userJson)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var measure = new MeasureRepository();
        //            bool saved = measure.SaveMeasure(new Item());
        //            if (saved)
        //            {
        //                return RedirectToAction("Index");
        //            }
        //        }
        //        return View();
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}