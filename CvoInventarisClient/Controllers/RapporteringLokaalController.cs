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

        public SelectList lokaalNamen;
        public SelectList netwerkmerken;

        public SelectList vulLokaalNamen()
        {
            List<Lokaal> lokalen = test.LokaalGetAll().ToList();
            lokaalNamen = new SelectList(lokalen, "IdLokaal", "LokaalNaam");
            return lokaalNamen;
        }
        public SelectList vulNetwerkmerken()
        {
            List<Netwerk> netwerken = test.NetwerkGetAll().ToList();
            netwerkmerken = new SelectList(netwerken, "Id", "Merk");
            return netwerkmerken;
        }

        public ActionResult LokaalRapportering()
        {
            ViewBag.styleLokaalStap4 = "none";
            ViewBag.styleLokaalStap5 = "none";
            ViewBag.lokalen = vulLokaalNamen();
            ViewBag.netwerken = vulNetwerkmerken();

            return View("LokaalRapportering");
        }
        [HttpPost]
        public ActionResult Stap3Lokaal(FormCollection collection)
        {
            string request = Request["action"];
            string[] requestOplossing = request.Split('/');
            if (requestOplossing[0].Equals("OpslaanPdf"))
            {

                OpslaanPdfLokaal(requestOplossing[1], requestOplossing[2]);

                OpslaanExcelLokaal(requestOplossing[1], requestOplossing[2]);

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
                bool checkId = Convert.ToBoolean(collection["TblLokaal.idLokaal"].Split(',')[0]);
                bool checkLokaalNaam = Convert.ToBoolean(collection["TblLokaal.lokaalNaam"].Split(',')[0]);
                bool checkAantalPlaatsen = Convert.ToBoolean(collection["TblLokaal.aantalPlaatsen"].Split(',')[0]);
                bool checkIsComputerLokaal = Convert.ToBoolean(collection["TblLokaal.isComputerLokaal"].Split(',')[0]);
                bool checkNetwerk = Convert.ToBoolean(collection["TblNetwerk.merk"].Split(',')[0]);

                if (checkId == true)
                    resultaat += "TblLokaal.idLokaal";
                if (checkLokaalNaam == true)
                    resultaat += " TblLokaal.lokaalNaam";
                if (checkAantalPlaatsen == true)
                    resultaat += " TblLokaal.aantalPlaatsen";
                if (checkIsComputerLokaal == true)
                    resultaat += " TblLokaal.isComputerLokaal";
                if (checkNetwerk == true)
                    resultaat += " TblNetwerk.merk";

                ViewBag.styleFactuurStap4 = "inline";
                ViewBag.lokalen = vulLokaalNamen();
                ViewBag.netwerken = vulNetwerkmerken();
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
                bool checkLokaalNaam = Convert.ToBoolean(collection["lokaalNaamKeuzeCheck"].Split(',')[0]);
                bool checkAantalPlaatsen = Convert.ToBoolean(collection["aantalPlaatsenKeuzeCheck"].Split(',')[0]);
                bool checkIsComputerLokaal = Convert.ToBoolean(collection["isComputerLokaalKeuzeCheck"].Split(',')[0]);
                bool checkNetwerkMerk = Convert.ToBoolean(collection["netwerkMerkKeuzeCheck"].Split(',')[0]);


                if (checkLokaalNaam == true)
                {
                    int id = Int32.Parse(collection["lokalen"].Split(',')[0]);
                    string l = test.LokaalGetById(id).LokaalNaam;
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblLokaal\".\"LokaalNaam\"", " = " + "'" + l + "'"));
                }

                if (checkAantalPlaatsen == true)
                {
                    string resultaat = collection["aantalPlaatsenKeuze"].Split(',')[0] + " " + collection["aantalPlaatsenKeuze1"].Split(',')[0];
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblLokaal\".\"aantalPlaatsen\"", " " + resultaat));
                }
                if (checkIsComputerLokaal == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblLokaal\".\"isComputerLokaal\"", " = " + "'" + collection["isComputerLokaalKeuze"].Split(',')[0] + "'"));
                }
                if (checkNetwerkMerk == true)
                {
                    int id = Int32.Parse(collection["netwerken"].Split(',')[0]);
                    string l = test.NetwerkGetById(id).Merk;
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblNetwerk\".\"merk\"", " = " + "'" + l + "'"));
                }


                string query = "SELECT ";
                foreach (string s in keuzeKolommen)
                {
                    query += s + ", ";
                }
                query = query.Substring(0, query.Length - 2);
                query += " FROM TblLokaal ";
                if (checkNetwerkMerk == true)
                {
                    query += "INNER JOIN TblNetwerk ON TblLokaal.idNetwerk=TblNetwerk.idNetwerk";
                }
                query += " WHERE ";
                foreach (var element in lijstConditie)
                {
                    query += element.Key + element.Value + " AND ";
                }
                query = query.Substring(0, query.Length - 4);
                query += " ;";
                ViewBag.Rapport = test.RapporteringLokaal(query, keuzeKolommen.ToArray());

                foreach (string s in keuzeKolommen)
                {
                    ViewBag.tables += s + " ";
                    if (s.Equals("TblLokaal.idLokaal"))
                        ViewBag.test = s;
                    if (s.Equals("TblLokaal.lokaalNaam"))
                        ViewBag.test1 = s;
                    if (s.Equals("TblLokaal.aantalPlaatsen"))
                        ViewBag.test2 = s;
                    if (s.Equals("TblLokaal.isComputerLokaal"))
                        ViewBag.test3 = s;
                    if (s.Equals("TblNetwerk.merk"))
                        ViewBag.test4 = s;
                }
                ViewBag.query = query;
                ViewBag.lokalen = vulLokaalNamen();
                ViewBag.netwerken = vulNetwerkmerken();
            }
            return View("LokaalRapportering");
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