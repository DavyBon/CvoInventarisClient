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
    public class RapporteringFactuurController : Controller
    {
        TabelModel model;

        public void vulDropDownLijstenIn()
        {
            DAL.TblFactuur factuurDal = new DAL.TblFactuur();
            List<FactuurModel> factuurModel = factuurDal.GetAll();
            List <string> boekjaren = new List<string>();
            List <string> cvoVolgNummers = new List<string>();
            List <string> factuurNummers = new List<string>();
            List <string> scholengroepNummers = new List<string>();
            List <string> factuurDatums = new List<string>();
            List <string> leveranciers = new List<string>();
            List <string> prijzen = new List<string>();
            List <int> garanties = new List<int>();
            List <string> omschrijvingen = new List<string>();
            List <string> opmerkingen = new List<string>();
            List <int> afschrijfperiodes = new List<int>();
            List <string> datumInserts = new List<string>();
            List <string> userInserts = new List<string>();
            List <string> datumModifieds = new List<string>();
            List <string> userModifieds = new List<string>();
            foreach (FactuurModel fm in factuurModel)
            {
                boekjaren.Add(fm.Boekjaar);
                cvoVolgNummers.Add(fm.CvoVolgNummer);
                factuurNummers.Add(fm.FactuurNummer);
                scholengroepNummers.Add(fm.ScholengroepNummer);
                factuurDatums.Add(fm.FactuurDatum);
                leveranciers.Add(fm.Leverancier.Naam);
                prijzen.Add(fm.Prijs);
                garanties.Add(fm.Garantie);
                omschrijvingen.Add(fm.Omschrijving);
                opmerkingen.Add(fm.Opmerking);
                afschrijfperiodes.Add(fm.Afschrijfperiode);
                datumInserts.Add(fm.DatumInsert);
                userInserts.Add(fm.UserInsert);
                datumModifieds.Add(fm.DatumModified);
                userModifieds.Add(fm.UserModified);
            }
            SelectList boekjarenSelectList = new SelectList(boekjaren);
            SelectList cvoVolgNummersSelectList = new SelectList(cvoVolgNummers);
            SelectList factuurNummersSelectList = new SelectList(factuurNummers);
            SelectList scholengroepNummersSelectList = new SelectList(scholengroepNummers);
            SelectList factuurDatumsSelectList = new SelectList(factuurDatums);
            SelectList leveranciersSelectList = new SelectList(leveranciers);
            SelectList prijzenSelectList = new SelectList(prijzen);
            SelectList garantiesSelectList = new SelectList(garanties);
            SelectList omschrijvingenSelectList = new SelectList(omschrijvingen);
            SelectList opmerkingenSelectList = new SelectList(opmerkingen);
            SelectList afschrijfperiodesSelectList = new SelectList(afschrijfperiodes);
            SelectList datumInsertsSelectList = new SelectList(datumInserts);
            SelectList userInsertsSelectList = new SelectList(userInserts);
            SelectList datumModifiedsSelectList = new SelectList(datumModifieds);
            SelectList userModifiedsSelectList = new SelectList(userModifieds);
            ViewBag.boekjaren = boekjarenSelectList;
            ViewBag.cvoVolgNummers = cvoVolgNummersSelectList;
            ViewBag.factuurNummers = factuurNummersSelectList;
            ViewBag.scholengroepNummers = scholengroepNummersSelectList;
            ViewBag.factuurDatums = factuurDatumsSelectList;
            ViewBag.leveranciers = leveranciersSelectList;
            ViewBag.prijzen = prijzenSelectList;
            ViewBag.garanties = garantiesSelectList;
            ViewBag.omschrijvingen = omschrijvingenSelectList;
            ViewBag.opmerkingen = opmerkingenSelectList;
            ViewBag.afschrijfperiodes = afschrijfperiodesSelectList;
            ViewBag.datumInserts = datumInsertsSelectList;
            ViewBag.userInserts = userInsertsSelectList;
            ViewBag.datumModifieds = datumModifiedsSelectList;
            ViewBag.userModifieds = userModifiedsSelectList;
        }

        public ActionResult FactuurRapportering()
        {
            model = new TabelModel();
            return View(model);
        }
        public ActionResult Stap3(FormCollection collection)
        {
            model = new TabelModel();
            DAL.TblFactuur factuurDal = new DAL.TblFactuur();
            List<FactuurModel> facturen = new List<FactuurModel>();
            //een tabel van drie omdat je maar drie lijnen nodig hebt. een select statement, u from en u where clausule
            string[] query = new string[4];
            
            //hier gaat hem nakijken welke checkboxen er aan gevinkt zijn. dus welke tabellen de klant gegevens van wilt hebben
            List<string> kolomKeuze = new List<string>();
            if (Convert.ToBoolean(collection["boekjaar"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.Boekjaar");
            if (Convert.ToBoolean(collection["cvoVolgNummer"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.CvoVolgNummer");
            if (Convert.ToBoolean(collection["factuurNummer"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.FactuurNummer");
            if (Convert.ToBoolean(collection["scholengroepNummer"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.ScholengroepNummer");
            if (Convert.ToBoolean(collection["factuurDatum"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.FactuurDatum");
            if (Convert.ToBoolean(collection["leverancier"].Split(',')[0]) == true)
            {
                kolomKeuze.Add("TblLeverancier.naam");
                query[2] = "LEFT JOIN TblLeverancier ON TblFactuur.idLeverancier = TblLeverancier.idLeverancier";
            }
            if (Convert.ToBoolean(collection["prijs"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.Prijs");
            if (Convert.ToBoolean(collection["garantie"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.Garantie");
            if (Convert.ToBoolean(collection["omschrijving"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.Omschrijving");
            if (Convert.ToBoolean(collection["opmerking"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.Opmerking");
            if (Convert.ToBoolean(collection["afschrijfperiode"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.Afschrijfperiode");
            if (Convert.ToBoolean(collection["datumInsert"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.DatumInsert");
            if (Convert.ToBoolean(collection["userInsert"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.UserInsert");
            if (Convert.ToBoolean(collection["datumModified"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.DatumModified");
            if (Convert.ToBoolean(collection["userModified"].Split(',')[0]) == true)
                kolomKeuze.Add("TblFactuur.UserModified");

            List<string> lijstConditie = new List<string>();
            if (Convert.ToBoolean(collection["boekjaar"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.Boekjaar " + collection["boekjaar"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["cvoVolgNummer"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.CvoVolgNummer " + collection["cvoVolgNummer"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["factuurNummer"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.FactuurNummer " + collection["factuurNummer"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["scholengroepNummer"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.scholengroepNummer " + collection["scholengroepNummer"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["factuurDatum"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.FactuurDatum " + collection["factuurDatum"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["leverancier"].Split(',')[0]) == true)
            {
                string leverancier = collection["leveranciers"].Split(',')[0];
                lijstConditie.Add("TblLeverancier.naam = " + "'" + leverancier + "'");
            }
            if (Convert.ToBoolean(collection["prijs"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.Prijs " + collection["prijs"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["garantie"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.Garantie " + collection["garantie"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["omschrijving"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.Omschrijving " + collection["omschrijving"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["opmerking"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.Opmerking " + collection["opmerking"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["afschrijfperiode"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.Afschrijfperiode " + collection["afschrijfperiode"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["datumInsert"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.DatumInsert " + collection["datumInsert"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["userInsert"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.UserInsert " + collection["userInsert"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["datumModified"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.DatumModified " + collection["datumModified"].Split(',')[0]);
            }
            if (Convert.ToBoolean(collection["userModified"].Split(',')[0]) == true)
            {
                lijstConditie.Add("TblFactuur.UserModified " + collection["userModified"].Split(',')[0]);
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
            query[1] = " FROM TblFactuur ";

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
            facturen = factuurDal.Rapportering(queryResultaat, kolomKeuzeArray).ToList();
            model.facturen = facturen;
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
            return View("FactuurRapportering", model);
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
            return View("FactuurRapportering", model);
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
                DAL.TblFactuur tblFactuur = new DAL.TblFactuur();
                List<FactuurModel> facturen = new List<FactuurModel>();
                facturen = tblFactuur.Rapportering(query, kolomKeuze).ToList();

                if (facturen[0].Boekjaar != null)
                    table.AddCell("boekjaar");
                if (facturen[0].CvoVolgNummer != null)
                    table.AddCell("CVO volgnummer");
                if (facturen[0].FactuurNummer != null)
                    table.AddCell("factuurnummer");
                if (facturen[0].ScholengroepNummer != null)
                    table.AddCell("scholengroepnummer");
                if (facturen[0].FactuurDatum != null)
                    table.AddCell("factuurdatum");
                if (facturen[0].Leverancier != null)
                    table.AddCell("leverancier");
                if (facturen[0].Prijs != null)
                    table.AddCell("prijs");
                if ((object)facturen[0].Garantie != null)
                    table.AddCell("garantie");
                if (facturen[0].Omschrijving != null)
                    table.AddCell("omschrijving");
                if (facturen[0].Opmerking != null)
                    table.AddCell("opmerking");
                if ((object)facturen[0].Afschrijfperiode != null)
                    table.AddCell("afschrijfperiode");
                if (facturen[0].DatumInsert != null)
                    table.AddCell("datum insert");
                if (facturen[0].UserInsert != null)
                    table.AddCell("user insert");
                if (facturen[0].DatumModified != null)
                    table.AddCell("datum modified");
                if (facturen[0].UserModified != null)
                    table.AddCell("user modified");

                foreach(var item in facturen)
                {
                    if (facturen[0].Boekjaar != null)
                        table.AddCell(item.Boekjaar);
                    if (facturen[0].CvoVolgNummer != null)
                        table.AddCell(item.CvoVolgNummer);
                    if (facturen[0].FactuurNummer != null)
                        table.AddCell(item.FactuurNummer);
                    if (facturen[0].ScholengroepNummer != null)
                        table.AddCell(item.ScholengroepNummer);
                    if (facturen[0].FactuurDatum != null)
                        table.AddCell(item.FactuurDatum);
                    if (facturen[0].Leverancier != null)
                        table.AddCell(item.Leverancier.Naam);
                    if (facturen[0].Prijs != null)
                        table.AddCell(item.Prijs);
                    if ((object)facturen[0].Garantie != null)
                        table.AddCell(item.Garantie.ToString());
                    if (facturen[0].Omschrijving != null)
                        table.AddCell(item.Omschrijving);
                    if (facturen[0].Opmerking != null)
                        table.AddCell(item.Opmerking);
                    if ((object)facturen[0].Afschrijfperiode != null)
                        table.AddCell(item.Afschrijfperiode.ToString());
                    if (facturen[0].DatumInsert != null)
                        table.AddCell(item.DatumInsert);
                    if (facturen[0].UserInsert != null)
                        table.AddCell(item.UserInsert);
                    if (facturen[0].DatumModified != null)
                        table.AddCell(item.DatumModified);
                    if (facturen[0].UserModified != null)
                        table.AddCell(item.UserModified);
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
            DAL.TblFactuur tblFactuur = new DAL.TblFactuur();
            List<FactuurModel> facturen = new List<FactuurModel>();
            facturen = tblFactuur.Rapportering(query, kolomKeuze).ToList();
            for (int i = 0; i < kolomKeuze.Length; i++)
            {
                if (facturen[0].Boekjaar != null)
                    worksheet.Cells[1, 1 + i] = "boekjaar";
                if (facturen[0].CvoVolgNummer != null)
                    worksheet.Cells[1, 1 + i] = "CVO volgnummer";
                if (facturen[0].FactuurNummer != null)
                    worksheet.Cells[1, 1 + i] = "factuurnummer";
                if (facturen[0].ScholengroepNummer != null)
                    worksheet.Cells[1, 1 + i] = "scholengroepnummer";
                if (facturen[0].FactuurDatum != null)
                    worksheet.Cells[1, 1 + i] = "factuurdatum";
                if (facturen[0].Leverancier != null)
                    worksheet.Cells[1, 1 + i] = "leverancier";
                if (facturen[0].Prijs != null)
                    worksheet.Cells[1, 1 + i] = "prijs";
                if ((object)facturen[0].Garantie != null)
                    worksheet.Cells[1, 1 + i] = "garantie";
                if (facturen[0].Omschrijving != null)
                    worksheet.Cells[1, 1 + i] = "omschrijving";
                if (facturen[0].Opmerking != null)
                    worksheet.Cells[1, 1 + i] = "opmerking";
                if ((object)facturen[0].Afschrijfperiode != null)
                    worksheet.Cells[1, 1 + i] = "afschrijfperiode";
                if (facturen[0].DatumInsert != null)
                    worksheet.Cells[1, 1 + i] = "datum insert";
                if (facturen[0].UserInsert != null)
                    worksheet.Cells[1, 1 + i] = "user insert";
                if (facturen[0].DatumModified != null)
                    worksheet.Cells[1, 1 + i] = "datum modified";
                if (facturen[0].UserModified != null)
                    worksheet.Cells[1, 1 + i] = "user modified";
            }
            for (int i = 0; i < facturen.Count; i++)
            {
                //ik werkt met een teller omdat niet alle kolommen getoond worden. dus als er een kolom gegevens heeft gaat de teller met 1 omhoog waardoor het perfect naast elkaar komt
                int teller = 0;
                if (facturen[0].Boekjaar != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].Boekjaar;
                    teller++;
                }
                if (facturen[0].CvoVolgNummer != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].CvoVolgNummer;
                    teller++;
                }
                if (facturen[0].FactuurNummer != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].FactuurNummer;
                    teller++;
                }
                if (facturen[0].ScholengroepNummer != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].ScholengroepNummer;
                    teller++;
                }
                if (facturen[0].FactuurDatum != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].FactuurDatum;
                    teller++;
                }
                if (facturen[0].Leverancier != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].Leverancier.Naam;
                    teller++;
                }
                if (facturen[0].Prijs != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].Prijs;
                    teller++;
                }
                if ((object)facturen[0].Garantie != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].Garantie;
                    teller++;
                }
                if (facturen[0].Omschrijving != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].Omschrijving;
                    teller++;
                }
                if (facturen[0].Opmerking != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].Opmerking;
                    teller++;
                }
                if ((object)facturen[0].Afschrijfperiode != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].Afschrijfperiode;
                    teller++;
                }
                if (facturen[0].DatumInsert != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].DatumInsert;
                    teller++;
                }
                if (facturen[0].UserInsert != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].UserInsert;
                    teller++;
                }
                if (facturen[0].DatumModified != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].DatumModified;
                    teller++;
                }
                if (facturen[0].UserModified != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = facturen[i].UserModified;
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