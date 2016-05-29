using ExcelLibrary.SpreadSheet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
                    Document pdfDoc;
                    //maakt een nieuw Pdf document aan
                    if (objects[0].GetType().GetProperties().Count() > 5)
                        pdfDoc = new Document(iTextSharp.text.PageSize.A4.Rotate(), 25, 10, 25, 10);
                    else pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);

                    //maakt een nieuw pdf writer object aan waar je in u pdf document in ga writen. net zoals een streamwriter
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    iTextSharp.text.Font fontRapport = new iTextSharp.text.Font(bfTimes, 20, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    Phrase phrase = new Phrase("Rapport", fontRapport);
                    pdfDoc.Add(phrase);
                    Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    pdfDoc.Add(p);
                    iTextSharp.text.Font fontDatum = new iTextSharp.text.Font(bfTimes, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    var dateTimeNow = DateTime.Now; // Return 00/00/0000 00:00:00
                    var dateOnlyString = dateTimeNow.ToShortDateString(); //Return 00/00/0000
                    Phrase datum = new Phrase("Opgemaakt op " + dateOnlyString, fontDatum);
                    pdfDoc.Add(datum);

                    PdfPTable table = new PdfPTable(objects[0].GetType().GetProperties().Count() - 1);

                    string naamHoofding = objects[0].GetType().Name;
                    naamHoofding.Trim();
                    naamHoofding = naamHoofding.Remove(naamHoofding.Length - 5);

                    iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 18, iTextSharp.text.Font.ITALIC, BaseColor.WHITE);
                    PdfPCell cell = new PdfPCell(new Phrase(naamHoofding, times));
                    cell.Colspan = objects[0].GetType().GetProperties().Count();
                    cell.BackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(62, 182, 222));
                    cell.BorderColor = new BaseColor(Color.FromArgb(30, 84, 102));
                    table.AddCell(cell);

                    iTextSharp.text.Font fontHeader = new iTextSharp.text.Font(bfTimes, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bfTimes, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    foreach (var prop in objects[0].GetType().GetProperties())
                    {
                        if (!prop.Name.Equals("Id"))
                        {
                            table.AddCell(new Phrase(prop.Name, fontHeader));
                        }

                    }
                    for (int i = 0; i < objects.Count; i++)
                    {
                        foreach (var prop in objects[i].GetType().GetProperties())
                        {
                            if (!prop.Name.Equals("Id"))
                            {

                                if (prop.GetValue(objects[0], null) == null)
                                {
                                    table.AddCell(new Phrase(""));

                                    //table.AddCell(prop.GetValue(objects[i], null).ToString());
                                }
                                else
                                {
                                    table.AddCell(new Phrase(prop.GetValue(objects[i], null).ToString(), font));
                                }
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
                Workbook workbook = new Workbook();
                Worksheet worksheet = new Worksheet("Rapport Sheet");
                workbook.Worksheets.Add(worksheet);
                int counter = 0;
                for (int i = 0; i < objects[0].GetType().GetProperties().Count(); i++)
                {
                    var prop = objects[0].GetType().GetProperties()[i];
                    if (prop.GetValue(objects[0], null) != null)
                    {
                        worksheet.Cells[0, 0 + counter] = new Cell(prop.Name);
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
                            worksheet.Cells[i + 1, y] = new Cell(prop.GetValue(objects[i], null).ToString());
                            teller++;
                        }
                    }
                }
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;  filename=rapport.xls");
                    workbook.Save(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("Index", type);
        }
    }
}