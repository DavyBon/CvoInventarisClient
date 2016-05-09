using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers.Beheer
{
    [Authorize(Roles = "Admin")] // enkel een ingelogde admin kan gebruik maken van deze controler
    public class CpuController : Controller
    {
        // GET: Inventaris
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                List<Models.CpuModel> model = new List<Models.CpuModel>();
                foreach (Cpu cpu in client.CpuGetAll())
                {
                    model.Add(new Models.CpuModel() { IdCpu = cpu.IdCpu, Merk = cpu.Merk, Type = cpu.Type, Snelheid = cpu.Snelheid, FabrieksNummer = cpu.FabrieksNummer });
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
                Cpu cpu = new Cpu();
                cpu.Merk = Request.Form["Merk"];
                cpu.Type = Request.Form["Type"];
                cpu.Snelheid = Convert.ToInt32(Request.Form["Snelheid"]);
                cpu.FabrieksNummer = Request.Form["FabrieksNummer"];
                client.CpuCreate(cpu);

                TempData["action"] = "cpu" + " " + Request.Form["Merk"] + " werd toegevoegd";
            }
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Cpu cpu = client.CpuGetById(id);
                return View(new Models.CpuModel() { IdCpu = cpu.IdCpu, Merk = cpu.Merk, Type = cpu.Type, Snelheid = cpu.Snelheid, FabrieksNummer = cpu.FabrieksNummer });
            }
        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Cpu cpu = new Cpu();
                cpu.IdCpu = Convert.ToInt16(Request.Form["IdCpu"]);
                cpu.Merk = Request.Form["Merk"];
                cpu.Type = Request.Form["Type"];
                cpu.Snelheid = Convert.ToInt32(Request.Form["Snelheid"]);
                cpu.FabrieksNummer = Request.Form["FabrieksNummer"];

                TempData["action"] = Request.Form["Merk"] + " werd aangepast";

                client.CpuUpdate(cpu);
            }
            return RedirectToAction("Index");
        }

        // POST: Inventaris/Delete/5
        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                foreach (int id in idArray)
                {
                    client.CpuDelete(id);
                }
                if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " cpus werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " cpu werd verwijderd";
                }
            }
            return RedirectToAction("Index");
        }
    }
}