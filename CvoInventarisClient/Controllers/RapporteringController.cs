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
            tabellen.Add("Factuur");
            tabellen.Add("Cpu");
            tabellen.Add("Inventaris");
            tabellen.Add("leverancier");
            tabellen.Add("Campus");
            tabellen.Add("campus");
            tabellen.Add("lokaal");
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
                    ViewBag.tabelKeuze = "Cpu";
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
                    DAL.TblVerzekering verzekering = new DAL.TblVerzekering();
                    model.verzekeringen = verzekering.GetAll();
                    ViewBag.tabelKeuze = "verzekering";
                }
                if (tabelKeuze[1].Trim().Equals("ObjectType"))
                {
                    DAL.TblObjectType objectType = new DAL.TblObjectType();
                    model.objectTypes = objectType.GetAll();
                    ViewBag.tabelKeuze = "objectType";
                }
                if (tabelKeuze[1].Trim().Equals("Campus"))
                {
                    DAL.TblCampus campus = new DAL.TblCampus();
                    model.campussen = campus.GetAll();
                    ViewBag.tabelKeuze = "campus";
                }
                if (tabelKeuze[1].Trim().Equals("Lokaal"))
                {
                    DAL.TblLokaal lokalen = new DAL.TblLokaal();
                    model.lokalen = lokalen.GetAll();
                    ViewBag.tabelKeuze = "lokalen";
                }
                if (tabelKeuze[1].Trim().Equals("Factuur"))
                {
                    DAL.TblFactuur factuur = new DAL.TblFactuur();
                    model.facturen = factuur.GetAll();
                    ViewBag.tabelKeuze = "factuur";
                }
                if (tabelKeuze[1].Trim().Equals("Leverancier"))
                {
                    DAL.TblLeverancier leverancier = new DAL.TblLeverancier();
                    model.leveranciers = leverancier.GetAll();
                    ViewBag.tabelKeuze = "leverancier";
                }
                if (tabelKeuze[1].Trim().Equals("Object"))
                {
                    DAL.TblObject Object = new DAL.TblObject();
                    model.objecten = Object.GetAll();
                    ViewBag.tabelKeuze = "object";
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
                if (tabelKeuze[1].Trim().Equals("Campus"))
                {
                    return Redirect("/RapporteringCampus/CampusRapportering");
                }
                if (tabelKeuze[1].Trim().Equals("Lokalen"))
                {
                    return Redirect("/RapporteringLokalen/LokalenRapportering");
                }
                if (tabelKeuze[1].Trim().Equals("Factuur"))
                {
                    return Redirect("/RapporteringFactuur/FactuurRapportering");
                }
                if (tabelKeuze[1].Trim().Equals("Leverancier"))
                {
                    return Redirect("/RapporteringLeverancier/LeverancierRapportering");
                }
                if (tabelKeuze[1].Trim().Equals("Object"))
                {
                    return Redirect("/RapporteringObject/ObjectRapportering");
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
                    DAL.TblVerzekering verzekering = new DAL.TblVerzekering();
                    foreach (var item in verzekering.GetAll())
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
                    DAL.TblObjectType objectType = new DAL.TblObjectType();
                    foreach (var item in objectType.GetAll())
                    {
                        table.AddCell(item.IdObjectType.ToString());
                        table.AddCell(item.Omschrijving);
                    }
                    pdfDoc.Add(table);
                }
                if (tabelKeuze == "campus")
                {
                    PdfPTable table = new PdfPTable(4);
                    table.AddCell("naam");
                    table.AddCell("postcode");
                    table.AddCell("straat");
                    table.AddCell("nummer");
                    DAL.TblCampus campus = new DAL.TblCampus();
                    foreach (var item in campus.GetAll())
                    {
                        table.AddCell(item.Naam);
                        table.AddCell(item.Postcode);
                        table.AddCell(item.Straat);
                        table.AddCell(item.Nummer);
                    }
                    pdfDoc.Add(table);
                }
                if (tabelKeuze == "lokalen")
                {
                    PdfPTable table = new PdfPTable(3);
                    table.AddCell("naam");
                    table.AddCell("aantal plaatsen");
                    table.AddCell("computerlokaal");
                    DAL.TblLokaal lokaal = new DAL.TblLokaal();
                    foreach (var item in lokaal.GetAll())
                    {
                        table.AddCell(item.LokaalNaam);
                        table.AddCell(item.AantalPlaatsen.ToString());
                        table.AddCell(item.IsComputerLokaal.ToString());
                    }
                    pdfDoc.Add(table);
                }
                if (tabelKeuze == "factuur")
                {
                    PdfPTable table = new PdfPTable(15);
                    table.AddCell("Boekjaar");
                    table.AddCell("CVO volgnummer");
                    table.AddCell("Factuurnummer");
                    table.AddCell("ScholengroepNummer");
                    table.AddCell("Factuur datum");
                    table.AddCell("Leverancier");
                    table.AddCell("Prijs");
                    table.AddCell("Garantie");
                    table.AddCell("Omschrijving");
                    table.AddCell("Opmerking");
                    table.AddCell("Afschrijfperiode");
                    table.AddCell("Datum insert");
                    table.AddCell("User insert");
                    table.AddCell("Datum modified");
                    table.AddCell("User modified");
                    DAL.TblFactuur factuur = new DAL.TblFactuur();
                    foreach (var item in factuur.GetAll())
                    {
                        table.AddCell(item.Boekjaar);
                        table.AddCell(item.CvoVolgNummer);
                        table.AddCell(item.FactuurNummer);
                        table.AddCell(item.ScholengroepNummer);
                        table.AddCell(item.FactuurDatum.ToString());
                        table.AddCell(item.Leverancier.Naam);
                        table.AddCell(item.Prijs);
                        table.AddCell(item.Garantie.ToString());
                        table.AddCell(item.Omschrijving);
                        table.AddCell(item.Opmerking);
                        table.AddCell(item.Afschrijfperiode.ToString());
                        table.AddCell(item.DatumInsert.ToString());
                        table.AddCell(item.UserInsert);
                        table.AddCell(item.DatumModified.ToString());
                        table.AddCell(item.UserModified);
                    }
                    pdfDoc.Add(table);
                }
                if (tabelKeuze == "leverancier")
                {
                    PdfPTable table = new PdfPTable(14); // aantal kolommen
                    table.AddCell("Naam");
                    table.AddCell("Afkorting");
                    table.AddCell("Straat");
                    table.AddCell("HuisNummer");
                    table.AddCell("BusNummer");
                    table.AddCell("Postcode");
                    table.AddCell("Telefoon");
                    table.AddCell("Fax");
                    table.AddCell("Email");
                    table.AddCell("Website");
                    table.AddCell("Btw nummer");
                    table.AddCell("Iban");
                    table.AddCell("Bic");
                    table.AddCell("Toegevoegd op");
                    DAL.TblLeverancier objectType = new DAL.TblLeverancier();
                    foreach (var item in objectType.GetAll())
                    {
                        table.AddCell(item.Naam);
                        table.AddCell(item.Afkorting);
                        table.AddCell(item.Straat);
                        table.AddCell(item.HuisNummer.ToString());
                        table.AddCell(item.BusNummer.ToString());
                        table.AddCell(item.Postcode.ToString());
                        table.AddCell(item.Telefoon);
                        table.AddCell(item.Fax);
                        table.AddCell(item.Email);
                        table.AddCell(item.Website);
                        table.AddCell(item.BtwNummer);
                        table.AddCell(item.Iban);
                        table.AddCell(item.Bic);
                        table.AddCell(item.ToegevoegdOp.ToString());
                    }
                    pdfDoc.Add(table);
                }
                if (tabelKeuze == "object")
                {
                    PdfPTable table = new PdfPTable(15);
                    table.AddCell("Kenmerken");
                    table.AddCell("Factuur nummer");
                    table.AddCell("Object type");
                    DAL.TblObject Object = new DAL.TblObject();
                    foreach (var item in Object.GetAll())
                    {
                        table.AddCell(item.Kenmerken);
                        table.AddCell(item.Factuur.FactuurNummer);
                        table.AddCell(item.ObjectType.Omschrijving);
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
                DAL.TblVerzekering verzekering = new DAL.TblVerzekering();
                List<VerzekeringModel> vm = verzekering.GetAll();
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
                DAL.TblObjectType objectType = new DAL.TblObjectType();
                List<ObjectTypeModel> om = objectType.GetAll();
                worksheet.Cells[1, 1] = "id objectType";
                worksheet.Cells[1, 2] = "omschrijving";
                for (int i = 0; i < om.Count; i++)
                {
                    worksheet.Cells[i + 2, 1] = om[i].IdObjectType;
                    worksheet.Cells[i + 2, 2] = om[i].Omschrijving;
                }
            }
            if (tabelKeuze.Equals("campus"))
            {
                DAL.TblCampus campus = new DAL.TblCampus();
                List<CampusModel> cm = new List<CampusModel>();
                worksheet.Cells[1, 1] = "naam";
                worksheet.Cells[1, 2] = "postcode";
                worksheet.Cells[1, 3] = "straat";
                worksheet.Cells[1, 4] = "nummer";
                for (int i = 0; i < cm.Count; i++)
                {
                    worksheet.Cells[i + 2, 1] = cm[i].Naam;
                    worksheet.Cells[i + 2, 2] = cm[i].Postcode;
                    worksheet.Cells[i + 2, 3] = cm[i].Straat;
                    worksheet.Cells[i + 2, 4] = cm[i].Nummer;
                }
            }
            if (tabelKeuze.Equals("lokalen"))
            {
                DAL.TblLokaal lokaal = new DAL.TblLokaal();
                List<LokaalModel> lm = new List<LokaalModel>();
                worksheet.Cells[1, 1] = "naam";
                worksheet.Cells[1, 2] = "aantal plaatsen";
                worksheet.Cells[1, 3] = "computerlokaal";
                for (int i = 0; i < lm.Count; i++)
                {
                    worksheet.Cells[i + 2, 1] = lm[i].LokaalNaam;
                    worksheet.Cells[i + 2, 2] = lm[i].AantalPlaatsen.ToString();
                    worksheet.Cells[i + 2, 3] = lm[i].IsComputerLokaal.ToString();
                }
            }
            if (tabelKeuze.Equals("factuur"))
            {
                DAL.TblFactuur factuur = new DAL.TblFactuur();
                List<FactuurModel> fm = factuur.GetAll();
                worksheet.Cells[1, 1] = "Boekjaar";
                worksheet.Cells[1, 2] = "CVO volgnummer";
                worksheet.Cells[1, 3] = "Factuurnummer";
                worksheet.Cells[1, 4] = "ScholengroepNummer";
                worksheet.Cells[1, 5] = "Factuur datum";
                worksheet.Cells[1, 6] = "Leverancier";
                worksheet.Cells[1, 7] = "Prijs";
                worksheet.Cells[1, 8] = "Garantie";
                worksheet.Cells[1, 9] = "Omschrijving";
                worksheet.Cells[1, 10] = "Opmerking";
                worksheet.Cells[1, 11] = "Afschrijfperiode";
                worksheet.Cells[1, 12] = "Datum insert";
                worksheet.Cells[1, 13] = "User insert";
                worksheet.Cells[1, 14] = "Datum modified";
                worksheet.Cells[1, 15] = "User modified";
                for (int i = 0; 1 < fm.Count; i++)
                {
                    worksheet.Cells[i + 2, 1] = fm[i].Boekjaar;
                    worksheet.Cells[i + 2, 2] = fm[i].CvoVolgNummer;
                    worksheet.Cells[i + 2, 3] = fm[i].FactuurNummer;
                    worksheet.Cells[i + 2, 4] = fm[i].ScholengroepNummer;
                    worksheet.Cells[i + 2, 5] = fm[i].FactuurDatum;
                    worksheet.Cells[i + 2, 6] = fm[i].Leverancier;
                    worksheet.Cells[i + 2, 7] = fm[i].Prijs;
                    worksheet.Cells[i + 2, 8] = fm[i].Garantie;
                    worksheet.Cells[i + 2, 9] = fm[i].Omschrijving;
                    worksheet.Cells[i + 2, 10] = fm[i].Opmerking;
                    worksheet.Cells[i + 2, 11] = fm[i].Afschrijfperiode;
                    worksheet.Cells[i + 2, 12] = fm[i].DatumInsert;
                    worksheet.Cells[i + 2, 13] = fm[i].UserInsert;
                    worksheet.Cells[i + 2, 14] = fm[i].DatumModified;
                    worksheet.Cells[i + 2, 15] = fm[i].UserModified;
                }
            }
            if (tabelKeuze.Equals("leverancier"))
            {
                DAL.TblLeverancier leverancier = new DAL.TblLeverancier();
                List<LeverancierModel> lm = leverancier.GetAll();
                worksheet.Cells[1, 1] = "Naam";
                worksheet.Cells[1, 2] = "Afkorting";
                worksheet.Cells[1, 3] = "Straat";
                worksheet.Cells[1, 4] = "HuisNummer";
                worksheet.Cells[1, 5] = "BusNummer";
                worksheet.Cells[1, 6] = "Postcode";
                worksheet.Cells[1, 7] = "Telefoon";
                worksheet.Cells[1, 8] = "Fax";
                worksheet.Cells[1, 9] = "Email";
                worksheet.Cells[1, 10] = "Website";
                worksheet.Cells[1, 11] = "Btw nummer";
                worksheet.Cells[1, 12] = "Iban";
                worksheet.Cells[1, 13] = "Bic";
                worksheet.Cells[1, 14] = "Toegevoegd op";

                for (int i = 0; i < lm.Count; i++)
                {
                    worksheet.Cells[i + 2, 1] = lm[i].Naam;
                    worksheet.Cells[i + 2, 2] = lm[i].Afkorting;
                    worksheet.Cells[i + 2, 3] = lm[i].Straat;
                    worksheet.Cells[i + 2, 4] = lm[i].HuisNummer;
                    worksheet.Cells[i + 2, 5] = lm[i].BusNummer;
                    worksheet.Cells[i + 2, 6] = lm[i].Postcode;
                    worksheet.Cells[i + 2, 7] = lm[i].Telefoon;
                    worksheet.Cells[i + 2, 8] = lm[i].Fax;
                    worksheet.Cells[i + 2, 9] = lm[i].Email;
                    worksheet.Cells[i + 2, 10] = lm[i].Website;
                    worksheet.Cells[i + 2, 11] = lm[i].BtwNummer;
                    worksheet.Cells[i + 2, 12] = lm[i].Iban;
                    worksheet.Cells[i + 2, 13] = lm[i].Bic;
                    worksheet.Cells[i + 2, 14] = lm[i].ToegevoegdOp;
                }
            }
            if (tabelKeuze.Equals("object"))
            {
                DAL.TblObject Object = new DAL.TblObject();
                List<ObjectModel> om = Object.GetAll();
                worksheet.Cells[1, 1] = "Kenmerken";
                worksheet.Cells[1, 2] = "Factuur nummer";
                worksheet.Cells[1, 3] = "Object type";
                for (int i = 0; 1 < om.Count; i++)
                {
                    worksheet.Cells[i + 2, 1] = om[i].Kenmerken;
                    worksheet.Cells[i + 2, 2] = om[i].Factuur.FactuurNummer;
                    worksheet.Cells[i + 2, 3] = om[i].ObjectType.Omschrijving;
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