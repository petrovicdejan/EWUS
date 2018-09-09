using EWUS_Expertdatabase.Business;
using EWUS_Expertdatabase.Model;
using Newtonsoft.Json;
using System;
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
            ViewBag.SerialNumber = MeasureRepository.GetMaxSerialNumber();
            
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

            Measure measure = measureRepo.GetById(key.ToLong(0)) as Measure;
                
            ViewBag.Input = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(measure)));

            var documentInstanceRepo = new DocumentInstanceRepository();
            DocumentInstance documentInstance = documentInstanceRepo.GetByRefers(measure.Id, "Measure");

            ViewBag.DocumentInstance = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(documentInstance)));

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
    }
}