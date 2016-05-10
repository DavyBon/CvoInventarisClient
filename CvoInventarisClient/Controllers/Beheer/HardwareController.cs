using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class HardwareController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                //WCF servicereference objecten collection naar InventarisModel objecten collection
                HardwareViewModel model = new HardwareViewModel();
                model.Hardwares = new List<HardwareModel>();
                model.Cpus = new List<SelectListItem>();
                model.Devices = new List<SelectListItem>();
                model.GrafischeKaarten = new List<SelectListItem>();
                model.Harddisks = new List<SelectListItem>();

                foreach (Hardware h in client.HardwareGetAll())
                {
                    HardwareModel hardware = new HardwareModel();
                    hardware.IdHardware = h.Id;
                    hardware.Cpu = new CpuModel() { IdCpu = h.Cpu.IdCpu, FabrieksNummer = h.Cpu.FabrieksNummer, Merk = h.Cpu.Merk, Snelheid = h.Cpu.Snelheid, Type = h.Cpu.Type };
                    hardware.Device = new DeviceModel() { IdDevice = h.Device.IdDevice, Type = h.Device.Type, Merk = h.Device.Merk, FabrieksNummer = h.Device.FabrieksNummer, IsPcCompatibel = h.Device.IsPcCompatibel, Serienummer = h.Device.Serienummer };
                    hardware.GrafischeKaart = new GrafischeKaartModel() { FabrieksNummer = h.GrafischeKaart.FabrieksNummer, Merk = h.GrafischeKaart.Merk, Type = h.GrafischeKaart.Type, Driver = h.GrafischeKaart.Driver, IdGrafischeKaart = h.GrafischeKaart.IdGrafischeKaart };
                    hardware.Harddisk = new HarddiskModel() { Merk = h.Harddisk.Merk, FabrieksNummer = h.Harddisk.FabrieksNummer, Grootte = h.Harddisk.Grootte, IdHarddisk = h.Harddisk.IdHarddisk};
                    model.Hardwares.Add(hardware);
                }
                foreach (Cpu  c in client.CpuGetAll())
                {
                    model.Cpus.Add(new SelectListItem { Text = c.Merk, Value = c.IdCpu.ToString() });
                }
                foreach (Device d in client.DeviceGetAll())
                {
                    model.Devices.Add(new SelectListItem { Text = d.Merk, Value = d.IdDevice.ToString() });
                }
                foreach (GrafischeKaart g in client.GrafischeKaartGetAll())
                {
                    model.GrafischeKaarten.Add(new SelectListItem { Text = g.Merk, Value = g.IdGrafischeKaart.ToString() });
                }
                foreach (Harddisk h in client.HarddiskGetAll())
                {
                    model.Harddisks.Add(new SelectListItem { Text = h.Merk, Value = h.IdHarddisk.ToString() });
                }
                return View(model);
            }
        }

        // POST: Inventaris/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Hardware hardware = new Hardware();
                hardware.Cpu = new ServiceReference.Cpu() { IdCpu = Convert.ToInt16(Request.Form["Cpus"]) };
                hardware.Device = new ServiceReference.Device() { IdDevice = Convert.ToInt16(Request.Form["Devices"]) };
                hardware.GrafischeKaart = new ServiceReference.GrafischeKaart() { IdGrafischeKaart = Convert.ToInt16(Request.Form["GrafischeKaarten"]) };
                hardware.Harddisk = new ServiceReference.Harddisk() { IdHarddisk = Convert.ToInt16(Request.Form["Harddisks"]) };
                client.HardwareCreate(hardware);
            }
            TempData["action"] = "Object werd toegevoegd aan hardware";
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                HardwareViewModel model = new HardwareViewModel();
                model.Hardwares = new List<HardwareModel>();
                model.Cpus = new List<SelectListItem>();
                model.Devices = new List<SelectListItem>();
                model.GrafischeKaarten = new List<SelectListItem>();
                model.Harddisks = new List<SelectListItem>();

                Hardware h = client.HardwareGetById(id);
                HardwareModel hardware = new HardwareModel();
                hardware.IdHardware = h.Id;
                hardware.Cpu = new CpuModel() { IdCpu = h.Cpu.IdCpu, FabrieksNummer = h.Cpu.FabrieksNummer, Merk = h.Cpu.Merk, Snelheid = h.Cpu.Snelheid, Type = h.Cpu.Type };
                hardware.Device = new DeviceModel() { IdDevice = h.Device.IdDevice, Type = h.Device.Type, Merk = h.Device.Merk, FabrieksNummer = h.Device.FabrieksNummer, IsPcCompatibel = h.Device.IsPcCompatibel, Serienummer = h.Device.Serienummer };
                hardware.GrafischeKaart = new GrafischeKaartModel() { FabrieksNummer = h.GrafischeKaart.FabrieksNummer, Merk = h.GrafischeKaart.Merk, Type = h.GrafischeKaart.Type, Driver = h.GrafischeKaart.Driver, IdGrafischeKaart = h.GrafischeKaart.IdGrafischeKaart };
                hardware.Harddisk = new HarddiskModel() { Merk = h.Harddisk.Merk, FabrieksNummer = h.Harddisk.FabrieksNummer, Grootte = h.Harddisk.Grootte, IdHarddisk = h.Harddisk.IdHarddisk };
                model.Hardwares.Add(hardware);

                foreach (Cpu c in client.CpuGetAll())
                {
                    if (!(c.IdCpu == hardware.Cpu.IdCpu))
                    {
                        model.Cpus.Add(new SelectListItem { Text = c.Merk, Value = c.IdCpu.ToString() });
                    }
                }
                foreach (Device d in client.DeviceGetAll())
                {
                    if (!(d.IdDevice == hardware.Device.IdDevice))
                    {
                        model.Devices.Add(new SelectListItem { Text = d.Merk, Value = d.IdDevice.ToString() });
                    }
                }
                foreach (GrafischeKaart g in client.GrafischeKaartGetAll())
                {
                    if (!(g.IdGrafischeKaart == hardware.GrafischeKaart.IdGrafischeKaart))
                    {
                        model.GrafischeKaarten.Add(new SelectListItem { Text = g.Merk, Value = g.IdGrafischeKaart.ToString() });
                    }
                }
                foreach (Harddisk hd in client.HarddiskGetAll())
                {
                    if (!(hd.IdHarddisk == hardware.Harddisk.IdHarddisk))
                    {
                        model.Harddisks.Add(new SelectListItem { Text = hd.Merk, Value = hd.IdHarddisk.ToString() });
                    }
                }
                return View(model);
            }
        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Hardware hardware = new Hardware();
                hardware.Id = Convert.ToInt16(Request.Form["idHardware"]);
                hardware.Cpu = new ServiceReference.Cpu() { IdCpu = Convert.ToInt16(Request.Form["idCpu"]) };

                if (!String.IsNullOrWhiteSpace(Request.Form["cpus"])) { hardware.Cpu = new ServiceReference.Cpu() { IdCpu = Convert.ToInt16(Request.Form["cpus"]) }; }
                else { hardware.Cpu = new ServiceReference.Cpu() { IdCpu = Convert.ToInt16(Request.Form["defaultIdCpu"]) }; }

                if (!String.IsNullOrWhiteSpace(Request.Form["devices"])) { hardware.Device = new ServiceReference.Device() { IdDevice = Convert.ToInt16(Request.Form["devices"]) }; }
                else { hardware.Device = new ServiceReference.Device() { IdDevice = Convert.ToInt16(Request.Form["defaultIdDevice"]) }; }

                if (!String.IsNullOrWhiteSpace(Request.Form["grafischeKaarten"])) { hardware.GrafischeKaart = new ServiceReference.GrafischeKaart() { IdGrafischeKaart = Convert.ToInt16(Request.Form["grafischeKaarten"]) }; }
                else { hardware.GrafischeKaart = new ServiceReference.GrafischeKaart() { IdGrafischeKaart = Convert.ToInt16(Request.Form["defaultIdGrafischeKaart"]) }; }

                if (!String.IsNullOrWhiteSpace(Request.Form["harddisks"])) { hardware.Harddisk = new ServiceReference.Harddisk() { IdHarddisk = Convert.ToInt16(Request.Form["harddisks"]) }; }
                else { hardware.Harddisk = new ServiceReference.Harddisk() { IdHarddisk = Convert.ToInt16(Request.Form["defaultIdHarddisk"]) }; }

                client.HardwareUpdate(hardware);
            }
            TempData["action"] = "Object in inventaris werd gewijzigd";
            return RedirectToAction("Index");
        }

        // POST: Inventaris/Delete/5
        [HttpPost]
        public ActionResult Delete(int[] idArray, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                foreach (int id in idArray)
                {
                    client.HardwareDelete(id);
                }
            }
            if (idArray.Length >= 2)
            {
                TempData["action"] = idArray.Length + " objecten werden verwijderd uit de hardware tabel";
            }
            else
            {
                TempData["action"] = idArray.Length + " netwerk werd verwijderd uit de hardware tabel";
            }
            return RedirectToAction("Index");
        }
        public List<HardwareModel> HardwareGetAll()
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            Hardware[] hw = new Hardware[] { };

            try
            {
                hw = service.HardwareGetAll();
            }
            catch (Exception)
            {

            }

            List<HardwareModel> hardwares = new List<HardwareModel>();

            foreach (Hardware hardware in hw)
            {
                HardwareModel h = new HardwareModel();
                CpuModel cm = new CpuModel();
                DeviceModel dm = new DeviceModel();
                GrafischeKaartModel gm = new GrafischeKaartModel();
                HarddiskModel hm = new HarddiskModel();

                h.IdHardware = hardware.Id;

                cm.FabrieksNummer = hardware.Cpu.FabrieksNummer;
                cm.IdCpu = hardware.Cpu.IdCpu;
                cm.Merk = hardware.Cpu.Merk;
                cm.Snelheid = hardware.Cpu.Snelheid;
                cm.Type = hardware.Cpu.Type;
                h.Cpu = cm;

                dm.FabrieksNummer = hardware.Device.FabrieksNummer;
                dm.IdDevice = hardware.Device.IdDevice;
                dm.IsPcCompatibel = hardware.Device.IsPcCompatibel;
                dm.Merk = hardware.Device.Merk;
                dm.Serienummer = hardware.Device.Serienummer;
                dm.Type = hardware.Device.Type;
                h.Device = dm;

                gm.Driver = hardware.GrafischeKaart.Driver;
                gm.FabrieksNummer = hardware.GrafischeKaart.FabrieksNummer;
                gm.IdGrafischeKaart = hardware.GrafischeKaart.IdGrafischeKaart;
                gm.Merk = hardware.GrafischeKaart.Merk;
                gm.Type = hardware.GrafischeKaart.Type;
                h.GrafischeKaart = gm;

                hm.FabrieksNummer = hardware.Harddisk.FabrieksNummer;
                hm.Grootte = hardware.Harddisk.Grootte;
                hm.IdHarddisk = hardware.Harddisk.IdHarddisk;
                hm.Merk = hardware.Harddisk.Merk;
                h.Harddisk = hm;
                hardwares.Add(h);
            }

            return hardwares;
        }
    }
}