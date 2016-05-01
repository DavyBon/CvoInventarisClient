using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CvoInventarisClient.Controllers
{
    public class RapporteringLokaalController : Controller
    {
        CvoInventarisServiceClient test = new CvoInventarisServiceClient();
        public ActionResult LokaalRapportering()
        {
            ViewBag.styleLokaalStap4 = "none";
            ViewBag.styleLokaalStap5 = "none";
            return View("FactuurRapportering");
        }
        [HttpPost]
        public ActionResult Stap3Lokaal(FormCollection collection)
        {
            string request = Request["action"];
            string[] requestOplossing = request.Split('/');
            if (requestOplossing[0].Equals("OpslaanPdf"))
            {
                OpslaanLokaalCpu(requestOplossing[1], requestOplossing[2]);
                ViewBag.testj = requestOplossing[1] + requestOplossing[2];
            }
            if (requestOplossing[0].Equals("OpslaanExcel"))
            {
                OpslaanExcelLokaal(requestOplossing[1], requestOplossing[2]);
                ViewBag.testj = requestOplossing[1] + requestOplossing[2];
            }
            if (requestOplossing[0].Equals("kolomKeuze"))
            {
                string resultaat = "";
                bool checkId = Convert.ToBoolean(collection["TblFactuur.idFactuur"].Split(',')[0]);
                bool checkBoekjaar = Convert.ToBoolean(collection["TblFactuur.Boekjaar"].Split(',')[0]);
                bool checkCvoVolgNummer = Convert.ToBoolean(collection["TblFactuur.CvoVolgNummer"].Split(',')[0]);
                bool checkFactuurNummer = Convert.ToBoolean(collection["TblFactuur.FactuurNummer"].Split(',')[0]);
                bool checkFactuurDatum = Convert.ToBoolean(collection["TblFactuur.FactuurDatum"].Split(',')[0]);
                bool checkFactuurStatusGetekend = Convert.ToBoolean(collection["TblFactuur.FactuurStatusGetekend"].Split(',')[0]);
                bool checkVerwerkingsDatum = Convert.ToBoolean(collection["TblFactuur.VerwerkingsDatum"].Split(',')[0]);
                bool checkLeverancierNaam = Convert.ToBoolean(collection["TblLeverancier.naam"].Split(',')[0]);
                bool checkPrijs = Convert.ToBoolean(collection["TblFactuur.Prijs"].Split(',')[0]);
                bool checkGarantie = Convert.ToBoolean(collection["TblFactuur.Garantie"].Split(',')[0]);
                bool checkOmschrijving = Convert.ToBoolean(collection["TblFactuur.Omschrijving"].Split(',')[0]);
                bool checkOpmerking = Convert.ToBoolean(collection["TblFactuur.Opmerking"].Split(',')[0]);
                bool checkAfschrijfperiode = Convert.ToBoolean(collection["TblFactuur.Afschrijfperiode"].Split(',')[0]);
                bool checkOleDoc = Convert.ToBoolean(collection["TblFactuur.OleDoc"].Split(',')[0]);
                bool checkOleDocPath = Convert.ToBoolean(collection["TblFactuur.OleDocPath"].Split(',')[0]);
                bool checkOleDocFileName = Convert.ToBoolean(collection["TblFactuur.OleDocFileName"].Split(',')[0]);
                bool checkDatumInsert = Convert.ToBoolean(collection["TblFactuur.DatumInsert"].Split(',')[0]);
                bool checkUserInsert = Convert.ToBoolean(collection["TblFactuur.UserInsert"].Split(',')[0]);
                bool checkDatumModified = Convert.ToBoolean(collection["TblFactuur.DatumModified"].Split(',')[0]);
                bool checkUserModified = Convert.ToBoolean(collection["TblFactuur.UserModified"].Split(',')[0]);

                if (checkId == true)
                    resultaat += "TblFactuur.idFactuur";
                if (checkBoekjaar == true)
                    resultaat += " TblFactuur.Boekjaar";
                if (checkCvoVolgNummer == true)
                    resultaat += " TblFactuur.CvoVolgNummer";
                if (checkFactuurNummer == true)
                    resultaat += " TblFactuur.FactuurNummer";
                if (checkFactuurDatum == true)
                    resultaat += " TblFactuur.FactuurDatum";
                if (checkFactuurStatusGetekend == true)
                    resultaat += " TblFactuur.FactuurStatusGetekend";
                if (checkVerwerkingsDatum == true)
                    resultaat += " TblFactuur.VerwerkingsDatum";
                if (checkLeverancierNaam == true)
                    resultaat += " TblLeverancier.naam";
                if (checkPrijs == true)
                    resultaat += " TblFactuur.Prijs";
                if (checkGarantie == true)
                    resultaat += " TblFactuur.Garantie";
                if (checkOmschrijving == true)
                    resultaat += " TblFactuur.Omschrijving";
                if (checkOpmerking == true)
                    resultaat += " TblFactuur.Opmerking";
                if (checkAfschrijfperiode == true)
                    resultaat += " TblFactuur.Afschrijfperiode";
                if (checkOleDoc == true)
                    resultaat += " TblFactuur.TblFactuur.OleDoc";
                if (checkOleDocPath == true)
                    resultaat += " TblFactuur.OleDocPath";
                if (checkOleDocFileName == true)
                    resultaat += " TblFactuur.OleDocFileName";
                if (checkDatumInsert == true)
                    resultaat += " TblFactuur.DatumInsert";
                if (checkUserInsert == true)
                    resultaat += " TblFactuur.UserInsert";
                if (checkDatumModified == true)
                    resultaat += " TblFactuur.DatumModified";
                if (checkUserModified == true)
                    resultaat += " TblFactuur.UserModified";

                ViewBag.styleFactuurStap4 = "inline";
                ViewBag.resultaatCheck = resultaat;
            }
            if (requestOplossing[0].Equals("conditieKeuze"))
            {
                ViewBag.styleFactuurStap5 = "inline";
                string keuzeKolom = requestOplossing[1];
                string[] tussen = keuzeKolom.Split(' ');
                List<string> keuzeKolommen = new List<string>();
                foreach (string s in tussen)
                {
                    if (!s.Equals(""))
                    {
                        keuzeKolommen.Add(s);
                    }
                }

                var lijstConditie = new List<KeyValuePair<string, string>>();
                bool checkBoekjaar = Convert.ToBoolean(collection["boekjaarKeuzeCheck"].Split(',')[0]);
                bool checkCvoVolgNummer = Convert.ToBoolean(collection["cvoVolgNummerKeuzeCheck"].Split(',')[0]);
                bool checkFactuurNummer = Convert.ToBoolean(collection["factuurnummerKeuzeCheck"].Split(',')[0]);
                bool checkFactuurDatum = Convert.ToBoolean(collection["factuurDatumKeuzeCheck"].Split(',')[0]);
                bool checkFactuurStatusGetekend = Convert.ToBoolean(collection["factuurStatusGetekendKeuzeCheck"].Split(',')[0]);
                bool checkVerwerkingsDatum = Convert.ToBoolean(collection["verwerkingsdatumKeuzeCheck"].Split(',')[0]);
                bool checkLeverancierNaam = Convert.ToBoolean(collection["leverancierNaamKeuzeCheck"].Split(',')[0]);
                bool checkPrijs = Convert.ToBoolean(collection["prijsKeuzeCheck"].Split(',')[0]);
                bool checkGarantie = Convert.ToBoolean(collection["garantieKeuzeCheck"].Split(',')[0]);
                bool checkOmschrijving = Convert.ToBoolean(collection["omschrijvingKeuzeCheck"].Split(',')[0]);
                bool checkOpmerking = Convert.ToBoolean(collection["opmerkingKeuzeCheck"].Split(',')[0]);
                bool checkAfschrijfperiode = Convert.ToBoolean(collection["afschrijfperiodeKeuzeCheck"].Split(',')[0]);
                bool checkOleDoc = Convert.ToBoolean(collection["oleDocKeuzeCheck"].Split(',')[0]);
                bool checkOleDocPath = Convert.ToBoolean(collection["oleDocPathKeuzeCheck"].Split(',')[0]);
                bool checkOleDocFileName = Convert.ToBoolean(collection["oleDocFileNameKeuzeCheck"].Split(',')[0]);
                bool checkDatumInsert = Convert.ToBoolean(collection["datumInsertKeuzeCheck"].Split(',')[0]);
                bool checkUserInsert = Convert.ToBoolean(collection["userInsertKeuzeCheck"].Split(',')[0]);
                bool checkDatumModified = Convert.ToBoolean(collection["datumModifiedKeuzeCheck"].Split(',')[0]);
                bool checkUserModified = Convert.ToBoolean(collection["userModifiedKeuzeCheck"].Split(',')[0]);

                if (checkBoekjaar == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"Boekjaar\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkCvoVolgNummer == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"CvoVolgNummer\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkFactuurNummer == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"FactuurNummer\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkFactuurDatum == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"FactuurDatum\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkFactuurStatusGetekend == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"FactuurStatusGetekend\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkVerwerkingsDatum == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"VerwerkingsDatum\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkPrijs == true)
                {
                    string resultaat = collection["prijsKeuze"].Split(',')[0] + " " + collection["prijsKeuze1"].Split(',')[0];
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"Prijs\"", " " + resultaat));
                }
                if (checkGarantie == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"Garantie\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkOmschrijving == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"Omschrijving\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkOpmerking == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"Opmerking\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkAfschrijfperiode == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"Afschrijfperiode\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkOleDoc == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"OleDoc\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkOleDocPath == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"OleDocPath\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkOleDocFileName == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"OleDocFileName\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkDatumInsert == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"DatumInsert\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkUserInsert == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"UserInsert\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkDatumModified == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"DatumModified\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));
                if (checkUserModified == true)
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblFactuur\".\"UserModified\"", " = " + "'" + collection["boekjaarKeuzeTekst"].Split(',')[0] + "'"));

                string query = "SELECT ";
                foreach (string s in keuzeKolommen)
                {
                    query += s + ", ";
                }
                query = query.Substring(0, query.Length - 2);
                query += " FROM TblFactuur ";
                if (checkLeverancierNaam == true)
                {

                }
                query += " WHERE ";
                foreach (var element in lijstConditie)
                {
                    query += element.Key + element.Value + " AND ";
                }
                query = query.Substring(0, query.Length - 4);
                query += " ;";
                ViewBag.Rapport = test.RapporteringFactuur(query, keuzeKolommen.ToArray());

                foreach (string s in keuzeKolommen)
                {
                    ViewBag.tables += s + " ";
                    if (s.Equals("TblFactuur.idFactuur"))
                        ViewBag.test = s;
                    if (s.Equals("TblFactuur.Boekjaar"))
                        ViewBag.test1 = s;
                    if (s.Equals("TblFactuur.CvoVolgNummer"))
                        ViewBag.test2 = s;
                    if (s.Equals("TblFactuur.FactuurNummer"))
                        ViewBag.test3 = s;
                    if (s.Equals("TblFactuur.FactuurDatum"))
                        ViewBag.test4 = s;
                    if (s.Equals("TblFactuur.FactuurStatusGetekend"))
                        ViewBag.test5 = s;
                    if (s.Equals("TblFactuur.VerwerkingsDatum"))
                        ViewBag.test6 = s;
                    if (s.Equals("TblLeverancier.naam"))
                        ViewBag.test7 = s;
                    if (s.Equals("TblFactuur.Prijs"))
                        ViewBag.test8 = s;
                    if (s.Equals("TblFactuur.Garantie"))
                        ViewBag.test9 = s;
                    if (s.Equals("TblFactuur.Omschrijving"))
                        ViewBag.test10 = s;
                    if (s.Equals("TblFactuur.Opmerking"))
                        ViewBag.test11 = s;
                    if (s.Equals("TblFactuur.Afschrijfperiode"))
                        ViewBag.test12 = s;
                    if (s.Equals("TblFactuur.TblFactuur.OleDoc"))
                        ViewBag.test13 = s;
                    if (s.Equals("TblFactuur.OleDocFileName"))
                        ViewBag.test14 = s;
                    if (s.Equals("TblFactuur.DatumInsert"))
                        ViewBag.test15 = s;
                    if (s.Equals("TblFactuur.UserInsert"))
                        ViewBag.test16 = s;
                    if (s.Equals("TblFactuur.DatumModified"))
                        ViewBag.test17 = s;
                    if (s.Equals("TblFactuur.UserModified"))
                        ViewBag.test18 = s;
                }
                ViewBag.query = query;
            }
            return View("FactuurRapportering");
        }
        public void toExcel(GridView grid)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=rapport.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw))
                {
                    grid.AllowSorting = false;
                    grid.DataBind();
                    grid.RenderControl(htw);
                    Response.Write(sw.ToString());
                }
            }

            Response.End();
        }

        public void OpslaanExcelLokaal(string query, string tables)
        {
            GridView grid = new GridView();
            string[] tablesOplossingVoorlopig = tables.Split(' ');
            string[] tabllesOplossing = new string[tablesOplossingVoorlopig.Count()];
            int teller = 0;
            foreach (string s in tablesOplossingVoorlopig)
            {
                if (!s.Equals(" "))
                {
                    tabllesOplossing[teller] = s; teller++;
                }
            }
            tabllesOplossing = tablesOplossingVoorlopig.Take(tablesOplossingVoorlopig.Count() - 1).ToArray();
            List<Cpu> cpuRapport = test.RapporteringCpu(query, tabllesOplossing).ToList();
            List<CpuModel> modellen = new List<CpuModel>();
            foreach (Cpu c in cpuRapport)
            {
                CpuModel model = new CpuModel();
                if (c.IdCpu != 0)
                {
                    model.IdCpu = c.IdCpu;
                }
                if (c.Merk != null)
                {
                    model.Merk = c.Merk;
                }
                if (c.Type != null)
                {
                    model.Type = c.Type;
                }
                if (c.Snelheid != 0)
                {
                    model.Snelheid = c.Snelheid;
                }
                if (c.FabrieksNummer != null)
                {
                    model.FabrieksNummer = c.FabrieksNummer;
                }
                modellen.Add(model);
            }
            grid.DataSource = modellen;
            toExcel(grid);
        }

        public void OpslaanPdfLokaal(string query, string tables)
        {
            try
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                string[] tablesOplossingVoorlopig = tables.Split(' ');
                string[] tabllesOplossing = new string[tablesOplossingVoorlopig.Count()];
                int teller = 0;
                foreach (string s in tablesOplossingVoorlopig)
                {
                    if (!s.Equals(" "))
                    {
                        tabllesOplossing[teller] = s; teller++;
                    }
                }
                tabllesOplossing = tablesOplossingVoorlopig.Take(tablesOplossingVoorlopig.Count() - 1).ToArray();

                PdfPTable table = new PdfPTable(tabllesOplossing.Length);
                List<Cpu> cpuRapport = test.RapporteringCpu(query, tabllesOplossing).ToList();
                foreach (string s in tabllesOplossing)
                {
                    if (s.Equals("idCpu"))
                        table.AddCell(s);
                    if (s.Equals("merk"))
                        table.AddCell(s);
                    if (s.Equals("type"))
                        table.AddCell(s);
                    if (s.Equals("snelheid"))
                        table.AddCell(s);
                    if (s.Equals("fabrieksNummer"))
                        table.AddCell(s);
                }
                foreach (Cpu c in cpuRapport)
                {
                    if (c.IdCpu != 0)
                    {
                        table.AddCell(c.IdCpu.ToString());
                    }
                    if (c.Merk != null)
                    {
                        table.AddCell(c.Merk);
                    }
                    if (c.Type != null)
                    {
                        table.AddCell(c.Type);
                    }
                    if (c.Snelheid != 0)
                    {
                        table.AddCell(c.Snelheid.ToString());
                    }
                    if (c.FabrieksNummer != null)
                    {
                        table.AddCell(c.FabrieksNummer);
                    }
                }
                pdfDoc.Add(table);
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Example.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }
        }
    }
}