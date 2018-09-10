using EWUS_Expertdatabase.Business;
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
            var classification = new ClassificationRepository();
            return Json(classification.GetClassificationsByType(name), JsonRequestBehavior.AllowGet);
        }
    }
}