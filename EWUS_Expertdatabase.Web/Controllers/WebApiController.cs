using EWUS_Expertdatabase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EWUS_Expertdatabase.Web.Controllers
{
    [RoutePrefix("api")]
    public class WebApiController : ApiController
    {
        public WebApiController()
        {
        }

        [Route("{name:alpha}")]
        public HttpResponseMessage GetAll(string name)
        {
            HttpResponseMessage response = null;

            return response;
        }

        [Route("{name:length(1,140)}")]
        public HttpResponseMessage Put(string name, [FromBody]Item value)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            result.StatusCode = HttpStatusCode.OK;

            return result;
        }

        [Route("{name:length(1,140)}")]
        public HttpResponseMessage Post(string name, [FromBody]Item value)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            result.StatusCode = HttpStatusCode.OK;

            return result;
        }
    }
}
