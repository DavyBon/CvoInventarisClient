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
                h.IdHardware = hardware.IdHardware;
                h.IdCpu = hardware.IdCpu;
                h.IdDevice = hardware.IdDevice;
                h.IdGrafischeKaart = hardware.IdGrafischeKaart;
                h.IdHarddisk = hardware.IdHarddisk;
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
            h.IdHardware = hardware.IdHardware;
            h.IdCpu = hardware.IdCpu;
            h.IdDevice = hardware.IdDevice;
            h.IdGrafischeKaart = hardware.IdGrafischeKaart;
            h.IdHarddisk = hardware.IdHarddisk;
            return h;
        }

        public bool UpdateHardware(HardwareModel hardware)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            Hardware h = new Hardware();
            h.IdHardware = hardware.IdHardware;
            h.IdCpu = hardware.IdCpu;
            h.IdDevice = hardware.IdDevice;
            h.IdGrafischeKaart = hardware.IdGrafischeKaart;
            h.IdHarddisk = hardware.IdHarddisk;

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
            h.IdCpu = hardware.IdCpu;
            h.IdDevice = hardware.IdDevice;
            h.IdGrafischeKaart = hardware.IdGrafischeKaart;
            h.IdHarddisk = hardware.IdHarddisk;

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