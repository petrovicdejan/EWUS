using AutoMapper;
using EWUS_Expertdatabase.Business;
using EWUS_Expertdatabase.Model;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EWUS_Expertdatabase.Web.Client.Controllers
{
    [RoutePrefix("Classification")]
    public class ClassificationController : Controller
    {
        // GET: GetClassificationsByType
        [Route("GetClassificationsByType/{name:alpha}")]
        public JsonResult GetClassificationsByType(string name)
        {
            var classificationRepo = new ClassificationRepository();
            List<Classification> classifications = classificationRepo.GetClassificationsByType(name);

            List<ClassificationViewModel> lstClassificationViewModel = new List<ClassificationViewModel>();
            lstClassificationViewModel = Mapper.Map<List<Classification>, List<ClassificationViewModel>>(classifications);
            
            return Json(lstClassificationViewModel, JsonRequestBehavior.AllowGet);            
        }
    }
}