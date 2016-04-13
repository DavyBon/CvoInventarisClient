using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class CpuController : Controller
    {
        public ActionResult Index()
        {
            return View(GetCpus());
        }

        public ActionResult Edit(int id)
        {
            return View(GetCpuById(id));
        }

        [HttpPost]
        public ActionResult Edit(CpuModel cm)
        {
            if (UpdateCpu(cm))
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
            return View(new CpuModel());
        }

        [HttpPost]
        public ActionResult Create(CpuModel cm)
        {
            if (InsertCpu(cm) >= 0)
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
            return View(GetCpuById(id));
        }

        public ActionResult Delete(int id)
        {
            return View(GetCpuById(id));
        }

        [HttpPost]
        public ActionResult Delete(CpuModel cm)
        {
            if (DeleteCpu(cm))
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

        public List<CpuModel> GetCpus()
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            Cpu[] c = new Cpu[] { };

            try
            {
                c = service.CpuGetAll();
            }
            catch (Exception e)
            {

            }            

            List<CpuModel> cpus = new List<CpuModel>();

            foreach (Cpu cpu in c)
            {
                CpuModel cm = new CpuModel();
                cm.IdCpu = cpu.IdCpu;
                cm.Merk = cpu.Merk;
                cm.Type = cpu.Type;
                cm.Snelheid = cpu.Snelheid;
                cm.FabrieksNummer = cpu.FabrieksNummer;
                cpus.Add(cm);
            }

            return cpus;
        }

        public CpuModel GetCpuById(int id)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            Cpu cpu = new Cpu();

            try
            {
                cpu = service.CpuGetById(id);
            }
            catch (Exception e)
            {

            }

            CpuModel cm = new CpuModel();
            cm.IdCpu = cpu.IdCpu;
            cm.Merk = cpu.Merk;
            cm.Type = cpu.Type;
            cm.Snelheid = cpu.Snelheid;
            cm.FabrieksNummer = cpu.FabrieksNummer;

            return cm;
        }

        public bool UpdateCpu(CpuModel cm)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            Cpu cpu = new Cpu();
            cpu.IdCpu = cm.IdCpu;
            cpu.Merk = cm.Merk;
            cpu.Type = cm.Type;
            cpu.Snelheid = cm.Snelheid;
            cpu.FabrieksNummer = cm.FabrieksNummer;

            try
            {
                return service.CpuUpdate(cpu);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int InsertCpu(CpuModel cm)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            Cpu cpu = new Cpu();
            cpu.Merk = cm.Merk;
            cpu.Type = cm.Type;
            cpu.Snelheid = cm.Snelheid;
            cpu.FabrieksNummer = cm.FabrieksNummer;

            try
            {
                return service.CpuCreate(cpu);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool DeleteCpu(CpuModel cm)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            int id = cm.IdCpu;

            try
            {
                return service.CpuDelete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}