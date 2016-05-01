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
    public class RapporteringCpuController : Controller
    {
        CvoInventarisServiceClient test = new CvoInventarisServiceClient();
        public ActionResult CpuRapportering()
        {
            ViewBag.styleCpuStap4 = "none";
            ViewBag.styleCpuStap5 = "none";
            return View("CpuRapportering");
        }
        public RapporteringCpuController()
        {

        }
        [HttpPost]
        public ActionResult Stap3Cpu(FormCollection collection)
        {
            string request = Request["action"];
            string[] requestOplossing = request.Split('/');
            if (requestOplossing[0].Equals("OpslaanPdf"))
            {
                OpslaanPdfCpu(requestOplossing[1], requestOplossing[2]);
                ViewBag.testj = requestOplossing[1] + requestOplossing[2];
            }
            if (requestOplossing[0].Equals("OpslaanExcel"))
            {
                OpslaanExcelCpu(requestOplossing[1], requestOplossing[2]);
                ViewBag.testj = requestOplossing[1] + requestOplossing[2];
            }
            if (requestOplossing[0].Equals("kolomKeuze"))
            {
                string resultaat = "";
                bool checkId = Convert.ToBoolean(collection["idCpu"].Split(',')[0]);
                bool checkMerk = Convert.ToBoolean(collection["merk"].Split(',')[0]);
                bool checkType = Convert.ToBoolean(collection["type"].Split(',')[0]);
                bool checkSnelheid = Convert.ToBoolean(collection["snelheid"].Split(',')[0]);
                bool checkFabrieksnummer = Convert.ToBoolean(collection["fabrieksnummer"].Split(',')[0]);
                if (checkId == true)
                    resultaat += "idCpu";
                if (checkMerk == true)
                    resultaat += " merk";
                if (checkType == true)
                    resultaat += " type";
                if (checkSnelheid == true)
                    resultaat += " snelheid";
                if (checkFabrieksnummer == true)
                    resultaat += " fabrieksNummer";
                ViewBag.styleCpuStap4 = "inline";
                ViewBag.resultaatCheck = resultaat;
            }
            if (requestOplossing[0].Equals("conditieKeuze"))
            {
                ViewBag.styleCpuStap5 = "inline";
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
                bool checkMerk = Convert.ToBoolean(collection["merkKeuzeCheck"].Split(',')[0]);
                bool checkType = Convert.ToBoolean(collection["typeKeuzeCheck"].Split(',')[0]);
                bool checkSnelheid = Convert.ToBoolean(collection["snelheidKeuzeCheck"].Split(',')[0]);
                bool checkFabrieksnummer = Convert.ToBoolean(collection["fabrieksnummerKeuzeCheck"].Split(',')[0]);
                if (checkMerk == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("merk", " = " + "'" + collection["merkKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkType == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("type", " = " + "'" + collection["typeKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkSnelheid == true)
                {
                    string resultaat = collection["snelheidKeuze"].Split(',')[0] + " " + collection["snelheidKeuze1"].Split(',')[0];
                    lijstConditie.Add(new KeyValuePair<string, string>("snelheid", " " + resultaat));
                }
                if (checkFabrieksnummer == true)
                {
                    string resultaat = collection["fabrieksnummerKeuze"].Split(',')[0] + " " + collection["fabrieksnummerKeuze1"].Split(',')[0];
                    lijstConditie.Add(new KeyValuePair<string, string>("fabrieksNummer", " " + resultaat));
                }
                string query = "SELECT ";
                foreach (string s in keuzeKolommen)
                {
                    query += s + ", ";
                }
                query = query.Substring(0, query.Length - 2);
                query += " FROM TblCpu ";
                query += " WHERE ";
                foreach (var element in lijstConditie)
                {
                    query += element.Key + element.Value + " AND ";
                }
                query = query.Substring(0, query.Length - 4);
                query += " ;";
                ViewBag.Rapport = test.RapporteringCpu(query, keuzeKolommen.ToArray());

                foreach (string s in keuzeKolommen)
                {
                    ViewBag.tables += s + " ";
                    if (s.Equals("idCpu"))
                        ViewBag.test = s;
                    if (s.Equals("merk"))
                        ViewBag.test1 = s;
                    if (s.Equals("type"))
                        ViewBag.test2 = s;
                    if (s.Equals("snelheid"))
                        ViewBag.test3 = s;
                    if (s.Equals("fabrieksNummer"))
                        ViewBag.test4 = s;
                }
                ViewBag.query = query;
            }
            return View("CpuRapportering");
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

        public void OpslaanExcelCpu(string query, string tables)
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

        public void OpslaanPdfCpu(string query, string tables)
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