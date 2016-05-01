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
            return View(HardwareGetAll());
        }

        public ActionResult Edit(int id)
        {
            return View(HardwareGetById(id));
        }

        [HttpPost]
        public ActionResult Edit(HardwareModel h)
        {
            if (UpdateHardware(h))
            {
                ViewBag.EditMesage = "Row updated";
                //return View("Index");
                return View();
            }
            else
            {
                ViewBag.EditMesage = "Row not updated";
                return View();
            }
        }

        public ActionResult Create()
        {
            return View(new HardwareModel());
        }

        [HttpPost]
        public ActionResult Create(HardwareModel h)
        {
            if (InsertHardware(h) >= 0)
            {
                ViewBag.CreateMesage = "Row inserted";
                //return View("Index");
                return View();
            }
            else
            {
                ViewBag.CreateMesage = "Row not inserted";
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            return View(HardwareGetById(id));
        }

        public ActionResult Delete(int id)
        {
            return View(HardwareGetById(id));
        }

        [HttpPost]
        public ActionResult Delete(HardwareModel h)
        {
            if (DeleteHardware(h))
            {
                ViewBag.DeleteMesage = "Row deleted";
                //return View("Index");
                return View();
            }
            else
            {
                ViewBag.DeleteMesage = "Row not deleted";
                return View();
            }
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
                h.IdHardware = hardware.Id;

                h.Cpu.FabrieksNummer = hardware.Cpu.FabrieksNummer;
                h.Cpu.IdCpu = hardware.Cpu.IdCpu;
                h.Cpu.Merk = hardware.Cpu.Merk;
                h.Cpu.Snelheid = hardware.Cpu.Snelheid;
                h.Cpu.Type = hardware.Cpu.Type;

                h.Device.FabrieksNummer = hardware.Device.FabrieksNummer;
                h.Device.IdDevice = hardware.Device.IdDevice;
                h.Device.IsPcCompatibel = hardware.Device.IsPcCompatibel;
                h.Device.Merk = hardware.Device.Merk;
                h.Device.Serienummer = hardware.Device.Serienummer;
                h.Device.Type = hardware.Device.Type;

                h.GrafischeKaart.Driver = hardware.GrafischeKaart.Driver;
                h.GrafischeKaart.FabrieksNummer = hardware.GrafischeKaart.FabrieksNummer;
                h.GrafischeKaart.IdGrafischeKaart = hardware.GrafischeKaart.IdGrafischeKaart;
                h.GrafischeKaart.Merk = hardware.GrafischeKaart.Merk;
                h.GrafischeKaart.Type = hardware.GrafischeKaart.Type;

                h.Harddisk.FabrieksNummer = hardware.Harddisk.FabrieksNummer;
                h.Harddisk.Grootte = hardware.Harddisk.Grootte;
                h.Harddisk.IdHarddisk = hardware.Harddisk.IdHarddisk;
                h.Harddisk.Merk = hardware.Harddisk.Merk;
                hardwares.Add(h);
            }

            return hardwares;
        }

        public HardwareModel HardwareGetById(int id)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            Hardware hardware = new Hardware();

            try
            {
                hardware = service.HardwareGetById(id);
            }
            catch (Exception)
            {

            }

            HardwareModel h = new HardwareModel();
            h.IdHardware = hardware.Id;

            h.Cpu.FabrieksNummer = hardware.Cpu.FabrieksNummer;
            h.Cpu.IdCpu = hardware.Cpu.IdCpu;
            h.Cpu.Merk = hardware.Cpu.Merk;
            h.Cpu.Snelheid = hardware.Cpu.Snelheid;
            h.Cpu.Type = hardware.Cpu.Type;

            h.Device.FabrieksNummer = hardware.Device.FabrieksNummer;
            h.Device.IdDevice = hardware.Device.IdDevice;
            h.Device.IsPcCompatibel = hardware.Device.IsPcCompatibel;
            h.Device.Merk = hardware.Device.Merk;
            h.Device.Serienummer = hardware.Device.Serienummer;
            h.Device.Type = hardware.Device.Type;

            h.GrafischeKaart.Driver = hardware.GrafischeKaart.Driver;
            h.GrafischeKaart.FabrieksNummer = hardware.GrafischeKaart.FabrieksNummer;
            h.GrafischeKaart.IdGrafischeKaart = hardware.GrafischeKaart.IdGrafischeKaart;
            h.GrafischeKaart.Merk = hardware.GrafischeKaart.Merk;
            h.GrafischeKaart.Type = hardware.GrafischeKaart.Type;

            h.Harddisk.FabrieksNummer = hardware.Harddisk.FabrieksNummer;
            h.Harddisk.Grootte = hardware.Harddisk.Grootte;
            h.Harddisk.IdHarddisk = hardware.Harddisk.IdHarddisk;
            h.Harddisk.Merk = hardware.Harddisk.Merk;
            return h;
        }

        public bool UpdateHardware(HardwareModel hardware)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            Hardware h = new Hardware();
            h.Id = hardware.IdHardware;
            h.Cpu.IdCpu = hardware.Cpu.IdCpu;
            h.Device.IdDevice = hardware.Device.IdDevice;
            h.GrafischeKaart.IdGrafischeKaart = hardware.GrafischeKaart.IdGrafischeKaart;
            h.Harddisk.IdHarddisk = hardware.Harddisk.IdHarddisk;

            try
            {
                return service.HardwareUpdate(h);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int InsertHardware(HardwareModel hardware)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            Hardware h = new Hardware();
            h.Cpu.IdCpu = hardware.Cpu.IdCpu;
            h.Device.IdDevice = hardware.Device.IdDevice;
            h.GrafischeKaart.IdGrafischeKaart = hardware.GrafischeKaart.IdGrafischeKaart;
            h.Harddisk.IdHarddisk = hardware.Harddisk.IdHarddisk;

            try
            {
                return service.HardwareCreate(h);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool DeleteHardware(HardwareModel hardware)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            int id = hardware.IdHardware;

            try
            {
                return service.HardwareDelete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}