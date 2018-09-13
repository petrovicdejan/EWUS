using EWUS_Expertdatabase.Business;
using EWUS_Expertdatabase.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EWUS_Expertdatabase.Web.Client
{
    [RoutePrefix("Document")]
    public class DocumentController : Controller
    {
        [Route("insert/contentstream")]
        public ActionResult InsertContentStream(string tag, string number)
        {
            Field objField = null;
            Item objItem = new Item();
            objItem.Name = "CmisObject";
            string ObjectId = string.Empty;

            if (number != null)
            {
                ObjectId = number;
            }

            foreach (string file in Request.Files)
            {
                var fileContent = Request.Files[file];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    // get a stream
                    Stream stream = fileContent.InputStream;
                    foreach (PropertyInfo objPropInfo in fileContent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        objField = new Field(objPropInfo.Name, objPropInfo.GetValue(fileContent));
                        objField.IsEnumerable = false;
                        objItem.Fields.Add(objField);
                    };
                    objField = new Field("ObjectId", ObjectId);
                    objField.IsEnumerable = false;
                    objItem.Fields.Add(objField);
                }
            }
            
            string folder = Path.Combine(ConfigurationManager.AppSettings["SharedFolder_" + tag]);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var streamInput = objItem.GetValue("InputStream") as Stream;
            var fileName = objItem.GetValue("ObjectId") as String;
            var path = Path.Combine(ConfigurationManager.AppSettings["SharedFolder_" + tag], fileName);

            using (var fileStream = System.IO.File.Create(path))
            {
                streamInput.CopyTo(fileStream);
            }
            
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Route("download/contentstream")]
        [HttpGet]
        public FileResult DownloadContentStream(string tag, string number, string key = "")
        {
            string objectId = string.Empty;

            try
            {
                if (!string.IsNullOrWhiteSpace(number))
                    objectId = number;

                string fileName = string.Empty;

                if (!string.IsNullOrWhiteSpace(objectId))
                    fileName = objectId;

                var localFilePath = Path.Combine(ConfigurationManager.AppSettings["SharedFolder_" + tag], fileName);

                if (!System.IO.File.Exists(localFilePath))
                {
                    new HttpStatusCodeResult(HttpStatusCode.Gone);
                }
                else
                {// serve the file to the client
                    return new FileStreamResult(new FileStream(localFilePath, FileMode.Open, FileAccess.Read), "image/jpeg");
                }
            }
            catch (Exception ex)
            {
                new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "An error has occured !!!");
            }

            return null;
        }

        [HttpPost]
        public ActionResult DeleteDocument(DocumentItemViewModel model)
        {
            var documentRepo = new DocumentItemRepository();
            var item = documentRepo.DeleteDocumentItem(model.ObjectId);

            return Json(item.Value);
        }
        
    }
}