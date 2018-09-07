using EWUS_Expertdatabase.Data;
using System.Web.Mvc;

namespace EWUS_Expertdatabase.Web.Controllers
{
    [RoutePrefix("Classification")]
    public class ClassificationController : Controller
    {
        // GET: GetClassificationsByType
        [Route("GetClassificationsByType/{type:alpha}")]
        public JsonResult GetClassificationsByType(string type)
        {
            var classification = new ClassificationRepository();
            return Json(classification.GetClassificationsByType(type), JsonRequestBehavior.AllowGet);
        }
    }
}