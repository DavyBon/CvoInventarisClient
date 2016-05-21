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
                //export code hier
            }
            return RedirectToAction("Index", type);
        }
    }
}