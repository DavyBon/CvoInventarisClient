using CvoInventarisClient.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;

namespace CvoInventarisClient.Controllers
{
    [Authorize]
    public class RapporteringObjectController : Controller
    {
        TabelModel model;

        public void vulDropDownLijstenIn()
        {
            DAL.TblObject objectDal = new DAL.TblObject();
            List<ObjectModel> objectModel = objectDal.GetAll();
            List<string> kenmerken = new List<string>();
            List<string> facturen = new List<string>();
            List<string> objectTypes = new List<string>();
            foreach (ObjectModel om in objectModel)
            {
                kenmerken.Add(om.Kenmerken);
                facturen.Add(om.Factuur.FactuurNummer);
                objectTypes.Add(om.ObjectType.Omschrijving);
            }
            SelectList kenmerkenSelectList = new SelectList(kenmerken);
            SelectList facturenSelectlist = new SelectList(facturen);
            SelectList objecytTypesSelelist = new SelectList(objectTypes);
            ViewBag.kenmerken = kenmerkenSelectList;
            ViewBag.facturen = facturenSelectlist;
            ViewBag.objectTypes = objecytTypesSelelist;
        }

        public ActionResult ObjectRapportering()
        {
            model = new TabelModel();
            return View(model);
        }

        public ActionResult Stap3(FormCollection collection)
        {
            model = new TabelModel();
            DAL.TblObject objectDal = new DAL.TblObject();
            List<ObjectModel> objecten = new List<ObjectModel>();
            //een tabel van drie omdat je maar drie lijnen nodig hebt. een select statement, u from en u where clausule
            string[] query = new string[4];

            //hier gaat hem nakijken welke checkboxen er aan gevinkt zijn. dus welke tabellen de klant gegevens van wilt hebben
            List<string> kolomKeuze = new List<string>();
            if (Convert.ToBoolean(collection["kenmerken"].Split(',')[0]) == true)
                kolomKeuze.Add("TblObject.kenmerken");
            if (Convert.ToBoolean(collection["factuur"].Split(',')[0]) == true)
            {
                kolomKeuze.Add("TblFactuur.FactuurNummer");
                query[2] = "LEFT JOIN TblFactuur ON TblObject.idFactuur = TblFactuur.idFactuur";
            }
            if (Convert.ToBoolean(collection["objecttype"].Split(',')[0]) == true)
            {
                kolomKeuze.Add("TblObjectType.omschrijving");
                query[2] = "LEFT JOIN TblObjectType ON TblObject.idObjectType = TblObject.idObjectType";
            }

            List<string> lijstConditie = new List<string>();
            if (collection["kenmerken"].Split(',')[0].Count() != 0)
            {
                string kenmerken = collection["kenmerken"].Split(',')[0];
                lijstConditie.Add("TblObject.kenmerken = " + "'" + kenmerken + "'");
            }
            if (collection["facturen"].Split(',')[0].Count() != 0)
            {
                string facturen = collection["facturen"].Split(',')[0];
                lijstConditie.Add("TblFactuur.FactuurNummer = " + "'" + facturen + "'");
            }
            if (collection["objecttypes"].Split(',')[0].Count() != 0)
            {
                string objecttypes = collection["objecttypes"].Split(',')[0];
                lijstConditie.Add("TblObjectType.omschrijving = " + "'" + objecttypes + "'");
            }
            //maak de select statement
            query[0] = "SELECT ";
            foreach (string s in kolomKeuze)
            {
                query[0] += s + ",";
            }
            //verwijder de laatste komma
            query[0] = query[0].Substring(0, query[0].Length - 1);

            //de FROM
            query[1] = " FROM TblObject ";

            //de where clausule
            query[3] = "WHERE ";
            foreach (string s in lijstConditie)
            {
                query[3] += s + ",";
            }
            query[3] = query[3].Substring(0, query[3].Length - 1);
            string queryResultaat = "";
            foreach (string s in query)
            {
                queryResultaat += s;
            }
            queryResultaat += ";";
            string[] kolomKeuzeArray = kolomKeuze.ToArray();
            objecten = objectDal.Rapportering(queryResultaat, kolomKeuzeArray).ToList();
            model.objecten = objecten;
            //ik geef die mee omdat ik die op de view op hidden ga zetten maar dan kan ik daarna terug aan als ik ze wil opslaan als pdf of excel
            ViewBag.query = queryResultaat;
            ViewBag.kolomKeuze = kolomKeuzeArray;
            vulDropDownLijstenIn();
            ViewBag.stijlStapOpslaan = "inline";
            string kolomnamen = "";
            foreach (string s in kolomKeuze)
            {
                kolomnamen += s + " ";
            }
            ViewBag.kolomnamen = kolomnamen.Trim();
            return View("ObjectRapportering", model);
        }
        [HttpPost]
        public ActionResult StapOpslaan(string kolomnaam, string opslaan, string query)
        {
            model = new TabelModel();
            model = new TabelModel();
            string[] kolomKeuze = new string[kolomnaam.Split(' ').Length];
            int teller = 0;
            foreach (string s in kolomnaam.Split(' '))
            {
                kolomKeuze[teller] = s;
                teller++;
            }
            if (opslaan.Equals("OpslaanExcel"))
            {
                OpslaanExcel(kolomKeuze, query);
            }
            if (opslaan.Equals("OpslaanPdf"))
            {
                OpslaanPdf(kolomKeuze, query);
            }
            vulDropDownLijstenIn();
            return View("ObjectRapportering", model);
        }
        public void OpslaanPdf(string[] kolomKeuze, string query)
        {
            try
            {
                //maakt een nieuw Pdf document aan
                Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                //maakt een nieuw pdf writer object aan waar je in u pdf document in ga writen. net zoals een streamwriter
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                PdfPTable table = new PdfPTable(kolomKeuze.Length);
                DAL.TblObject tblObject = new DAL.TblObject();
                List<ObjectModel> objecten = new List<ObjectModel>();
                objecten = tblObject.Rapportering(query, kolomKeuze).ToList();

                if (objecten[0].Kenmerken != null)
                    table.AddCell("kenmerken");
                if (objecten[0].Factuur != null)
                    table.AddCell("factuur");
                if (objecten[0].ObjectType != null)
                    table.AddCell("object type");

                foreach (var item in objecten)
                {
                    if (objecten[0].Kenmerken != null)
                        table.AddCell(item.Kenmerken);
                    if (objecten[0].Factuur != null)
                        table.AddCell(item.Factuur.FactuurNummer);
                    if (objecten[0].ObjectType != null)
                        table.AddCell(item.ObjectType.Omschrijving);
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
        public void OpslaanExcel(string[] kolomKeuze, string query)
        {
            //maakt een nieuwe excel programma waar je in kunt werken met daaronder een workbook en daarin een worksheet waar u gegevens in komt
            Excel.Application app = new Excel.Application();
            app.Visible = true;
            Excel.Workbook workbook = app.Workbooks.Add(1);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];
            DAL.TblObject tblObject = new DAL.TblObject();
            List<ObjectModel> objecten = new List<ObjectModel>();

            objecten = tblObject.Rapportering(query, kolomKeuze).ToList();
            for (int i = 0; i < kolomKeuze.Length; i++)
            {
                if (objecten[0].Kenmerken != null)
                    worksheet.Cells[1, 1 + i] = "kenmerken";
                if (objecten[0].Factuur != null)
                    worksheet.Cells[1, i + 1] = "factuurnummer";
                if (objecten[0].ObjectType != null)
                    worksheet.Cells[1, i + 1] = "object type";
            }
            for (int i = 0; i < objecten.Count; i++)
            {
                //ik werkt met een teller omdat niet alle kolommen getoond worden. dus als er een kolom gegevens heeft gaat de teller met 1 omhoog waardoor het perfect naast elkaar komt
                int teller = 0;
                if (objecten[0].Kenmerken != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = objecten[i].Kenmerken;
                    teller++;
                }

                if (objecten[0].Factuur != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = objecten[i].Factuur.FactuurNummer;
                    teller++;
                }

                if (objecten[0].ObjectType != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = objecten[i].ObjectType.Omschrijving;
                    teller++;
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