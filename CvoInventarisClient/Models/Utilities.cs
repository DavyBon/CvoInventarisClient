using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Models
{
    public class Utilities : Controller
    {
        List<object> objectenLijst = new List<object>();
        private object kolomKeuze;

        public List<object> ObjectenLijst
        {
            get
            {
                return objectenLijst;
            }

            set
            {
                objectenLijst = value;
            }
        }

        public void OpslaanPdf()
        {
            try
            {
                //maakt een nieuw Pdf document aan
                Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                //maakt een nieuw pdf writer object aan waar je in u pdf document in ga writen. net zoals een streamwriter
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                PdfPTable table = new PdfPTable(objectenLijst[0].GetType().GetProperties().Count());
                foreach(var prop in objectenLijst[0].GetType().GetProperties())
                {
                    table.AddCell(prop.Name);
                }
                for(int i = 0; i < objectenLijst.Count;i++)
                {
                    foreach (var prop in objectenLijst[i].GetType().GetProperties())
                    {
                        table.AddCell(prop.GetValue(objectenLijst[i],null).ToString());
                    }
                }
                pdfDoc.Add(table);

                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                //dit is het stuk dat zorgt dat je het kunt opslaan op een locatie naar keuze
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Rapport.pdf");
                Response.Write(pdfDoc);
                Response.End();
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }
        }
        public void OpslaanExcel()
        {
            //maakt een nieuwe excel programma waar je in kunt werken met daaronder een workbook en daarin een worksheet waar u gegevens in komt
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(1);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
            for(int i = 0; i < objectenLijst[0].GetType().GetProperties().Count(); i++)
            {
                var prop = objectenLijst[0].GetType().GetProperties()[i];
                worksheet.Cells[1, 1 + i] = prop.Name;

            }
            for (int i = 0; i < objectenLijst.Count; i++)
            {
                for (int y = 0; y < objectenLijst[i].GetType().GetProperties().Count(); y++)
                {
                    var prop = objectenLijst[i].GetType().GetProperties()[y];
                    worksheet.Cells[i + 2, 1 + y] = prop.GetValue(objectenLijst[i], null).ToString();
                }
            }
            
            worksheet.Columns.AutoFit();
            Response.Buffer = true;
            //dit is het stuk dat zorgt dat je het kunt opslaan op een locatie naar keuze
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=rapport.xls");
            Response.Write(worksheet);
            Response.End();
        }
    }
}