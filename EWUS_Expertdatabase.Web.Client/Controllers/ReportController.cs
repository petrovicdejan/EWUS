using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using EWUS_Expertdatabase.Business.Common;
using System.Configuration;

namespace EWUS_Expertdatabase.Web.Client.Controllers
{
    public class ReportController : Controller
    {
        //GenerateReport
        public FileContentResult GenerateReport(long projectId)
        {
            string htmlString = System.Web.HttpContext.Current.Server.MapPath("~/Content/PdfReports/ReportEWUS.html");

            string cssString = System.Web.HttpContext.Current.Server.MapPath("~/Content/PdfReports/report.css");
            
            string html = System.IO.File.ReadAllText(htmlString);

            string css = System.IO.File.ReadAllText(cssString);
            
            string logo = System.Web.HttpContext.Current.Server.MapPath("~/Lib/images/logo.png");

            string rightLogo = System.Web.HttpContext.Current.Server.MapPath("~/Lib/images/ewus.png");

            html = html.Replace("$$$CSS$$$", cssString);           

            byte[] bytes = GeneratePDF.MakePDF(html, css, logo,rightLogo,projectId);

            string fileName = "ReportEWUS_" + DateTime.Now.Ticks + ".pdf";

            var openInBrowser = ConfigurationManager.AppSettings["openInBrowser"];

            if (openInBrowser == "true")            
                return File(bytes, "application/pdf");
            else 
                return File(bytes, "application/force-download", fileName);
        }
    }
}