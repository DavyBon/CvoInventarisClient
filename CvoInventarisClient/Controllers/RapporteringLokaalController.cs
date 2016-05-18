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
    public class RapporteringLokaalController : Controller
    {
        TabelModel model;

        public void vulDropDownLijstenIn()
        {
            DAL.TblLokaal lokaalDal = new DAL.TblLokaal();
            List<LokaalModel> lokaalModel = lokaalDal.GetAll();
            List<string> namen = new List<string>();
            List<string> campussen = new List<string>();
            foreach (LokaalModel cm in lokaalModel)
            {
                namen.Add(cm.LokaalNaam);
                campussen.Add(cm.Campus.Naam);
            }
            SelectList namenSelectList = new SelectList(namen);
            SelectList campusSelectList = new SelectList(campussen);
            ViewBag.namen = namenSelectList;
            ViewBag.campussen = campusSelectList;
        }

        public ActionResult LokaalRapportering()
        {
            model = new TabelModel();
            return View(model);
        }
        public ActionResult Stap3(FormCollection collection)
        {
            model = new TabelModel();
            DAL.TblLokaal lokaalDal = new DAL.TblLokaal();
            List<LokaalModel> lokalen = new List<LokaalModel>();
            //een tabel van drie omdat je maar drie lijnen nodig hebt. een select statement, u from en u where clausule
            string[] query = new string[4];
            
            //hier gaat hem nakijken welke checkboxen er aan gevinkt zijn. dus welke tabellen de klant gegevens van wilt hebben
            List<string> kolomKeuze = new List<string>();
            if (Convert.ToBoolean(collection["lokaalNaam"].Split(',')[0]) == true)
                kolomKeuze.Add("TblLokaal.lokaalNaam");
            if (Convert.ToBoolean(collection["aantalPlaatsen"].Split(',')[0]) == true)
                kolomKeuze.Add("TblLokaal.aantalPlaatsen");
            if (Convert.ToBoolean(collection["isComputerLokaal"].Split(',')[0]) == true)
                kolomKeuze.Add("TblLokaal.isComputerLokaal");
            if (Convert.ToBoolean(collection["campus"].Split(',')[0]) == true)
            {
                kolomKeuze.Add("TblCampus.naam");
                query[2] = "LEFT JOIN TblCampus ON TblLokaal.idCampus = TblCampus.idCampus";
            }
                

            List<string> lijstConditie = new List<string>();
            if (collection["namen"].Split(',')[0].Count() != 0)
            {
                string naam = collection["namen"].Split(',')[0];
                lijstConditie.Add("TblLokaal.naam = " + "'" + naam + "'");
            }
            if (Convert.ToBoolean(collection["aantalPlaatsenKeuze"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblLokaal.aantalPlaatsen " + collection["aantalPlaatsenKeuze"].Split(',')[0] + " " + collection["aantalPlaatsenKeuze1"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["isComputerLokaalKeuze"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblLokaal.isComputerLokaalKeuze =  " + collection["isComputerLokaalKeuze"].Split(',')[0]);
            }
            if (collection["campussen"].Split(',')[0].Count() != 0)
            {
                string campus = collection["campussen"].Split(',')[0];
                lijstConditie.Add("TblCampus.naam = " + "'" + campus + "'");
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
            query[1] = " FROM TblLokaal ";

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
            lokalen = lokaalDal.Rapportering(queryResultaat, kolomKeuzeArray).ToList();
            model.lokalen = lokalen;
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
            return View("LokaalRapportering", model);
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
            return View("CpuRapportering", model);
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
                DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
                List<LokaalModel> lokalen = new List<LokaalModel>();
                lokalen = tblLokaal.Rapportering(query, kolomKeuze).ToList();

                if (lokalen[0].LokaalNaam != null)
                    table.AddCell("naam");
                if (lokalen[0].AantalPlaatsen != 0)
                    table.AddCell("aantal plaatsen");
                if (lokalen[0].IsComputerLokaal != null)
                    table.AddCell("computerlokaal");
                if (lokalen[0].Campus != null)
                    table.AddCell("campus");

                foreach (var item in lokalen)
                {
                    if (lokalen[0].LokaalNaam != null)
                        table.AddCell(item.LokaalNaam);
                    if (lokalen[0].AantalPlaatsen != 0)
                        table.AddCell(item.AantalPlaatsen.ToString());
                    if (lokalen[0].IsComputerLokaal)
                        table.AddCell(item.IsComputerLokaal.ToString());
                    if (lokalen[0].Campus != null)
                        table.AddCell(item.Campus.Naam);
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
            DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
            List<LokaalModel> lokalen = new List<LokaalModel>();
            lokalen = tblLokaal.Rapportering(query, kolomKeuze).ToList();
            for (int i = 0; i < kolomKeuze.Length; i++)
            {
                if (lokalen[0].LokaalNaam != null)
                    worksheet.Cells[1, 1 + i] = "naam";
                if (lokalen[0].AantalPlaatsen != 0)
                    worksheet.Cells[1, i + 1] = "aantal plaatsen";
                if (lokalen[0].IsComputerLokaal != null)
                    worksheet.Cells[1, i + 1] = "computer lokaal";
                if (lokalen[0].Campus.Naam != null)
                    worksheet.Cells[1, i + 1] = "campus";
            }
            for (int i = 0; i < lokalen.Count; i++)
            {
                //ik werkt met een teller omdat niet alle kolommen getoond worden. dus als er een kolom gegevens heeft gaat de teller met 1 omhoog waardoor het perfect naast elkaar komt
                int teller = 0;
                if (lokalen[0].LokaalNaam != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = lokalen[i].LokaalNaam;
                    teller++;
                }

                if (lokalen[0].AantalPlaatsen != 0)
                {
                    worksheet.Cells[i + 2, 1 + teller] = lokalen[i].AantalPlaatsen;
                    teller++;
                }

                if (lokalen[0].IsComputerLokaal != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = lokalen[i].IsComputerLokaal;
                    teller++;
                }

                if (lokalen[0].Campus != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = lokalen[i].Campus.Naam;
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