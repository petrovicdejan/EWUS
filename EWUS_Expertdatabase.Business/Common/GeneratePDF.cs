using EWUS_Expertdatabase.Data;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using EWUS_Expertdatabase.Model;

namespace EWUS_Expertdatabase.Business.Common
{
   public static class GeneratePDF
    {
        public static byte[] MakePDF(string htmlString,string cssText,string header,string logo,string rightLogo,long projectId)
        {
            var memoryStream = new MemoryStream();
            var document = new iTextSharp.text.Document();
            var writer = PdfWriter.GetInstance(document, memoryStream);
            Header events = new Header();
            writer.PageEvent = events;

            var projectMeasureRepo = new ProjectMeasureRepository();
            ProjectMeasurePoco projectMeasurePoco = projectMeasureRepo.GetProjectMeasureById(projectId);

            document.Open();

            DocumentItem logoDoc = null;

            var projectRepo = new ProjectRepository();
            logoDoc = projectRepo.GetDocumentItemForProject(projectMeasurePoco.ProjectId);    
            
            string logoPath = Path.Combine(ConfigurationManager.AppSettings["SharedFolder_Customer"] + @"\" + logoDoc.ObjectId);
            
            events.setHeader(logoPath, rightLogo);
            document.NewPage();

            htmlString = FillPdfWithData(htmlString, projectMeasurePoco);
            
            using (var cssMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(cssText)))
            {
                using (var htmlMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(htmlString)))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, htmlMemoryStream, cssMemoryStream);
                }
            }
            document.Close();

            byte[] bytes = memoryStream.ToArray();

            return bytes;
        }

        public static string FillPdfWithData(string html, ProjectMeasurePoco projectMeasurePoco) {
            
            if (projectMeasurePoco == null)
                return html;

            var sRootUrl = ConfigurationManager.AppSettings["rootURL"];

            string headerImage = sRootUrl + "document/download/contentstream?Tag=ProjectMeasure&Number=Deutchland.png";
            
            html = html.Replace("$$$Liegenschaftstyp$$$", projectMeasurePoco.OperationType)
                        .Replace("$$$LiegenschaftsNr$$$",projectMeasurePoco.PerformanseSheetNumber.ToString())
                        .Replace("$$$Standort$$$", projectMeasurePoco.Location)
                        .Replace("$$$Plz$$$", projectMeasurePoco.ZipCode)
                        .Replace("$$$Ort$$$", projectMeasurePoco.City)
                        .Replace("$$$Wartungsfirma$$$", projectMeasurePoco?.MaintenanceCompany?.Name)
                        .Replace("$$$WartungsfirmaEmail$$$", projectMeasurePoco?.MaintenanceCompany?.Email)
                        .Replace("$$$Massnahmenart$$$", projectMeasurePoco?.OperationType)
                        .Replace("$$$Massnahme$$$", projectMeasurePoco?.MeasureName)
                        .Replace("$$$HeaderImage$$$", headerImage);

            string tablesCurrentSituation = string.Empty;
            foreach (var item in projectMeasurePoco.DocumentItems)
            {
                if (item.Hide)
                    continue;
                
                string imagePath = sRootUrl + "document/download/contentstream?Tag=ProjectMeasure&Number=" + item.ObjectId;
                
                string table = " <table class='dotted' width='604' height='500'><tbody><tr><td width ='604' valign='top'> "
                    + "<p>" +item.Description +" </p>"
                    + "<p align='center'> <img border='0' width='320' height='320' src='" + imagePath + "' /> </p> "
                    + " </td> "
                    + " </tr> "
                    + " </tbody> "
                    + " </table> ";

                tablesCurrentSituation += table;

            }
            
            string tableBesch = "<table class='dotted'><tbody><tr><td width='604' valign='top'> <p>"+ projectMeasurePoco.Description + " "
                          + "</p> "
                       + "</td>"
                    + " </tr>"
                 + "</tbody>"
             + "</table> ";

            if (projectMeasurePoco.Description == null || string.IsNullOrWhiteSpace(projectMeasurePoco.Description))
                tableBesch = string.Empty;

            html = html.Replace("$$$TableSituation$$$", tablesCurrentSituation)
                        .Replace("$$$Beschreibung$$$", tableBesch)
                        .Replace("$$$EingereichtAm$$$", projectMeasurePoco.SubmittedOnDate.ToString()); ;
            
            return html;

        }
    }
}
