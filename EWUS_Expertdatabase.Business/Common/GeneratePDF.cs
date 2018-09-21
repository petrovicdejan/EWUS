﻿using EWUS_Expertdatabase.Data;
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
            document.SetMargins(75, 75, 90, 30);
            var writer = PdfWriter.GetInstance(document, memoryStream);
            Header events = new Header();
            writer.PageEvent = events;

            var projectMeasureRepo = new ProjectMeasureRepository();
            ProjectMeasurePoco projectMeasurePoco = projectMeasureRepo.GetProjectMeasureById(projectId);

            document.Open();

            DocumentItem logoDoc = null;

            var projectRepo = new ProjectRepository();
            logoDoc = projectRepo.GetDocumentItemForProject(projectMeasurePoco.ProjectId);    
            
            string logoPath = Path.Combine(ConfigurationManager.AppSettings["SharedFolder_Customer"] + @"\" + logoDoc?.ObjectId);
            
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

            string performanceSheetNumber = string.Empty;

            if (projectMeasurePoco.PerformanseSheetNumber != 0)
                performanceSheetNumber = projectMeasurePoco.PerformanseSheetNumber.ToString();

            string companyEmail = string.Empty;

            if(!string.IsNullOrWhiteSpace(projectMeasurePoco?.MaintenanceCompany?.Email))
                companyEmail = "(" + projectMeasurePoco?.MaintenanceCompany?.Email + ")";

            html = html.Replace("$$$Liegenschaftstyp$$$", projectMeasurePoco?.OperationType.HtmlEncode())
                        .Replace("$$$LiegenschaftsNr$$$", performanceSheetNumber.HtmlEncode())
                        .Replace("$$$Standort$$$", projectMeasurePoco.Location.HtmlEncode())
                        .Replace("$$$Plz$$$", projectMeasurePoco?.ZipCode.HtmlEncode())
                        .Replace("$$$Ort$$$", projectMeasurePoco?.City.HtmlEncode())
                        .Replace("$$$Wartungsfirma$$$", projectMeasurePoco?.MaintenanceCompany?.Name.HtmlEncode())
                        .Replace("$$$WartungsfirmaEmail$$$", companyEmail.HtmlEncode())
                        .Replace("$$$Massnahmenart$$$", projectMeasurePoco?.OperationType.HtmlEncode())
                        .Replace("$$$Massnahme$$$", projectMeasurePoco?.MeasureName.HtmlEncode())
                        .Replace("$$$HeaderImage$$$", headerImage);

            string tablesCurrentSituation = string.Empty;
            foreach (var item in projectMeasurePoco.ProjectMeasurePerformances)
            {
                if (item.Hide)
                    continue;
                
                string imagePath = sRootUrl + "document/download/contentstream?Tag=ProjectMeasure&Number=" + item?.DocumentItem?.ObjectId;
                
                string table = " <table class='dotted' height='400'><tbody><tr><td width='610' style='padding-top:9px;'> "
                    + "<p style='padding-top:9px;margin-left:7px;'>" + item.Description.HtmlEncode() + " </p>"
                    + "<p align='center' style='margin-top:50px;'> <img border='0' width='603' height='395' src='" + imagePath + "' /> </p> "
                    + " </td> "
                    + " </tr> "
                    + " </tbody> "
                    + " </table> ";

                tablesCurrentSituation += table;
            }

            html = html.Replace("$$$TableSituation$$$", tablesCurrentSituation)
                        .Replace("$$$Beschreibung$$$", projectMeasurePoco.Description.HtmlEncode());
            
            if (projectMeasurePoco.SubmittedOnDate == null)
                html = html.Replace("$$$EingereichtAm$$$", "[EingereichtAm]");
            else
                html = html.Replace("$$$EingereichtAm$$$", projectMeasurePoco?.SubmittedOnDate.Value.ToString("dd.MM.yyy"));

            return html;
        }
    }
}
