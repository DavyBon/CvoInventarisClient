using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class HarddiskController : Controller
    {

        // INDEX:
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.HarddiskGetAll());
            }
        }

        // CREATE:        
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Harddisk harddisk = new Harddisk();
                harddisk.Merk = Request.Form["merk"];
                harddisk.Grootte = Convert.ToInt32(Request.Form["grootte"]);
                harddisk.FabrieksNummer = Request.Form["fabriekNummer"];

                client.HarddiskCreate(harddisk);

                TempData["action"] = "harddisk van het merk" + " " + Request.Form["merk"] + " werd toegevoegd";
            }
            return RedirectToAction("Index");
        }

        // EDIT:
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.HarddiskGetById(id));
            }
        }

        
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Harddisk harddisk = new Harddisk();
                harddisk.Merk = Request.Form["merk"];
                harddisk.Grootte = Convert.ToInt32(Request.Form["grootte"]);
                harddisk.FabrieksNummer = Request.Form["fabriekNummer"];

                client.HarddiskUpdate(harddisk);

                TempData["action"] = "harddisk van het merk" + " " + Request.Form["merk"] + " werd aangepast";
            }
            return RedirectToAction("Index");
        }


        // DELETE:        
        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                foreach (int id in idArray)
                {
                    client.HarddiskDelete(id);
                }
                if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " harddisks werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " harddisk werd verwijderd";
                }
            }
            return RedirectToAction("Index");
        }
    }
}