using EWUS_Expertdatabase.Business;
using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Model;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http;

namespace EWUS_Expertdatabase.Web.Client.Controllers
{
    [RoutePrefix("api")]
    public class WebApiController : ApiController
    {
        public WebApiController()
        {
        }

        //[Route("{name:alpha}")]
        //public HttpResponseMessage GetAll(string name)
        //{
        //    IEnumerable<object> repoAll = null;
        //    using (IRepository repo = ServiceLocator.Current.TryGet<IRepository>(name + "Repository"))
        //    {
        //        repoAll = repo.GetAll();
        //    }
            
        //    Result item = new Result();
        //    item.Value = repoAll;
        //    item.Status = ResultStatus.OK;
        //    var response = CreateResponse(item, string.Empty, false);

        //    return response;
        //}

        //[Route("{name:length(1,140)}")]
        //public HttpResponseMessage Save(string name, [FromBody]Item value)
        //{
        //    HttpResponseMessage result = new HttpResponseMessage();
        //    result.StatusCode = HttpStatusCode.BadRequest;

        //    Result item = null;
        //    using (IRepository repo = ServiceLocator.Current.TryGet<IRepository>(name + "Repository"))
        //    {
        //        item = repo.Save(value);
        //    }

        //    var response = CreateResponse(item, string.Empty, true);

        //    return response;
        //}

        //[Route("GetClassificationsByType/{name:alpha}")]
        //public HttpResponseMessage GetClassificationsByType(string name)
        //{
        //    ClassificationRepository classificationRepo = new ClassificationRepository();
        //    List<Classification> classifications = classificationRepo.GetClassificationsByType(name);

        //    Result item = new Result();
        //    item.Value = classifications;
        //    item.Status = ResultStatus.OK;
        //    var response = CreateResponse(item, string.Empty, false);

        //    return response;
        //}

        //[Route("insert/contentstream")]
        ////[AllowAnonymous]
        //public HttpResponseMessage InsertContentStream(string tag, string number)
        //{
        //    Field objField = null;
        //    Item objItem = new Item();
        //    objItem.Name = "CmisObject";
        //    string ObjectId = string.Empty;

        //    if (number != null)
        //    {
        //        ObjectId = number;
        //    }

        //    try
        //    {
        //        foreach (string file in HttpContext.Current.Request.Files)
        //        {
        //            var fileContent = HttpContext.Current.Request.Files[file];
        //            if (fileContent != null && fileContent.ContentLength > 0)
        //            {
        //                // get a stream
        //                Stream stream = fileContent.InputStream;
        //                foreach (PropertyInfo objPropInfo in fileContent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        //                {
        //                    objField = new Field(objPropInfo.Name, objPropInfo.GetValue(fileContent));
        //                    objField.IsEnumerable = false;
        //                    objItem.Fields.Add(objField);
        //                };
        //                objField = new Field("ObjectId", ObjectId);
        //                objField.IsEnumerable = false;
        //                objItem.Fields.Add(objField);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    HttpResponseMessage response = CreateResponse(null, string.Empty, false);

        //    string folder = Path.Combine(ConfigurationManager.AppSettings["SharedFolder_" + tag]);

        //    if (!Directory.Exists(folder))
        //    {
        //        Directory.CreateDirectory(folder);
        //    }

        //    var streamInput = objItem.GetValue("InputStream") as Stream;
        //    var fileName = objItem.GetValue("ObjectId") as String;
        //    var path = Path.Combine(ConfigurationManager.AppSettings["SharedFolder_" + tag], fileName);

        //    using (var fileStream = File.Create(path))
        //    {
        //        streamInput.CopyTo(fileStream);
        //    }

        //    response.StatusCode = HttpStatusCode.OK;

        //    return response;
        //}

        //[Route("download/contentstream")]
        //[HttpGet]
        //[AllowAnonymous]
        //public HttpResponseMessage DownloadContentStream(string tag, string number, string key="")
        //{
        //    HttpResponseMessage response = new HttpResponseMessage();
        //    response.StatusCode = HttpStatusCode.BadRequest;
        //    string objectId = string.Empty;

