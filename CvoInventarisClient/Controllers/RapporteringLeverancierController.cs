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
    public class RapporteringLeverancierController : Controller
    {
        TabelModel model;
        public void vulDropDownLijstenIn()
        {
            DAL.TblLeverancier leverancierDal = new DAL.TblLeverancier();
            List<LeverancierModel> leverancierModel = leverancierDal.GetAll();
            List<string> namen = new List<string>();
            List<string> afkortingen = new List<string>();
            List<string> straten = new List<string>();
            List<string> huisNummers = new List<string>();
            List<string> busNummers = new List<string>();
            List<int> postcodes = new List<int>();
            List<string> telefoons = new List<string>();
            List<string> faxen = new List<string>();
            List<string> emails = new List<string>();
            List<string> websites = new List<string>();
            List<string> btwNummers = new List<string>();
            List<string> ibans = new List<string>();
            List<string> bics = new List<string>();
            List<string> toegevoegdOps = new List<string>();
            foreach (LeverancierModel lm in leverancierModel)
            {
                namen.Add(lm.Naam);
                afkortingen.Add(lm.Afkorting);
                straten.Add(lm.Straat);
                huisNummers.Add(lm.HuisNummer);
                busNummers.Add(lm.BusNummer);
                postcodes.Add(lm.Postcode);
                telefoons.Add(lm.Telefoon);
                faxen.Add(lm.Fax);
                emails.Add(lm.Email);
                websites.Add(lm.Website);
                btwNummers.Add(lm.BtwNummer);
                ibans.Add(lm.Iban);
                bics.Add(lm.Bic);
                toegevoegdOps.Add(lm.ToegevoegdOp);
            }
            SelectList namenSelectList = new SelectList(namen);
            SelectList afkortingenSelectList = new SelectList(afkortingen);
            SelectList stratenSelectlist = new SelectList(straten);
            SelectList huisNummersSelectList = new SelectList(huisNummers);
            SelectList busNummersSelectList = new SelectList(busNummers);
            SelectList postcodesSelectList = new SelectList(postcodes);
            SelectList telefoonsSelectList = new SelectList(telefoons);
            SelectList faxenSelectList = new SelectList(faxen);
            SelectList emailsSelectList = new SelectList(emails);
            SelectList websitesSelectList = new SelectList(websites);
            SelectList btwNummersSelectList = new SelectList(btwNummers);
            SelectList ibansSelectList = new SelectList(ibans);
            SelectList bicsSelectList = new SelectList(bics);
            SelectList toegevoegdOpsSelectList = new SelectList(toegevoegdOps);
            ViewBag.namen = namenSelectList;
            ViewBag.afkortingen = afkortingenSelectList;
            ViewBag.straten = stratenSelectlist;
            ViewBag.huisNummers = huisNummersSelectList;
            ViewBag.busNummers = busNummersSelectList;
            ViewBag.postcodes = postcodesSelectList;
            ViewBag.telefoons = telefoonsSelectList;
            ViewBag.faxen = faxenSelectList;
            ViewBag.emails = emailsSelectList;
            ViewBag.websites = websitesSelectList;
            ViewBag.btwNummers = btwNummersSelectList;
            ViewBag.ibans = ibansSelectList;
            ViewBag.bics = bicsSelectList;
            ViewBag.toegevoegdOps = toegevoegdOpsSelectList;
        }
        public ActionResult LeverancierRapportering()
        {
            ViewBag.stijlStapOpslaan = "hidden";
            model = new TabelModel();
            vulDropDownLijstenIn();
            return View(model);
        }
        public ActionResult Stap3(FormCollection collection)
        {
            model = new TabelModel();
            DAL.TblLeverancier TblLeverancier = new DAL.TblLeverancier();
            List<LeverancierModel> leveranciers = new List<LeverancierModel>();
            //een tabel van drie omdat je maar drie lijnen nodig hebt. een select statement, u from en u where clausule
            string[] query = new string[4];
            //hier gaat hem nakijken welke checkboxen er aan gevinkt zijn. dus welke tabellen de klant gegevens van wilt hebben
            List<string> kolomKeuze = new List<string>();
            if (Convert.ToBoolean(collection["naam"].Split(',')[0]) == true)
                kolomKeuze.Add("naam");
            if (Convert.ToBoolean(collection["afkorting"].Split(',')[0]) == true)
                kolomKeuze.Add("afkorting");
            if (Convert.ToBoolean(collection["straat"].Split(',')[0]) == true)
                kolomKeuze.Add("straat");
            if (Convert.ToBoolean(collection["huisNummer"].Split(',')[0]) == true)
                kolomKeuze.Add("huisNummer");
            if (Convert.ToBoolean(collection["busNummer"].Split(',')[0]) == true)
                kolomKeuze.Add("busNummer");
            if (Convert.ToBoolean(collection["postcode"].Split(',')[0]) == true)
                kolomKeuze.Add("postcode");
            if (Convert.ToBoolean(collection["telefoon"].Split(',')[0]) == true)
                kolomKeuze.Add("telefoon");
            if (Convert.ToBoolean(collection["fax"].Split(',')[0]) == true)
                kolomKeuze.Add("fax");
            if (Convert.ToBoolean(collection["email"].Split(',')[0]) == true)
                kolomKeuze.Add("email");
            if (Convert.ToBoolean(collection["website"].Split(',')[0]) == true)
                kolomKeuze.Add("website");
            if (Convert.ToBoolean(collection["btwNummer"].Split(',')[0]) == true)
                kolomKeuze.Add("btwNummer");
            if (Convert.ToBoolean(collection["iban"].Split(',')[0]) == true)
                kolomKeuze.Add("iban");
            if (Convert.ToBoolean(collection["bic"].Split(',')[0]) == true)
                kolomKeuze.Add("bic");
            if (Convert.ToBoolean(collection["toegevoegdOp"].Split(',')[0]) == true)
                kolomKeuze.Add("toegevoegdOp");

            List<string> lijstConditie = new List<string>();
            if (collection["namen"].Split(',')[0].Count() != 0)
            {
                string naam = collection["namen"].Split(',')[0];
                lijstConditie.Add("naam = " + "'" + naam + "'");
            }
            if (collection["afkortingen"].Split(',')[0].Count() != 0)
            {
                string straat = collection["afkortingen"].Split(',')[0];
                lijstConditie.Add("afkorting = " + "'" + straat + "'");
            }
            if (collection["straten"].Split(',')[0].Count() != 0)
            {
                string nummer = collection["straten"].Split(',')[0];
                lijstConditie.Add("straat = " + "'" + nummer + "'");
            }
            if (collection["huisNummers"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["huisNummers"].Split(',')[0];
                lijstConditie.Add("huisNummer = " + "'" + postcode + "'");
            }
            if (collection["busNummers"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["busNummers"].Split(',')[0];
                lijstConditie.Add("busNummer = " + "'" + postcode + "'");
            }
            if (collection["postcodes"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["postcodes"].Split(',')[0];
                lijstConditie.Add("postcode = " + "'" + postcode + "'");
            }
            if (collection["telefoons"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["telefoons"].Split(',')[0];
                lijstConditie.Add("telefoon = " + "'" + postcode + "'");
            }
            if (collection["faxen"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["faxen"].Split(',')[0];
                lijstConditie.Add("fax = " + "'" + postcode + "'");
            }
            if (collection["emails"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["emails"].Split(',')[0];
                lijstConditie.Add("email = " + "'" + postcode + "'");
            }
            if (collection["websites"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["websites"].Split(',')[0];
                lijstConditie.Add("website = " + "'" + postcode + "'");
            }
            if (collection["btwNummers"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["btwNummers"].Split(',')[0];
                lijstConditie.Add("btwNummer = " + "'" + postcode + "'");
            }
            if (collection["ibans"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["ibans"].Split(',')[0];
                lijstConditie.Add("iban = " + "'" + postcode + "'");
            }
            if (collection["bics"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["bics"].Split(',')[0];
                lijstConditie.Add("bic = " + "'" + postcode + "'");
            }
            if (collection["toegevoegdOps"].Split(',')[0].Count() != 0)
            {
                string postcode = collection["toegevoegdOps"].Split(',')[0];
                lijstConditie.Add("toegevoegdOp = " + "'" + postcode + "'");
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
            query[1] = " FROM TblLeverancier ";

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
            leveranciers = TblLeverancier.Rapportering(queryResultaat, kolomKeuzeArray).ToList();
            model.leveranciers = leveranciers;
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
            return View("LeverancierRapportering", model);
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
            return View("LeverancierRapportering", model);
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
                DAL.TblLeverancier TblLeverancier = new DAL.TblLeverancier();
                List<LeverancierModel> leveranciers = new List<LeverancierModel>();
                leveranciers = TblLeverancier.Rapportering(query, kolomKeuze).ToList();

                if (leveranciers[0].Naam != null)
                    table.AddCell("naam");                
                if (leveranciers[0].Afkorting != null)
                    table.AddCell("afkorting");
                if (leveranciers[0].Straat != null)
                    table.AddCell("straat");
                if (leveranciers[0].HuisNummer != null)
                    table.AddCell("huis nummer");
                if (leveranciers[0].BusNummer != null)
                    table.AddCell("bus nummer");
                if (leveranciers[0].Postcode != 0)
                    table.AddCell("postcode");
                if (leveranciers[0].Telefoon != null)
                    table.AddCell("telefoon");
                if (leveranciers[0].Fax != null)
                    table.AddCell("fax");
                if (leveranciers[0].Email != null)
                    table.AddCell("email");
                if (leveranciers[0].Website != null)
                    table.AddCell("website");
                if (leveranciers[0].BtwNummer != null)
                    table.AddCell("btw nummer");
                if (leveranciers[0].Iban != null)
                    table.AddCell("iban");
                if (leveranciers[0].Bic != null)
                    table.AddCell("bic");
                if (leveranciers[0].ToegevoegdOp != null)
                    table.AddCell("toegevoegd op");

                foreach (var item in leveranciers)
                {
                    if (leveranciers[0].Naam != null)
                        table.AddCell(item.Naam);                    
                    if (leveranciers[0].Afkorting != null)
                        table.AddCell(item.Afkorting);
                    if (leveranciers[0].Straat != null)
                        table.AddCell(item.Straat);
                    if (leveranciers[0].HuisNummer != null)
                        table.AddCell(item.HuisNummer);
                    if (leveranciers[0].BusNummer != null)
                        table.AddCell(item.BusNummer);
                    if (leveranciers[0].Postcode != 0)
                        table.AddCell(item.Postcode.ToString());
                    if (leveranciers[0].Telefoon != null)
                        table.AddCell(item.Telefoon);
                    if (leveranciers[0].Fax != null)
                        table.AddCell(item.Fax);
                    if (leveranciers[0].Email != null)
                        table.AddCell(item.Email);
                    if (leveranciers[0].Website != null)
                        table.AddCell(item.Website);
                    if (leveranciers[0].BtwNummer != null)
                        table.AddCell(item.BtwNummer);
                    if (leveranciers[0].Iban != null)
                        table.AddCell(item.Iban);
                    if (leveranciers[0].Bic != null)
                        table.AddCell(item.Bic);
                    if (leveranciers[0].ToegevoegdOp != null)
                        table.AddCell(item.ToegevoegdOp);
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
            DAL.TblLeverancier TblLeverancier = new DAL.TblLeverancier();
            List<LeverancierModel> leveranciers = new List<LeverancierModel>();
            leveranciers = TblLeverancier.Rapportering(query, kolomKeuze).ToList();
            for (int i = 0; i < kolomKeuze.Length; i++)
            {
                if (leveranciers[0].Naam != null)
                    worksheet.Cells[1, 1 + i] = "naam";
                if (leveranciers[0].Afkorting != null)
                    worksheet.Cells[1, i + 1] = "afkorting";
                if (leveranciers[0].Straat != null)
                    worksheet.Cells[1, i + 1] = "straat";
                if (leveranciers[0].HuisNummer != null)
                    worksheet.Cells[1, i + 1] = "huis nummer";
                if (leveranciers[0].BusNummer != null)
                    worksheet.Cells[1, i + 1] = "bus nummer";
                if (leveranciers[0].Postcode != 0)
                    worksheet.Cells[1, i + 1] = "postcode";
                if (leveranciers[0].Telefoon != null)
                    worksheet.Cells[1, i + 1] = "telefoon";
                if (leveranciers[0].Fax != null)
                    worksheet.Cells[1, i + 1] = "fax";
                if (leveranciers[0].Email != null)
                    worksheet.Cells[1, i + 1] = "email";
                if (leveranciers[0].Website != null)
                    worksheet.Cells[1, i + 1] = "website";
                if (leveranciers[0].BtwNummer != null)
                    worksheet.Cells[1, i + 1] = "btw nummer";
                if (leveranciers[0].Iban != null)
                    worksheet.Cells[1, i + 1] = "iban";
                if (leveranciers[0].Bic != null)
                    worksheet.Cells[1, i + 1] = "bic";
                if (leveranciers[0].ToegevoegdOp != null)
                    worksheet.Cells[1, i + 1] = "toegevoegd op";
            }
            for (int i = 0; i < leveranciers.Count; i++)
            {
                //ik werkt met een teller omdat niet alle kolommen getoond worden. dus als er een kolom gegevens heeft gaat de teller met 1 omhoog waardoor het perfect naast elkaar komt
                int teller = 0;
                if (leveranciers[0].Naam != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].Naam;
                    teller++;
                }
                if (leveranciers[0].Afkorting != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].Afkorting;
                    teller++;
                }
                if (leveranciers[0].Straat != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].Straat;
                    teller++;
                }
                if (leveranciers[0].HuisNummer != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].HuisNummer;
                    teller++;
                }
                if (leveranciers[0].BusNummer != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].BusNummer;
                    teller++;
                }
                if (leveranciers[0].Postcode != 0)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].Postcode;
                    teller++;
                }
                if (leveranciers[0].Telefoon != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].Telefoon;
                    teller++;
                }
                if (leveranciers[0].Fax != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].Fax;
                    teller++;
                }
                if (leveranciers[0].Email != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].Email;
                    teller++;
                }
                if (leveranciers[0].Website != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].Website;
                    teller++;
                }
                if (leveranciers[0].BtwNummer != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].BtwNummer;
                    teller++;
                }
                if (leveranciers[0].Iban != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].Iban;
                    teller++;
                }
                if (leveranciers[0].Bic != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].Bic;
                    teller++;
                }
                if (leveranciers[0].ToegevoegdOp != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = leveranciers[0].ToegevoegdOp;
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