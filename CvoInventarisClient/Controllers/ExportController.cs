using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Controllers
{
    public class ExportController : Controller
    {
        // GET: Export
        public ActionResult ExportToPdf(int[] modelList, string type)
        {
            List<object> objectenLijst = new List<object>();
            switch (type)
            {
                case "Object":
                    DAL.TblObject tblObject = new DAL.TblObject();
                    foreach (int id in modelList)
                    {
                        objectenLijst.Add(tblObject.GetById(id));
                    }
                    break;
            }
            try
            {
                //maakt een nieuw Pdf document aan
                Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                //maakt een nieuw pdf writer object aan waar je in u pdf document in ga writen. net zoals een streamwriter
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                PdfPTable table = new PdfPTable(objectenLijst[0].GetType().GetProperties().Count());
                foreach (var prop in objectenLijst[0].GetType().GetProperties())
                {
                    if(prop.GetValue(objectenLijst[0], null)!= null)
                    {
                        table.AddCell(prop.Name);
                    }
                    
                }
                for (int i = 0; i < objectenLijst.Count; i++)
                {
                    foreach (var prop in objectenLijst[i].GetType().GetProperties())
                    {
                        if (prop.GetValue(objectenLijst[0], null) != null)
                        {
                            table.AddCell(prop.GetValue(objectenLijst[i], null).ToString());
                        }
                            
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

            return RedirectToAction("Index", type);
        }
        public ActionResult ExportToExcel(int[] modelList, string type)
        {
            List<object> objectenLijst = new List<object>();
            switch (type)
            {
                case "Object":
                    DAL.TblObject tblObject = new DAL.TblObject();
                    foreach (int id in modelList)
                    {
                        objectenLijst.Add(tblObject.GetById(id));
                    }
                    break;
            }
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(1);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
            int counter = 0;
            for (int i = 0; i < objectenLijst[0].GetType().GetProperties().Count(); i++)
            {
                var prop = objectenLijst[0].GetType().GetProperties()[i];
                if (prop.GetValue(objectenLijst[0], null) != null)
                {
                    worksheet.Cells[1, 1 + counter] = prop.Name;
                    counter++;
                }
            }
            for (int i = 0; i < objectenLijst.Count; i++)
            {
                int teller = 0;
                for (int y = 0; y < objectenLijst[i].GetType().GetProperties().Count(); y++)
                {
                    var prop = objectenLijst[i].GetType().GetProperties()[y];
                    if (prop.GetValue(objectenLijst[0], null) != null)
                    {
                        worksheet.Cells[i + 2, 1 + teller] = prop.GetValue(objectenLijst[i], null).ToString();
                        teller++;
                    }
                }
            }

            worksheet.Columns.AutoFit();
            Response.Buffer = true;
            //dit is het stuk dat zorgt dat je het kunt opslaan op een locatie naar keuze
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=rapport.xls");
            Response.Write(worksheet);
            Response.End();

            return RedirectToAction("Index", type);
        }
    }
}