        //    try
        //    {

        //        if (!string.IsNullOrWhiteSpace(number))
        //            objectId = number;

        //        string fileName = string.Empty;

        //        if (!string.IsNullOrWhiteSpace(objectId))
        //            fileName = objectId;

        //        var localFilePath = Path.Combine(ConfigurationManager.AppSettings["SharedFolder_" + tag], fileName);

        //        if (!File.Exists(localFilePath))
        //        {
        //            response.StatusCode = HttpStatusCode.Gone;
        //        }
        //        else
        //        {// serve the file to the client
        //            response.StatusCode = HttpStatusCode.OK;
        //            response.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
        //            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //            response.Content.Headers.ContentDisposition.FileName = key;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.StatusCode = HttpStatusCode.InternalServerError;
        //    }

        //    return response;
        //}

        //[Route("command/{name:alpha}")]
        //public HttpResponseMessage Command(string name, [FromBody] Item value)
        //{
        //    HttpResponseMessage result = new HttpResponseMessage();
        //    result.StatusCode = HttpStatusCode.BadRequest;

        //    Result item = null;
        //    using (IRepository repo = ServiceLocator.Current.TryGet<IRepository>(name + "Repository"))
        //    {
        //        item = repo.ExecuteCommand(value);
        //    }

        //    var response = CreateResponse(item, string.Empty, true);

        //    return response;
        //}

        //public ConcurrentDictionary<string, string> _IgnoreFormat = new ConcurrentDictionary<string, string>();

        //protected HttpResponseMessage CreateResponse(Result item, string format, bool withWrap)
        //{
        //    HttpResponseMessage response = new HttpResponseMessage();
        //    response.StatusCode = HttpStatusCode.BadRequest;

        //    if (HttpContext.Current != null)
        //        response = this.Request.CreateResponse(HttpStatusCode.BadRequest);

        //    if (item == null)
        //        return response;

        //    //string output = null;

        //    response.StatusCode = Utils.ToHttpCode(item.Status);

        //    object input = (withWrap) ? item : item.Value;

        //    Type type = (withWrap) ? typeof(Result) : item.Type;

        //    Result result = input as Result;

        //    if (result != null && result.Value != null)
        //    {
        //        result.SerializedValue = JsonConvert.SerializeObject(result.Value);
        //    }

        //    if (Request.Headers.GetValues("Accept").Any(a => a.ToLower() == "application/javascript"))
        //    {
        //        var queryStrings = this.Request.GetQueryNameValuePairs();
        //        if (queryStrings != null)
        //        {
        //            var callBack = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, "callback", true) == 0);

        //            if (callBack.Value != null)
        //            {
        //                string serializeContent = JsonConvert.SerializeObject(input);

        //                response.Content = new StringContent(callBack.Value.ToString() + "(" + serializeContent + ")",
        //                    Encoding.UTF8, "application/x-javascript");
        //                response.StatusCode = HttpStatusCode.OK;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        string serializeContent = JsonConvert.SerializeObject(input);

        //        response.Content = new StringContent(serializeContent, Encoding.UTF8, "application/json");
        //    }

        //    if (string.IsNullOrWhiteSpace(format) == false)
        //        _IgnoreFormat.TryAdd(format, format);

        //    if (item.MaxAge != null && item.MaxAge.HasValue)
        //    {
        //        response.Headers.CacheControl = new CacheControlHeaderValue()
        //        {
        //            Public = true,
        //            MaxAge = item.MaxAge
        //        };

        //    }

        //    if (item.Tag != null && !string.IsNullOrEmpty(item.Tag.ToString()))
        //    {
        //        long totalCount = 0;
        //        long.TryParse(item.Tag.ToString(), out totalCount);
        //        response.Headers.Add("m-paging-total", totalCount.ToString());

        //    }

        //    return response;
        //}
    }
}
