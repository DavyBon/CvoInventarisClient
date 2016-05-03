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
    public class RapporteringHardwareController : Controller
    {
        CvoInventarisServiceClient test = new CvoInventarisServiceClient();

        public SelectList cpuMerken;
        public SelectList deviceMerken;
        public SelectList grafischeKaartenMerken;
        public SelectList harddiskkMerken;
        //public SelectList lokaalNamen;
        //public SelectList netwerkmerken;

        //public SelectList vulLokaalNamen()
        //{
        //    List<Lokaal> lokalen = test.LokaalGetAll().ToList();
        //    lokaalNamen = new SelectList(lokalen, "IdLokaal", "LokaalNaam");
        //    return lokaalNamen;
        //}
        //public SelectList vulNetwerkmerken()
        //{
        //    List<Netwerk> netwerken = test.NetwerkGetAll().ToList();
        //    netwerkmerken = new SelectList(netwerken, "Id", "Merk");
        //    return netwerkmerken;
        //}

        public SelectList vulCpuMerken()
        {
            List<Cpu> cpus = test.CpuGetAll().ToList();
            cpuMerken = new SelectList(cpus, "idCpu", "merk");
            return cpuMerken;
        }
        public SelectList vulDeviceMerken()
        {
            List<Device> devices = test.DeviceGetAll().ToList();
            deviceMerken = new SelectList(devices, "idDevice", "merk");
            return deviceMerken;
        }
        public SelectList vulGrafischeKaartMerken()
        {
            List<GrafischeKaart> grafischeKaarten = test.GrafischeKaartGetAll().ToList();
            grafischeKaartenMerken = new SelectList(grafischeKaarten, "idGrafischeKaart", "merk");
            return grafischeKaartenMerken;
        }
        public SelectList vulHarddiskMerken()
        {
            List<Harddisk> harddisks = test.HarddiskGetAll().ToList();
            harddiskkMerken = new SelectList(harddisks, "idHarddisk", "merk");
            return harddiskkMerken;
        }

        public ActionResult HardwareRapportering()
        {
            ViewBag.styleLokaalStap4 = "none";
            ViewBag.styleLokaalStap5 = "none";
            ViewBag.cpus = vulCpuMerken();
            ViewBag.devices = vulDeviceMerken();
            ViewBag.grafischeKaarten = vulGrafischeKaartMerken();
            ViewBag.harddisks = vulHarddiskMerken();
            //ViewBag.lokalen = vulLokaalNamen();
            //ViewBag.netwerken = vulNetwerkmerken();

            return View("HardwareRapportering");
        }
        [HttpPost]
        public ActionResult Stap3Hardware(FormCollection collection)
        {
            string request = Request["action"];
            string[] requestOplossing = request.Split('/');
            if (requestOplossing[0].Equals("OpslaanPdf"))
            {
                OpslaanPdfHardware(requestOplossing[1], requestOplossing[2]);

                ViewBag.testj = requestOplossing[1] + requestOplossing[2];
            }
            if (requestOplossing[0].Equals("OpslaanExcel"))
            {
                OpslaanExcelHardware(requestOplossing[1], requestOplossing[2]);
                ViewBag.testj = requestOplossing[1] + requestOplossing[2];
            }
            if (requestOplossing[0].Equals("kolomKeuze"))
            {
                string resultaat = "";
                bool checkId = Convert.ToBoolean(collection["TblHardware.idHardware"].Split(',')[0]);
                bool checkLokaalNaam = Convert.ToBoolean(collection["TblCpu.merk"].Split(',')[0]);
                bool checkAantalPlaatsen = Convert.ToBoolean(collection["TblDevice.merk"].Split(',')[0]);
                bool checkIsComputerLokaal = Convert.ToBoolean(collection["TblGrafischeKaart.merk"].Split(',')[0]);
                bool checkNetwerk = Convert.ToBoolean(collection["TblHarddisk.merk"].Split(',')[0]);

                if (checkId == true)
                    resultaat += "TblHardware.idHardware";
                if (checkLokaalNaam == true)
                    resultaat += " TblCpu.merk cm";
                if (checkAantalPlaatsen == true)
                    resultaat += " TblDevice.merk dm";
                if (checkIsComputerLokaal == true)
                    resultaat += " TblGrafischeKaart.merk gm";
                if (checkNetwerk == true)
                    resultaat += " TblHarddisk.merk hm";

                ViewBag.styleFactuurStap4 = "inline";
                ViewBag.cpus = vulCpuMerken();
                ViewBag.devices = vulDeviceMerken();
                ViewBag.grafischeKaarten = vulGrafischeKaartMerken();
                ViewBag.harddisks = vulHarddiskMerken();
                //ViewBag.lokalen = vulLokaalNamen();
                //ViewBag.netwerken = vulNetwerkmerken();
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
                bool checkCpuMerk = Convert.ToBoolean(collection["cpuMerkKeuzeCheck"].Split(',')[0]);
                bool checkDeviceMerk = Convert.ToBoolean(collection["deviceMerkKeuzeCheck"].Split(',')[0]);
                bool checkGrafischeKaart = Convert.ToBoolean(collection["grafischeKaartKeuzeCheck"].Split(',')[0]);
                bool checkHardiskMerk = Convert.ToBoolean(collection["harddiskMerkKeuzeCheck"].Split(',')[0]);


                if (checkCpuMerk == true)
                {
                    int id = Int32.Parse(collection["cpus"].Split(',')[0]);
                    string l = test.CpuGetById(id).Merk;
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblCpu\".\"merk\"", " = " + "'" + l + "'"));
                }

                if (checkDeviceMerk == true)
                {
                    int id = Int32.Parse(collection["devices"].Split(',')[0]);
                    string l = test.DeviceGetById(id).Merk;
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblDevice\".\"merk\"", " = " + "'" + l + "'"));
                }
                if (checkGrafischeKaart == true)
                {
                    int id = Int32.Parse(collection["grafischeKaarten"].Split(',')[0]);
                    string l = test.GrafischeKaartGetById(id).Merk;
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblGrafischeKaart\".\"merk\"", " = " + "'" + l + "'"));
                }
                if (checkHardiskMerk == true)
                {
                    int id = Int32.Parse(collection["harddisks"].Split(',')[0]);
                    string l = test.HarddiskGetById(id).Merk;
                    lijstConditie.Add(new KeyValuePair<string, string>("\"TblHarddisk\".\"merk\"", " = " + "'" + l + "'"));
                }


                string query = "SELECT ";
                foreach (string s in keuzeKolommen)
                {
                    query += s + ", ";
                }
                query = query.Substring(0, query.Length - 2);
                query += " FROM TblHardware ";
                if (checkCpuMerk == true)
                {
                    query += "INNER JOIN TblCpu ON TblHardware.idCpu=TblCpu.idCpu";
                }

                if (checkDeviceMerk == true)
                {
                    query += "INNER JOIN TblDevice ON TblHardware.idDevice=TblDevice.idDevice";
                }
                if (checkGrafischeKaart == true)
                {
                    query += "INNER JOIN TblGrafischeKaart ON TblHardware.idGrafischeKaart=TblGrafischeKaart.idGrafischeKaart";
                }
                if (checkHardiskMerk == true)
                {
                    query += "INNER JOIN TblHarddisk ON TblHardware.idHarddisk=TblHarddisk.idHarddisk";
                }
                query += " WHERE ";
                foreach (var element in lijstConditie)
                {
                    query += element.Key + element.Value + " AND ";
                }
                query = query.Substring(0, query.Length - 4);
                query += " ;";
                ViewBag.Rapport = test.RapporteringHardware(query, keuzeKolommen.ToArray());

                foreach (string s in keuzeKolommen)
                {
                    ViewBag.tables += s + " ";
                    if (s.Equals("TblHardware.idHardware"))
                        ViewBag.test = s;
                    if (s.Equals("TblCpu.merk cm"))
                        ViewBag.test1 = s;
                    if (s.Equals("TblDevice.merk dm"))
                        ViewBag.test2 = s;
                    if (s.Equals("TblGrafischeKaart.merk gm"))
                        ViewBag.test3 = s;
                    if (s.Equals("TblHarddisk.merk hm"))
                        ViewBag.test4 = s;
                }
                ViewBag.query = query;
                ViewBag.cpus = vulCpuMerken();
                ViewBag.devices = vulDeviceMerken();
                ViewBag.grafischeKaarten = vulGrafischeKaartMerken();
                ViewBag.harddisks = vulHarddiskMerken();
                //ViewBag.lokalen = vulLokaalNamen();
                //ViewBag.netwerken = vulNetwerkmerken();
            }
            return View("HardwareRapportering");
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

        public void OpslaanExcelHardware(string query, string tables)
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

        public void OpslaanPdfHardware(string query, string tables)
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