using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Business.Common
{
   public static class GeneratePDF
    {
        public static byte[] MakePDF(string htmlString,string cssText,string header,string logo,string rightLogo)
        {
            var memoryStream = new MemoryStream();
            var document = new iTextSharp.text.Document();
            var writer = PdfWriter.GetInstance(document, memoryStream);
            Header events = new Header();
            writer.PageEvent = events;
            
            document.Open();

            events.setHeader(logo,rightLogo);
            document.NewPage();
            
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
    }
}
