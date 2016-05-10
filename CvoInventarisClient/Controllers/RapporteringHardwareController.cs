using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Controllers
{
    public class RapporteringHardwareController : Controller
    {
        public CvoInventarisServiceClient test = new CvoInventarisServiceClient();
        List<Hardware> hardwares;
        TabelModel model;
        public void vulHardwares()
        {
            hardwares = test.HardwareGetAll().ToList();

        }

        public List<List<object>> maakDropDownlijst()
        {
            List<List<object>> dropDownlijst = new List<List<object>>();
            vulHardwares();
            List<System.Object> cpus = new List<object>();
            List<System.Object> devices = new List<object>();
            List<System.Object> grafischeKaarten = new List<object>();
            List<System.Object> harddisks = new List<object>();
            foreach (Hardware h in hardwares)
            {
                cpus.Add(new { value = h.Cpu.IdCpu, text = h.Cpu.Merk });
                devices.Add(new { value = h.Device.IdDevice, text = h.Device.Merk });
                grafischeKaarten.Add(new { value = h.GrafischeKaart.IdGrafischeKaart, text = h.GrafischeKaart.Merk });
                harddisks.Add(new { value = h.Harddisk.IdHarddisk, text = h.Harddisk.Merk });
            }
            dropDownlijst.Add(cpus);
            dropDownlijst.Add(devices);
            dropDownlijst.Add(grafischeKaarten);
            dropDownlijst.Add(harddisks);
            return dropDownlijst;
        }

        public void VulDropDownLijstIn()
        {
            List<List<object>> dropDownLijst = maakDropDownlijst();
            SelectList cpus = new SelectList(dropDownLijst[0], "value", "text");
            SelectList apparaten = new SelectList(dropDownLijst[1], "value", "text");
            SelectList grafischeKaarten = new SelectList(dropDownLijst[2], "value", "text");
            SelectList harddisks = new SelectList(dropDownLijst[3], "value", "text");
            ViewBag.cpus = cpus;
            ViewBag.apparaten = apparaten;
            ViewBag.grafischeKaarten = grafischeKaarten;
            ViewBag.harddisks = harddisks;
        }

        public ActionResult HardwareRapportering()
        {
            ViewBag.stijlStapOpslaan = "hidden";
            model = new TabelModel();
            VulDropDownLijstIn();
            return View(model);
        }

        public ActionResult Stap3(FormCollection collection)
        {
            model = new TabelModel();
            //een tabel van drie omdat je maar 4 lijnen nodig hebt. een select statement, u from, left joins en u where clausule
            string[] query = new string[4];
            //hier gaat hem nakijken welke checkboxen er aan gevinkt zijn. dus welke tabellen de klant gegevens van wilt hebben
            List<string> kolomKeuze = new List<string>();
            kolomKeuze.Add("TblHardware.idHardware");
            if (Convert.ToBoolean(collection["idCpu"].Split(',')[0]) == true)
                kolomKeuze.Add("TblCpu.merk cm");
            if (Convert.ToBoolean(collection["idDevice"].Split(',')[0]) == true)
                kolomKeuze.Add("TblDevice.merk dm");
            if (Convert.ToBoolean(collection["idGrafischeKaart"].Split(',')[0]) == true)
                kolomKeuze.Add("TblGrafischeKaart.merk gm");
            if (Convert.ToBoolean(collection["idHarddisk"].Split(',')[0]) == true)
                kolomKeuze.Add("TblHarddisk.merk hm");

            List<string> lijstConditie = new List<string>();

            //kijkt welke checkboxen van stap 4 van CpuHardware zijn aangevinkt en deze waarden slaat hem op
            if (Convert.ToBoolean(collection["cpuKeuzeCheck"].Split(',')[0]) == true)
            {
                int id = Int32.Parse(collection["cpus"].Split(',')[0]);
                string l = test.CpuGetById(id).Merk;
                lijstConditie.Add("TblCpu.merk = " + "'" + l.Trim() + "'");
            }
            if (Convert.ToBoolean(collection["deviceKeuzeCheck"].Split(',')[0]) == true)
            {
                int id = Int32.Parse(collection["apparaten"].Split(',')[0]);
                string l = test.DeviceGetById(id).Merk;
                lijstConditie.Add("TblDevice.merk = " + "'" + l.Trim() + "'");
            }
            if (Convert.ToBoolean(collection["grafischeKaartKeuzeCheck"].Split(',')[0]) == true)
            {
                int id = Int32.Parse(collection["grafischeKaarten"].Split(',')[0]);
                string l = test.GrafischeKaartGetById(id).Merk;
                lijstConditie.Add("TblGrafischeKaart.merk = " + "'" + l.Trim() + "'");
            }
            if (Convert.ToBoolean(collection["harddiskKeuzeCheck"].Split(',')[0]) == true)
            {
                int id = Int32.Parse(collection["harddisks"].Split(',')[0]);
                string l = test.HarddiskGetById(id).Merk;
                lijstConditie.Add("TblHarddisk.merk = " + "'" + l.Trim() + "'");
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
            query[1] = " FROM TblHardware ";

            //left joins
            if (Convert.ToBoolean(collection["idCpu"].Split(',')[0]) == true)
                query[2] += "left JOIN TblCpu ON TblHardware.idCpu = TblCpu.idCpu ";
            if (Convert.ToBoolean(collection["idDevice"].Split(',')[0]) == true)
                query[2] += "left JOIN TblDevice ON TblHardware.idDevice = TblDevice.idDevice";
            if (Convert.ToBoolean(collection["idGrafischeKaart"].Split(',')[0]) == true)
                query[2] += "left JOIN TblGrafischeKaart ON TblHardware.idGrafischeKaart = TblGrafischeKaart.idGrafischeKaart ";
            if (Convert.ToBoolean(collection["idHarddisk"].Split(',')[0]) == true)
                query[2] += "left join TblHarddisk ON TblHardware.idHarddisk = TblHarddisk.idHarddisk";

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
            hardwares = test.RapporteringHardware(queryResultaat, kolomKeuzeArray).ToList();
            List<HardwareModel> hardwareModellen = new List<HardwareModel>();
            foreach (Hardware h in hardwares)
            {
                HardwareModel hm = new HardwareModel();

                CpuModel cm = new CpuModel();
                cm.Merk = h.Cpu.Merk;
                hm.Cpu = cm;

                DeviceModel dm = new DeviceModel();
                dm.Merk = h.Device.Merk;
                hm.Device = dm;

                GrafischeKaartModel gm = new GrafischeKaartModel();
                gm.Merk = h.GrafischeKaart.Merk;
                hm.GrafischeKaart = gm;

                HarddiskModel harddiskModel = new HarddiskModel();
                harddiskModel.Merk = h.Harddisk.Merk;
                hm.Harddisk = harddiskModel;

                hm.IdHardware = h.Id;
                hardwareModellen.Add(hm);
            }
            model.hardwares = hardwareModellen;
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
            VulDropDownLijstIn();
            return View("HardwareRapportering", model);
        }
    }
}