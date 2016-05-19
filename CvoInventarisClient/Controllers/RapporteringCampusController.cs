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
    [Authorize]
    public class RapporteringCampusController : Controller
    {
        TabelModel model;
        public void vulDropDownLijstenIn()
        {
            DAL.TblCampus campusDal = new DAL.TblCampus();
            List<CampusModel> campusModel = campusDal.GetAll();
            List<string> namen = new List<string>();
            List<string> postcodes = new List<string>();
            List<string> straten = new List<string>();
            List<string> nummers = new List<string>();
            foreach (CampusModel cm in campusModel)
            {
                namen.Add(cm.Naam);
                postcodes.Add(cm.Postcode);
                straten.Add(cm.Straat);
                nummers.Add(cm.Nummer);
            }
            SelectList namenSelectList = new SelectList(namen);
            SelectList postcodesSelectList = new SelectList(postcodes);
            SelectList stratenSelectlist = new SelectList(straten);
            SelectList nummersSelectList = new SelectList(nummers);
            ViewBag.namen = namenSelectList;
            ViewBag.postcodes = postcodesSelectList;
            ViewBag.straten = stratenSelectlist;
            ViewBag.nummers = nummersSelectList;

        }
        public ActionResult CampusRapportering()
        {
            ViewBag.stijlStapOpslaan = "hidden";
            model = new TabelModel();
            vulDropDownLijstenIn();
            return View(model);
        }
        public ActionResult Stap3(FormCollection collection)
        {
            model = new TabelModel();
            DAL.TblCampus TblCampus = new DAL.TblCampus();
            List<CampusModel> campussen = new List<CampusModel>();
            //een tabel van drie omdat je maar drie lijnen nodig hebt. een select statement, u from en u where clausule
            string[] query = new string[4];
            //hier gaat hem nakijken welke checkboxen er aan gevinkt zijn. dus welke tabellen de klant gegevens van wilt hebben
            List<string> kolomKeuze = new List<string>();
            if (Convert.ToBoolean(collection["naam"].Split(',')[0]) == true)
                kolomKeuze.Add("naam");
            if (Convert.ToBoolean(collection["straat"].Split(',')[0]) == true)
                kolomKeuze.Add("straat");
            if (Convert.ToBoolean(collection["nummer"].Split(',')[0]) == true)
                kolomKeuze.Add("nummer");
            if (Convert.ToBoolean(collection["postcode"].Split(',')[0]) == true)
                kolomKeuze.Add("postcode");

            List<string> lijstConditie = new List<string>();
            if (collection["namen"].Split(',')[0].Count() != 0)
            {
                string naam = collection["namen"].Split(',')[0];
                lijstConditie.Add("naam = " + "'" + naam + "'");
            }
            if (collection["straten"].Split(',')[0].Count() != 0)
            {
                string straat = collection["straten"].Split(',')[0];
                lijstConditie.Add("straat = " + "'" + straat + "'");
            }
            if (collection["nummers"].Split(',')[0].Count() != 0)
            {
                string nummer = collection["nummers"].Split(',')[0];
                lijstConditie.Add("nummer = " + "'" + nummer + "'");
            }
            if (collection["postcodes"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["postcodes"].Split(',')[0];
                lijstConditie.Add("postcode = " + "'" + postcode + "'");
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
            query[1] = " FROM TblCampus ";

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
            campussen = TblCampus.Rapportering(queryResultaat, kolomKeuzeArray).ToList();
            model.campussen = campussen;
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
            return View("CampusRapportering", model);
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
            return View("CampusRapportering", model);
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
                DAL.TblCampus TblCampus = new DAL.TblCampus();
                List<CampusModel> campussen = new List<CampusModel>();
                campussen = TblCampus.Rapportering(query, kolomKeuze).ToList();

                if (campussen[0].Naam != null)
                    table.AddCell("naam");
                if (campussen[0].Straat != null)
                    table.AddCell("straat");
                if (campussen[0].Nummer != null)
                    table.AddCell("nummer");
                if (campussen[0].Postcode != null)
                    table.AddCell("postcode");

                foreach (var item in campussen)
                {
                    if (campussen[0].Naam != null)
                        table.AddCell(item.Naam);
                    if (campussen[0].Straat != null)
                        table.AddCell(item.Straat);
                    if (campussen[0].Nummer != null)
                        table.AddCell(item.Nummer);
                    if (campussen[0].Postcode != null)
                        table.AddCell(item.Postcode);
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
            DAL.TblCampus TblCampus = new DAL.TblCampus();
            List<CampusModel> campussen = new List<CampusModel>();
            campussen = TblCampus.Rapportering(query, kolomKeuze).ToList();
            for (int i = 0; i < kolomKeuze.Length; i++)
            {
                if (campussen[0].Naam != null)
                    worksheet.Cells[1, 1 + i] = "naam";
                if (campussen[0].Straat != null)
                    worksheet.Cells[1, i + 1] = "straat";
                if (campussen[0].Nummer != null)
                    worksheet.Cells[1, i + 1] = "nummer";
                if (campussen[0].Postcode != null)
                    worksheet.Cells[1, i + 1] = "postcode";
            }
            for (int i = 0; i < campussen.Count; i++)
            {
                //ik werkt met een teller omdat niet alle kolommen getoond worden. dus als er een kolom gegevens heeft gaat de teller met 1 omhoog waardoor het perfect naast elkaar komt
                int teller = 0;
                if (campussen[0].Naam != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = campussen[i].Naam;
                    teller++;
                }

                if (campussen[0].Straat != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = campussen[i].Straat;
                    teller++;
                }

                if (campussen[0].Nummer != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = campussen[i].Nummer;
                    teller++;
                }

                if (campussen[0].Postcode != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = campussen[i].Postcode;
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