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

        public ActionResult Export(int[] modelList, string type, string exportType)
        {
            List<object> objects = new List<object>();

            switch (type)
            {
                case "Object":
                    DAL.TblObject tblObject = new DAL.TblObject();
                    foreach (int id in modelList)
                    {
                        objects.Add(tblObject.GetById(id));
                    }
                    break;
                case "ObjectType":
                    DAL.TblObjectType tblObjectType = new DAL.TblObjectType();
                    foreach (int id in modelList)
                    {
                        objects.Add(tblObjectType.GetById(id));
                    }
                    break;
                case "Verzekering":
                    DAL.TblVerzekering tblVerzekering = new DAL.TblVerzekering();
                    foreach (int id in modelList)
                    {
                        objects.Add(tblVerzekering.GetById(id));
                    }
                    break;
                case "Lokaal":
                    DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
                    foreach (int id in modelList)
                    {
                        objects.Add(tblLokaal.GetById(id));
                    }
                    break;
                case "Leverancier":
                    DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
                    foreach (int id in modelList)
                    {
                        objects.Add(tblLeverancier.GetById(id));
                    }
                    break;
                case "Inventaris":
                    DAL.TblInventaris tblInventaris = new DAL.TblInventaris();
                    foreach (int id in modelList)
                    {
                        objects.Add(tblInventaris.GetById(id));
                    }
                    break;
                case "Factuur":
                    DAL.TblFactuur tblFactuur = new DAL.TblFactuur();
                    foreach (int id in modelList)
                    {
                        objects.Add(tblFactuur.GetById(id));
                    }
                    break;
                case "Campus":
                    DAL.TblCampus tblCampus = new DAL.TblCampus();
                    foreach (int id in modelList)
                    {
                        objects.Add(tblCampus.GetById(id));
                    }
                    break;
            }
            if (exportType.Equals("pdf"))
            {
                try
                {
                    //maakt een nieuw Pdf document aan
                    Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                    //maakt een nieuw pdf writer object aan waar je in u pdf document in ga writen. net zoals een streamwriter
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();
                    PdfPTable table = new PdfPTable(objects[0].GetType().GetProperties().Count());
                    foreach (var prop in objects[0].GetType().GetProperties())
                    {
                        if (prop.GetValue(objects[0], null) != null)
                        {
                            table.AddCell(prop.Name);
                        }

                    }
                    for (int i = 0; i < objects.Count; i++)
                    {
                        foreach (var prop in objects[i].GetType().GetProperties())
                        {
                            if (prop.GetValue(objects[0], null) != null)
                            {
                                table.AddCell(prop.GetValue(objects[i], null).ToString());
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
            }
            else if(exportType.Equals("excel"))
            {
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                app.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(1);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                int counter = 0;
                for (int i = 0; i < objects[0].GetType().GetProperties().Count(); i++)
                {
                    var prop = objects[0].GetType().GetProperties()[i];
                    if (prop.GetValue(objects[0], null) != null)
                    {
                        worksheet.Cells[1, 1 + counter] = prop.Name;
                        counter++;
                    }
                }
                for (int i = 0; i < objects.Count; i++)
                {
                    int teller = 0;
                    for (int y = 0; y < objects[i].GetType().GetProperties().Count(); y++)
                    {
                        var prop = objects[i].GetType().GetProperties()[y];
                        if (prop.GetValue(objects[0], null) != null)
                        {
                            worksheet.Cells[i + 2, 1 + teller] = prop.GetValue(objects[i], null).ToString();
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
            }
            return RedirectToAction("Index", type);
        }
    }
}