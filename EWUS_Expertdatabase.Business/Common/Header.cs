using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Business
{
    public class Header : PdfPageEventHelper
    {
        protected string logoImg;
        protected string rightLogo;

        public void setHeader(string logoImg,string rightLogo)
        {
            this.logoImg = logoImg;
            this.rightLogo = rightLogo;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            iTextSharp.text.Image logoFirst = null;
            iTextSharp.text.Image logoSecond = null;

            if (File.Exists(this.logoImg))
                logoFirst = iTextSharp.text.Image.GetInstance(this.logoImg);

            if(File.Exists(this.rightLogo))
                logoSecond = iTextSharp.text.Image.GetInstance(this.rightLogo);
            
            PdfPTable table = new PdfPTable(2);
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            PdfPCell cell = new PdfPCell(logoFirst,true);            
            cell.FixedHeight = 43;           
            cell.Border = Rectangle.NO_BORDER;                     
            table.AddCell(cell);
            
            PdfPCell cellTwo = new PdfPCell(logoSecond,true);
            cellTwo.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellTwo.FixedHeight = 43;
           
            cellTwo.Border = Rectangle.NO_BORDER;
            table.AddCell(cellTwo);
            
            table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin; 
            table.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height-3, writer.DirectContent);
        }
    }

   public class dot : IPdfPCellEvent
    {
        public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
        {
            cell.Border = Rectangle.TOP_BORDER;
            PdfContentByte cb = canvases[PdfPTable.BASECANVAS];
            cb.SetLineDash(0, 2,2);
           // cb.SetLineWidth(1);
            cb.Rectangle(position.Left, position.Bottom, position.Width, position.Height);
            cb.Stroke();
        }
    }

    class DottedCells : IPdfPTableEvent
    {
        public void TableLayout(PdfPTable table, float[][] widths, float[] heights, int headerRows, int rowStart, PdfContentByte[] canvases)
        {
            PdfContentByte canvas = canvases[PdfPTable.LINECANVAS];
            canvas.SetLineDash(3f, 3f);
            float llx = widths[0][0];
            float urx = widths[0][widths[0].Length - 1];
            for (int i = 0; i < heights.Length; i++)
            {
                canvas.MoveTo(llx, heights[i]);
                canvas.LineTo(urx, heights[i]);
            }
            for (int i = 0; i < widths.Length; i++)
            {
                for (int j = 0; j < widths[i].Length; j++)
                {
                    canvas.MoveTo(widths[i][j], heights[i]);
                    canvas.LineTo(widths[i][j], heights[i + 1]);
                }
            }
            canvas.Stroke();
        }
    }
}
