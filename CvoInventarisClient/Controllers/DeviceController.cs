using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class DeviceController : Controller
    {
        public ActionResult Index()
        {
            return View(GetDevices());
        }

        public ActionResult Edit(int id)
        {
            return View(GetDeviceById(id));
        }

        [HttpPost]
        public ActionResult Edit(DeviceModel dm)
        {
            if (UpdateDevice(dm))
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
            return View(new DeviceModel());
        }

        [HttpPost]
        public ActionResult Create(DeviceModel dm)
        {
            if (InsertDevice(dm) >= 0)
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
            return View(GetDeviceById(id));
        }

        public ActionResult Delete(int id)
        {
            return View(GetDeviceById(id));
        }

        [HttpPost]
        public ActionResult Delete(DeviceModel dm)
        {
            if (DeleteDevice(dm))
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

        public List<DeviceModel> GetDevices()
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            Device[] d = new Device[] { };

            try
            {
                d = service.DeviceGetAll();
            }
            catch
            {
            }

            List<DeviceModel> devs = new List<DeviceModel>();

            foreach (Device dev in d)
            {
                DeviceModel dm = new DeviceModel();
                dm.IdDevice = dev.IdDevice;
                dm.Merk = dev.Merk;
                dm.Type = dev.Type;
                dm.Serienummer = dev.Serienummer;
                dm.IsPcCompatibel = dev.IsPcCompatibel;
                dm.FabrieksNummer = dev.FabrieksNummer;
                devs.Add(dm);
            }

            return devs;
        }

        public DeviceModel GetDeviceById(int id)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            Device dev = new Device();

            try
            {
                dev = service.DeviceGetById(id);
            }
            catch
            {
            }

            DeviceModel dm = new DeviceModel();
            dm.IdDevice = dev.IdDevice;
            dm.Merk = dev.Merk;
            dm.Type = dev.Type;
            dm.Serienummer = dev.Serienummer;
            dm.IsPcCompatibel = dev.IsPcCompatibel;
            dm.FabrieksNummer = dev.FabrieksNummer;

            return dm;
        }

        public bool UpdateDevice(DeviceModel dm)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            Device dev = new Device();
            dev.IdDevice = dm.IdDevice;
            dev.Merk = dm.Merk;
            dev.Type = dm.Type;
            dev.Serienummer = dm.Serienummer;
            dev.IsPcCompatibel = dm.IsPcCompatibel;
            dev.FabrieksNummer = dm.FabrieksNummer;

            try
            {
                return service.DeviceUpdate(dev);
            }
            catch
            {
                return false;
            }
        }

        public int InsertDevice(DeviceModel dm)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            Device dev = new Device();
            dev.IdDevice = dm.IdDevice;
            dev.Merk = dm.Merk;
            dev.Type = dm.Type;
            dev.Serienummer = dm.Serienummer;
            dev.IsPcCompatibel = dm.IsPcCompatibel;
            dev.FabrieksNummer = dm.FabrieksNummer;

            try
            {
                return service.DeviceCreate(dev);
            }
            catch
            {
                return -1;
            }
        }

        public bool DeleteDevice(DeviceModel dm)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            int id = dm.IdDevice;

            try
            {
                return service.DeviceDelete(id);
            }
            catch
            {
                return false;
            }
        }
    }
}