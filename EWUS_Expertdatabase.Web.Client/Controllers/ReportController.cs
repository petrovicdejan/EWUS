using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using EWUS_Expertdatabase.Business.Common;

namespace EWUS_Expertdatabase.Web.Client.Controllers
{
    public class ReportController : Controller
    {
        //GenerateReport
        public FileContentResult GenerateReport(long projectId)
        {
            string htmlString = System.Web.HttpContext.Current.Server.MapPath("~/Content/PdfReports/ReportEWUS.html");

            string cssString = System.Web.HttpContext.Current.Server.MapPath("~/Content/PdfReports/report.css");

            string headerString = System.Web.HttpContext.Current.Server.MapPath("~/Content/PdfReports/Header.html");

            string html = System.IO.File.ReadAllText(htmlString);

            string css = System.IO.File.ReadAllText(cssString);

            string header = System.IO.File.ReadAllText(headerString);

            string logo = System.Web.HttpContext.Current.Server.MapPath("~/Lib/images/logo.png");

            string rightLogo = System.Web.HttpContext.Current.Server.MapPath("~/Lib/images/rightLogo.png");

            html = html.Replace("$$$CSS$$$", cssString);           

            byte[] bytes = GeneratePDF.MakePDF(html, css, header,logo,rightLogo,projectId);

            string fileName = "ReportEWUS_" + DateTime.Now.Ticks;

            return File(bytes, "application/pdf", fileName);
        }
    }
}