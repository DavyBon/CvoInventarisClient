using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;
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
    public class RapporteringInventarisController : Controller
    {
        TabelModel model;
        public void vulDropDownLijstenIn()
        {
            DAL.TblInventaris inventarisDal = new DAL.TblInventaris();
            List<InventarisModel> inventarisModel = inventarisDal.GetAll();
            List<LokaalModel> lokalen = new List<LokaalModel>();
            List<ObjectModel> objecten = new List<ObjectModel>();
            List<VerzekeringModel> verzekeringen = new List<VerzekeringModel>();
            List<bool> aanwezigs = new List<bool>();
            List<bool> actiefs = new List<bool>();
            List<string> labels = new List<string>();
            List<string> historieken = new List<string>();
            List<int> aankoopjaren = new List<int>();
            List<int> afschrijvingsperiodes = new List<int>();
            foreach (InventarisModel im in inventarisModel)
            {
                lokalen.Add(im.Lokaal);
                objecten.Add(im.Object);
                verzekeringen.Add(im.Verzekering);
                aanwezigs.Add(im.IsAanwezig);
                actiefs.Add(im.IsActief);
                labels.Add(im.Label);
                historieken.Add(im.Historiek);
                aankoopjaren.Add(im.Aankoopjaar);
                afschrijvingsperiodes.Add(im.Afschrijvingsperiode);
            }
            SelectList lokalenSelectList = new SelectList(lokalen);
            SelectList objectenSelectList = new SelectList(objecten);
            SelectList verzekeringenSelectlist = new SelectList(verzekeringen);
            SelectList aanwezigsSelectList = new SelectList(aanwezigs);
            SelectList actiefsSelectList = new SelectList(actiefs);
            SelectList labelsSelectList = new SelectList(labels);
            SelectList historiekenSelectList = new SelectList(historieken);
            SelectList aankoopjarenSelectList = new SelectList(aankoopjaren);
            SelectList afschrijvingsperiodesSelectList = new SelectList(afschrijvingsperiodes);
            ViewBag.lokalen = lokalenSelectList;
            ViewBag.objecten = objectenSelectList;
            ViewBag.verzekeringen = verzekeringenSelectlist;
            ViewBag.aanwezigs = aanwezigsSelectList;
            ViewBag.actiefs = actiefsSelectList;
            ViewBag.labels = labelsSelectList;
            ViewBag.historieken = historiekenSelectList;
            ViewBag.aankoopjaren = aankoopjarenSelectList;
            ViewBag.afschrijvingsperiodes = afschrijvingsperiodesSelectList;
        }
        public ActionResult InventarisRapportering()
        {
            ViewBag.stijlStapOpslaan = "hidden";
            model = new TabelModel();
            vulDropDownLijstenIn();
            return View(model);
        }
        public ActionResult Stap3(FormCollection collection)
        {
            model = new TabelModel();
            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();
            List<InventarisModel> inventarissen = new List<InventarisModel>();
            //een tabel van drie omdat je maar drie lijnen nodig hebt. een select statement, u from en u where clausule
            string[] query = new string[4];
            //hier gaat hem nakijken welke checkboxen er aan gevinkt zijn. dus welke tabellen de klant gegevens van wilt hebben
            List<string> kolomKeuze = new List<string>();
            if (Convert.ToBoolean(collection["Lokaal"].Split(',')[0]) == true)
                kolomKeuze.Add("Lokaal");
            if (Convert.ToBoolean(collection["Object"].Split(',')[0]) == true)
                kolomKeuze.Add("Object");
            if (Convert.ToBoolean(collection["Verzekering"].Split(',')[0]) == true)
                kolomKeuze.Add("Verzekering");
            if (Convert.ToBoolean(collection["Aanwezig"].Split(',')[0]) == true)
                kolomKeuze.Add("Aanwezig");
            if (Convert.ToBoolean(collection["Actief"].Split(',')[0]) == true)
                kolomKeuze.Add("Actief");
            if (Convert.ToBoolean(collection["Label"].Split(',')[0]) == true)
                kolomKeuze.Add("Label");
            if (Convert.ToBoolean(collection["Historiek"].Split(',')[0]) == true)
                kolomKeuze.Add("Historiek");
            if (Convert.ToBoolean(collection["Aankoopjaar"].Split(',')[0]) == true)
                kolomKeuze.Add("Aankoopjaar");
            if (Convert.ToBoolean(collection["Afschrijvingsperiode"].Split(',')[0]) == true)
                kolomKeuze.Add("Afschrijvingsperiode");

            List<string> lijstConditie = new List<string>();
            if (collection["lokalen"].Split(',')[0].Count() != 0)
            {
                string naam = collection["lokalen"].Split(',')[0];
                lijstConditie.Add("lokaal = " + "'" + naam + "'");
            }
            if (collection["objecten"].Split(',')[0].Count() != 0)
            {
                string straat = collection["objecten"].Split(',')[0];
                lijstConditie.Add("object = " + "'" + straat + "'");
            }
            if (collection["verzekeringen"].Split(',')[0].Count() != 0)
            {
                string nummer = collection["verzekeringen"].Split(',')[0];
                lijstConditie.Add("verzekering = " + "'" + nummer + "'");
            }
            if (collection["aanwezigs"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["aanwezigs"].Split(',')[0];
                lijstConditie.Add("aanwezig = " + "'" + postcode + "'");
            }
            if (collection["actiefs"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["actiefs"].Split(',')[0];
                lijstConditie.Add("actief = " + "'" + postcode + "'");
            }
            if (collection["labels"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["labels"].Split(',')[0];
                lijstConditie.Add("label = " + "'" + postcode + "'");
            }
            if (collection["historieken"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["historieken"].Split(',')[0];
                lijstConditie.Add("historiek = " + "'" + postcode + "'");
            }
            if (collection["aankoopjaren"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["aankoopjaren"].Split(',')[0];
                lijstConditie.Add("aankoopjaar = " + "'" + postcode + "'");
            }
            if (collection["afschrijvingsperiodes"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["afschrijvingsperiodes"].Split(',')[0];
                lijstConditie.Add("afschrijvingsperiode = " + "'" + postcode + "'");
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
            query[1] = " FROM TblInventaris ";

            //de where clausule
            query[2] = "WHERE ";
            foreach (string s in lijstConditie)
            {
                query[2] += s + ",";
            }
            query[2] = query[2].Substring(0, query[2].Length - 1);
            string queryResultaat = "";
            foreach (string s in query)
            {
                queryResultaat += s;
            }
            queryResultaat += ";";
            string[] kolomKeuzeArray = kolomKeuze.ToArray();
            inventarissen = TblInventaris.Rapportering(queryResultaat, kolomKeuzeArray).ToList();
            model.inventarissen = inventarissen;
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
            return View("InventarisRapportering", model);
        }

        [HttpPost]
        public ActionResult StapOpslaan(string kolomnaam, string opslaan, string query)
        {
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
            return View("InventarisRapportering", model);
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
                DAL.TblInventaris TblInventaris = new DAL.TblInventaris();
                List<InventarisModel> inventarissen = new List<InventarisModel>();
                inventarissen = TblInventaris.Rapportering(query, kolomKeuze).ToList();

                if (inventarissen[0].Lokaal != null)
                    table.AddCell("Lokaal");
                if (inventarissen[0].Object != null)
                    table.AddCell("Object");
                if (inventarissen[0].Verzekering != null)
                    table.AddCell("Verzekering");
                if (inventarissen[0].IsAanwezig != false)
                    table.AddCell("Aanwezig");
                if (inventarissen[0].IsActief != false)
                    table.AddCell("Actief");
                if (inventarissen[0].Label != null)
                    table.AddCell("Label");
                if (inventarissen[0].Historiek != null)
                    table.AddCell("Historiek");
                if (inventarissen[0].Aankoopjaar != 0)
                    table.AddCell("Aankoopjaar");
                if (inventarissen[0].Afschrijvingsperiode != 0)
                    table.AddCell("Afschrijvingsperiode");

                foreach (var item in inventarissen)
                {
                    if (inventarissen[0].Lokaal != null)
                        table.AddCell(item.Lokaal.LokaalNaam);
                    if (inventarissen[0].Object != null)
                        table.AddCell(item.Object.Kenmerken);
                    if (inventarissen[0].Verzekering != null)
                        table.AddCell(item.Verzekering.Omschrijving);
                    if (inventarissen[0].IsAanwezig != false)
                        table.AddCell(item.IsAanwezig.ToString());
                    if (inventarissen[0].IsActief != false)
                        table.AddCell(item.IsActief.ToString());
                    if (inventarissen[0].Label != null)
                        table.AddCell(item.Label);
                    if (inventarissen[0].Historiek != null)
                        table.AddCell(item.Historiek);
                    if (inventarissen[0].Aankoopjaar != 0)
                        table.AddCell(item.Aankoopjaar.ToString());
                    if (inventarissen[0].Afschrijvingsperiode != 0)
                        table.AddCell(item.Afschrijvingsperiode.ToString());
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
            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();
            List<InventarisModel> inventarissen = new List<InventarisModel>();
            inventarissen = TblInventaris.Rapportering(query, kolomKeuze).ToList();
            for (int i = 0; i < kolomKeuze.Length; i++)
            {
                if (inventarissen[0].Lokaal != null)
                    worksheet.Cells[1, 1 + i] = "Lokaal";
                if (inventarissen[0].Object != null)
                    worksheet.Cells[1, i + 1] = "Object";
                if (inventarissen[0].Verzekering != null)
                    worksheet.Cells[1, i + 1] = "Verzekering";
                if (inventarissen[0].IsAanwezig != false)
                    worksheet.Cells[1, i + 1] = "Aanwezig";
                if (inventarissen[0].IsActief != false)
                    worksheet.Cells[1, i + 1] = "Actief";
                if (inventarissen[0].Label != null)
                    worksheet.Cells[1, i + 1] = "Label";
                if (inventarissen[0].Historiek != null)
                    worksheet.Cells[1, i + 1] = "Historiek";
                if (inventarissen[0].Aankoopjaar != 0)
                    worksheet.Cells[1, i + 1] = "Aankoopjaar";
                if (inventarissen[0].Afschrijvingsperiode != 0)
                    worksheet.Cells[1, i + 1] = "Afschrijvingsperiode";
            }
            for (int i = 0; i < inventarissen.Count; i++)
            {
                //ik werkt met een teller omdat niet alle kolommen getoond worden. dus als er een kolom gegevens heeft gaat de teller met 1 omhoog waardoor het perfect naast elkaar komt
                int teller = 0;
                if (inventarissen[0].Lokaal != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = inventarissen[0].Lokaal;
                    teller++;
                }
                if (inventarissen[0].Object != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = inventarissen[0].Object;
                    teller++;
                }
                if (inventarissen[0].Verzekering != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = inventarissen[0].Verzekering;
                    teller++;
                }
                if (inventarissen[0].IsAanwezig != false)
                {
                    worksheet.Cells[i + 2, 1 + teller] = inventarissen[0].IsAanwezig;
                    teller++;
                }
                if (inventarissen[0].IsActief != false)
                {
                    worksheet.Cells[i + 2, 1 + teller] = inventarissen[0].IsActief;
                    teller++;
                }
                if (inventarissen[0].Label != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = inventarissen[0].Label;
                    teller++;
                }
                if (inventarissen[0].Historiek != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = inventarissen[0].Historiek;
                    teller++;
                }
                if (inventarissen[0].Aankoopjaar != 0)
                {
                    worksheet.Cells[i + 2, 1 + teller] = inventarissen[0].Aankoopjaar;
                    teller++;
                }
                if (inventarissen[0].Afschrijvingsperiode != 0)
                {
                    worksheet.Cells[i + 2, 1 + teller] = inventarissen[0].Afschrijvingsperiode;
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