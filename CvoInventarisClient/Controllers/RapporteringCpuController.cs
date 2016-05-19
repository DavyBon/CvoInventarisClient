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
    public class RapporteringCpuController : Controller
    {
        public CvoInventarisServiceClient test = new CvoInventarisServiceClient();
        //nodig voor mijn dropdown menu te maken
        List<Cpu> cpus;
        TabelModel model;
        public void vulCpus()
        {
            cpus = test.CpuGetAll().ToList();

        }

        public List<List<object>> maakDropDownlijst()
        {
            List<List<object>> dropDownlijst = new List<List<object>>();
            vulCpus();
            List<System.Object> merken = new List<object>();
            List<System.Object> types = new List<object>();
            List<System.Object> fabrieksnummer = new List<object>();
            foreach (Cpu c in cpus)
            {
                merken.Add(new { value = c.IdCpu, text = c.Merk });
                types.Add(new { value = c.IdCpu, text = c.Type });
                fabrieksnummer.Add(new { value = c.IdCpu, text = c.FabrieksNummer });
            }
            dropDownlijst.Add(merken);
            dropDownlijst.Add(types);
            dropDownlijst.Add(fabrieksnummer);
            return dropDownlijst;
        }

        public void VulDropDownLijstIn()
        {
            List<List<object>> dropDownLijst = maakDropDownlijst();
            SelectList merken = new SelectList(dropDownLijst[0], "value", "text");
            SelectList types = new SelectList(dropDownLijst[1], "value", "text");
            SelectList fabrieksnummer = new SelectList(dropDownLijst[2], "value", "text");
            ViewBag.merken = merken;
            ViewBag.types = types;
            ViewBag.fabrieksNummers = fabrieksnummer;
        }

        public ActionResult CpuRapportering()
        {
            ViewBag.stijlStapOpslaan = "hidden";
            model = new TabelModel();
            VulDropDownLijstIn();
            return View(model);
        }

        public ActionResult Stap3Cpu(FormCollection collection)
        {
            model = new TabelModel();
            //een tabel van drie omdat je maar drie lijnen nodig hebt. een select statement, u from en u where clausule
            string[] query = new string[3];
            //hier gaat hem nakijken welke checkboxen er aan gevinkt zijn. dus welke tabellen de klant gegevens van wilt hebben
            List<string> kolomKeuze = new List<string>();
            if (Convert.ToBoolean(collection["idCpu"].Split(',')[0]) == true)
                kolomKeuze.Add("idCpu");
            if (Convert.ToBoolean(collection["merk"].Split(',')[0]) == true)
                kolomKeuze.Add("merk");
            if (Convert.ToBoolean(collection["type"].Split(',')[0]) == true)
                kolomKeuze.Add("type");
            if (Convert.ToBoolean(collection["snelheid"].Split(',')[0]) == true)
                kolomKeuze.Add("snelheid");
            if (Convert.ToBoolean(collection["fabrieksnummer"].Split(',')[0]) == true)
                kolomKeuze.Add("fabrieksNummer");

            List<string> lijstConditie = new List<string>();

            //kijkt welke checkboxen van stap 4 van CpuRapporting zijn aangevinkt en deze waarden slaat hem op
            if (Convert.ToBoolean(collection["merkKeuzeCheck"].Split(',')[0]) == true)
            {
                int id = Int32.Parse(collection["merken"].Split(',')[0]);
                string l = test.CpuGetById(id).Merk;
                lijstConditie.Add("merk = " + "'" + l.Trim() + "'");
            }
            if (Convert.ToBoolean(collection["typeKeuzeCheck"].Split(',')[0]) == true)
            {
                int id = Int32.Parse(collection["types"].Split(',')[0]);
                string l = test.CpuGetById(id).Type;
                lijstConditie.Add("type = " + "'" + l.Trim() + "'");
            }
            if (Convert.ToBoolean(collection["snelheidKeuzeCheck"].Split(',')[0]) == true)
            {
                string r = collection["snelheidKeuze"].Split(',')[0] + " " + collection["snelheidKeuze1"].Split(',')[0];
                lijstConditie.Add("snelheid =  " + r.Trim());
            }
            if (Convert.ToBoolean(collection["fabrieksnummerKeuzeCheck"].Split(',')[0]) == true)
            {
                int id = Int32.Parse(collection["fabrieksNummers"].Split(',')[0]);
                string l = test.CpuGetById(id).FabrieksNummer;
                lijstConditie.Add("fabrieksNummer = " + "'" + l.Trim() + "'");
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
            query[1] = " FROM TblCpu ";

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
            cpus = test.RapporteringCpu(queryResultaat, kolomKeuzeArray).ToList();
            List<CpuModel> cpuModellen = new List<CpuModel>();
            foreach (Cpu c in cpus)
            {
                CpuModel cm = new CpuModel();
                cm.FabrieksNummer = c.FabrieksNummer;
                cm.IdCpu = c.IdCpu;
                cm.Merk = c.Merk;
                cm.Snelheid = c.Snelheid;
                cm.Type = c.Type;
                cpuModellen.Add(cm);
            }
            model.cpus = cpuModellen;
            //ik geef die mee omdat ik die op de view op hidden ga zetten maar dan kan ik daarna terug aan als ik ze wil opslaan als pdf of excel
            ViewBag.query = queryResultaat;
            ViewBag.kolomKeuze = kolomKeuzeArray;
            VulDropDownLijstIn();
            ViewBag.stijlStapOpslaan = "inline";
            string kolomnamen = "";
            foreach (string s in kolomKeuze)
            {
                kolomnamen += s + " ";
            }
            ViewBag.kolomnamen = kolomnamen.Trim();
            return View("CpuRapportering", model);
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
            VulDropDownLijstIn();
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
                cpus = test.RapporteringCpu(query, kolomKeuze).ToList();
                List<CpuModel> cpuModellen = new List<CpuModel>();
                foreach (Cpu c in cpus)
                {
                    CpuModel cm = new CpuModel();
                    cm.FabrieksNummer = c.FabrieksNummer;
                    cm.IdCpu = c.IdCpu;
                    cm.Merk = c.Merk;
                    cm.Snelheid = c.Snelheid;
                    cm.Type = c.Type;
                    cpuModellen.Add(cm);
                }

                if (cpuModellen[0].IdCpu != 0)
                    table.AddCell("id");
                if (cpuModellen[0].Merk != null)
                    table.AddCell("merk");
                if (cpuModellen[0].Type != null)
                    table.AddCell("type");
                if (cpuModellen[0].Snelheid != 0)
                    table.AddCell("snelheid");
                if (cpuModellen[0].FabrieksNummer != null)
                    table.AddCell("fabrieksnummer");

                foreach (var item in cpuModellen)
                {
                    if (cpuModellen[0].IdCpu != 0)
                        table.AddCell(item.IdCpu.ToString());
                    if (cpuModellen[0].Merk != null)
                        table.AddCell(item.Merk);
                    if (cpuModellen[0].Type != null)
                        table.AddCell(item.Type);
                    if (cpuModellen[0].Snelheid != 0)
                        table.AddCell(item.Snelheid.ToString());
                    if (cpuModellen[0].FabrieksNummer != null)
                        table.AddCell(item.FabrieksNummer.ToString());
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
            List<Cpu> cpus = new List<Cpu>();
            cpus = test.RapporteringCpu(query, kolomKeuze).ToList();
            List<CpuModel> cpuModellen = new List<CpuModel>();
            foreach (Cpu c in cpus)
            {
                CpuModel cm = new CpuModel();
                cm.FabrieksNummer = c.FabrieksNummer;
                cm.IdCpu = c.IdCpu;
                cm.Merk = c.Merk;
                cm.Snelheid = c.Snelheid;
                cm.Type = c.Type;
                cpuModellen.Add(cm);
            }
            for (int i = 0; i < kolomKeuze.Length; i++)
            {
                if (cpuModellen[0].IdCpu != 0)
                    worksheet.Cells[1, 1 + i] = "id";
                if (cpuModellen[0].Merk != null)
                    worksheet.Cells[1, i + 1] = "merk";
                if (cpuModellen[0].Type != null)
                    worksheet.Cells[1, i + 1] = "type";
                if (cpuModellen[0].Snelheid != 0)
                    worksheet.Cells[1, i + 1] = "snelheid";
                if (cpuModellen[0].FabrieksNummer != null)
                    worksheet.Cells[1, i + 1] = "fabrieksnummer";
            }
            for (int i = 0; i < cpus.Count; i++)
            {
                //ik werkt met een teller omdat niet alle kolommen getoond worden. dus als er een kolom gegevens heeft gaat de teller met 1 omhoog waardoor het perfect naast elkaar komt
                int teller = 0;
                if (cpuModellen[0].IdCpu != 0)
                {
                    worksheet.Cells[i + 2, 1 + teller] = cpus[i].IdCpu;
                    teller++;
                }

                if (cpuModellen[0].Merk != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = cpus[i].Merk;
                    teller++;
                }

                if (cpuModellen[0].Type != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = cpus[i].Type;
                    teller++;
                }

                if (cpuModellen[0].Snelheid != 0)
                {
                    worksheet.Cells[i + 2, 1 + teller] = cpus[i].Snelheid;
                    teller++;
                }

                if (cpuModellen[0].FabrieksNummer != null)
                {
                    worksheet.Cells[i + 2, 1 + teller] = "=\"" + cpus[i].FabrieksNummer.Trim() + "\"";
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