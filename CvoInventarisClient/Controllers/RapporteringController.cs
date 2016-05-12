using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
using CvoInventarisClient.ServiceReference;
using CvoInventarisClient.Models;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using CvoInventarisClient.Controllers;
using System.Diagnostics;
using CvoInventarisClient.Controllers.Beheer;

namespace CvoInventarisClient.Controllers
{
    public class RapporteringController : Controller
    {
        public CvoInventarisServiceClient service = new CvoInventarisServiceClient();
        public SelectList keuzeTabellen;
        public List<string> tabellen;

        public List<string> vulTabellen()
        {
            tabellen = new List<string>();
            tabellen.Add("Cpu");
            tabellen.Add("Device");
            tabellen.Add("Factuur");
            tabellen.Add("GrafischeKaart");
            tabellen.Add("Harddisk");
            tabellen.Add("Hardware");
            tabellen.Add("Inventaris");
            tabellen.Add("leverancier");
            tabellen.Add("Lokaal");
            tabellen.Add("Netwerk");
            tabellen.Add("Object");
            tabellen.Add("ObjectType");
            tabellen.Add("Verzekering");
            return tabellen;
        }

        public SelectList vulListKeuzeTabellen()
        {
            keuzeTabellen = new SelectList(vulTabellen());
            return keuzeTabellen;
        }

        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            TabelModel model = new TabelModel();
            ViewBag.tabelKeuzes = vulListKeuzeTabellen();
            ViewBag.stijlStap2 = "hidden";
            ViewBag.hardware = "hidden";
            ViewBag.stijlStapOpslaan = "hidden";
            ViewBag.cpu = "hidden";
            return View(model);
        }

        public ActionResult Stap1(FormCollection f)
        {
            TabelModel model = new TabelModel();
            ViewBag.tabelkeuze = f["tabelkeuzes"];
            ViewBag.tabelKeuzes = vulListKeuzeTabellen();
            ViewBag.stijlStap2 = "inline";
            ViewBag.stijlStapOpslaan = "hidden";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Stap2(string submit)
        {
            //ik split op de submit string. deze submit string is de waarde van de value van de twee submit toppen in stap 2
            string[] tabelKeuze = submit.Split(':');
            //in dit model zit een verzameling van lijsten van alle models. waardoor je in de index pagina van rapportering aan alle models kunt.
            TabelModel model = new TabelModel();
            if (tabelKeuze[0].Trim().Equals("alle gegevens van de tabel"))
            {
                //hier kijkt men wat na de : van de value van de submit top sta. In dit geval is het Cpu. daarna gaat hem de readAll methode doen van de CpuController.tot slot de viewbag inline zetten waardoor het zichtbaar word
                if (tabelKeuze[1].Trim().Equals("Cpu"))
                {
                    //maakt een nieuwe Cpu controller omdat daar al een readAll methode in sta die we kunnen gebruiken voor ons model
                    CpuController cc = new CpuController();
                    model.cpus = cc.GetCpus();
                    ViewBag.cpu = "inline";
                    ViewBag.stijlStapOpslaan = "inline";
                    ViewBag.tabelKeuze = "cpu";
                }
                if (tabelKeuze[1].Trim().Equals("Hardware"))
                {
                    HardwareController hc = new HardwareController();
                    model.hardwares = hc.HardwareGetAll();
                    ViewBag.hardware = "inline";
                    ViewBag.tabelKeuze = "hardware";
                }
                if (tabelKeuze[1].Trim().Equals("Verzekering"))
                {
                    VerzekeringController vc = new VerzekeringController();
                    model.verzekeringen = vc.Getverzekeringen();
                    ViewBag.tabelKeuze = "verzekering";
                }
                if (tabelKeuze[1].Trim().Equals("ObjectType"))
                {
                    ObjectTypeController oc = new ObjectTypeController();
                    model.objectTypes = oc.GetObjectTypes();
                    ViewBag.tabelKeuze = "objectType";
                }

            }
            if (tabelKeuze[0].Trim().Equals("bepaalde conditie van de tabel"))
            {
                if (tabelKeuze[1].Trim().Equals("Cpu"))
                {
                    return Redirect("/RapporteringCpu/CpuRapportering");
                }
                if (tabelKeuze[1].Trim().Equals("Hardware"))
                {
                    return Redirect("/RapporteringHardware/HardwareRapportering");
                }
                if (tabelKeuze[1].Trim().Equals("Verzekering"))
                {
                    TempData["action"] = "Je kan geen conditie toepassen op verzekering. Enkel alle gegevens bekijken";
                    return RedirectToAction("Index");
                }
                if (tabelKeuze[1].Trim().Equals("ObjectType"))
                {
                    TempData["action"] = "Je kan geen conditie toepassen op objectType. Enkel alle gegevens bekijken";
                    return Redirect("/Rapportering/Index");
                }

            }
            ViewBag.tabelKeuzes = vulListKeuzeTabellen();
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult StapOpslaan(string tabelKeuze, string opslaan)
        {
            if (opslaan.Equals("OpslaanExcel"))
            {
                OpslaanExcel(tabelKeuze);
            }
            if (opslaan.Equals("OpslaanPdf"))
            {
                OpslaanPdf(tabelKeuze);
            }
            TabelModel model = new TabelModel();
            ViewBag.tabelKeuzes = vulListKeuzeTabellen();
            ViewBag.stijlStap2 = "hidden";
            ViewBag.hardware = "hidden";
            ViewBag.stijlStapOpslaan = "hidden";
            ViewBag.cpu = "hidden";
            return View("Index", model);
        }
        public void OpslaanPdf(string tabelKeuze)
        {
            try
            {
                //maakt een nieuw Pdf document aan
                Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                //maakt een nieuw pdf writer object aan waar je in u pdf document in ga writen. net zoals een streamwriter
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();

                if (tabelKeuze == "cpu")
                {
                    //maakt een tabel met 5 kolommen
                    PdfPTable table = new PdfPTable(5);
                    //de hoofdingen van de tabellen
                    table.AddCell("id");
                    table.AddCell("merk");
                    table.AddCell("type");
                    table.AddCell("snelheid");
                    table.AddCell("fabrieksnummer");
                    CpuController cc = new CpuController();
                    //loop door het model Cpu en steekt in elke kolom de informatie
                    foreach (var item in cc.GetCpus())
                    {
                        table.AddCell(item.IdCpu.ToString());
                        table.AddCell(item.Merk);
                        table.AddCell(item.Type);
                        table.AddCell(item.Snelheid.ToString());
                        table.AddCell(item.FabrieksNummer.ToString());
                    }
                    pdfDoc.Add(table);
                }

                if (tabelKeuze == "hardware")
                {
                    PdfPTable table = new PdfPTable(5);
                    table.AddCell("id hardware");
                    table.AddCell("cpu");
                    table.AddCell("apparaat");
                    table.AddCell("grafische kaart");
                    table.AddCell("harddisk");
                    HardwareController hc = new HardwareController();
                    foreach (var item in hc.HardwareGetAll())
                    {
                        table.AddCell(item.IdHardware.ToString());
                        table.AddCell(item.Cpu.Merk.ToString());
                        table.AddCell(item.Device.Merk.ToString());
                        table.AddCell(item.GrafischeKaart.Merk.ToString());
                        table.AddCell(item.Harddisk.Merk.ToString());
                    }
                    pdfDoc.Add(table);
                }
                if (tabelKeuze == "verzekering")
                {
                    PdfPTable table = new PdfPTable(2);
                    table.AddCell("id verzekering");
                    table.AddCell("omschrijving");
                    VerzekeringController vz = new VerzekeringController();
                    foreach (var item in vz.Getverzekeringen())
                    {
                        table.AddCell(item.IdVerzekering.ToString());
                        table.AddCell(item.Omschrijving);
                    }
                    pdfDoc.Add(table);
                }
                if (tabelKeuze == "objectType")
                {
                    PdfPTable table = new PdfPTable(2);
                    table.AddCell("id objectType");
                    table.AddCell("omschrijving");
                    ObjectTypeController oc = new ObjectTypeController();
                    foreach (var item in oc.GetObjectTypes())
                    {
                        table.AddCell(item.IdObjectType.ToString());
                        table.AddCell(item.Omschrijving);
                    }
                    pdfDoc.Add(table);
                }
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
            ViewBag.tabelKeuzes = vulListKeuzeTabellen();
        }
        public void OpslaanExcel(string tabelKeuze)
        {
            //maakt een nieuwe excel programma waar je in kunt werken met daaronder een workbook en daarin een worksheet waar u gegevens in komt
            Excel.Application app = new Excel.Application();
            app.Visible = true;
            Excel.Workbook workbook = app.Workbooks.Add(1);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

            if (tabelKeuze.Equals("cpu"))
            {
                CpuController cc = new CpuController();
                List<CpuModel> cpus = cc.GetCpus();
                worksheet.Cells[1, 1] = "id";
                worksheet.Cells[1, 2] = "merk";
                worksheet.Cells[1, 3] = "type";
                worksheet.Cells[1, 4] = "snelheid";
                worksheet.Cells[1, 5] = "fabrieksnummer";
                for (int i = 0; i < cpus.Count; i++)
                {
                    worksheet.Cells[i + 2, 1] = cpus[i].IdCpu;
                    worksheet.Cells[i + 2, 2] = cpus[i].Merk;
                    worksheet.Cells[i + 2, 3] = cpus[i].Type;
                    worksheet.Cells[i + 2, 4] = cpus[i].Snelheid;
                    //de haakjes eronder moet je doen wanneer je een getal hebt. anders ziet hem dat als een getal en word dat niet goed getoond in excel
                    worksheet.Cells[i + 2, 5] = "=\"" + cpus[i].FabrieksNummer.Trim() + "\"";
                }
            }
            if (tabelKeuze.Equals("hardware"))
            {
                HardwareController hc = new HardwareController();
                List<HardwareModel> hm = hc.HardwareGetAll().ToList();
                worksheet.Cells[1, 1] = "id";
                worksheet.Cells[1, 2] = "cpu";
                worksheet.Cells[1, 3] = "apparaat";
                worksheet.Cells[1, 4] = "grafische kaart";
                worksheet.Cells[1, 5] = "harddisk";
                for (int i = 0; i < hm.Count; i++)
                {
                    worksheet.Cells[i + 2, 1] = hm[i].IdHardware;
                    worksheet.Cells[i + 2, 2] = hm[i].Cpu.Merk;
                    worksheet.Cells[i + 2, 3] = hm[i].Device.Merk;
                    worksheet.Cells[i + 2, 4] = hm[i].GrafischeKaart.Merk;
                    worksheet.Cells[i + 2, 5] = hm[i].Harddisk.Merk;
                }
            }
            if (tabelKeuze.Equals("verzekering"))
            {
                VerzekeringController vc = new VerzekeringController();
                List<VerzekeringModel> vm = vc.Getverzekeringen().ToList();
                worksheet.Cells[1, 1] = "id verzekering";
                worksheet.Cells[1, 2] = "omschrijving";
                for (int i = 0; i < vm.Count; i++)
                {
                    worksheet.Cells[i + 2, 1] = vm[i].IdVerzekering;
                    worksheet.Cells[i + 2, 2] = vm[i].Omschrijving;
                }
            }
            if (tabelKeuze.Equals("objectType"))
            {
                ObjectTypeController oc = new ObjectTypeController();
                List<ObjectTypeModel> om = oc.GetObjectTypes().ToList();
                worksheet.Cells[1, 1] = "id objectType";
                worksheet.Cells[1, 2] = "omschrijving";
                for (int i = 0; i < om.Count; i++)
                {
                    worksheet.Cells[i + 2, 1] = om[i].IdObjectType;
                    worksheet.Cells[i + 2, 2] = om[i].Omschrijving;
                }
            }
            worksheet.Columns.AutoFit();
            Response.Buffer = true;
            //dit is het stuk dat zorgt dat je het kunt opslaan op een locatie naar keuze
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Rapport.xls");
            Response.Write(worksheet);
            Response.End();

        }
    }
}