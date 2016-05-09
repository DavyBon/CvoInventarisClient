using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers.Beheer
{
    public class DeviceController : Controller
    {
        // GET: Inventaris
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                List<Models.DeviceModel> model = new List<Models.DeviceModel>();
                foreach (Device dev in client.DeviceGetAll())
                {
                    model.Add(new Models.DeviceModel() { IdDevice = dev.IdDevice, Merk = dev.Merk, Type = dev.Type, Serienummer = dev.Serienummer, IsPcCompatibel = dev.IsPcCompatibel, FabrieksNummer = dev.FabrieksNummer });
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
                Device dev = new Device();
                dev.Merk = Request.Form["Merk"];
                dev.Type = Request.Form["Type"];
                dev.Serienummer = Request.Form["Serienummer"];
                dev.IsPcCompatibel = Convert.ToBoolean(Request.Form["IsPcCompatibel"]);
                dev.FabrieksNummer = Request.Form["FabrieksNummer"];
                client.DeviceCreate(dev);

                TempData["action"] = "device" + " " + Request.Form["Merk"] + " werd toegevoegd";
            }
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Device dev = client.DeviceGetById(id);
                return View(new Models.DeviceModel() { IdDevice = dev.IdDevice, Merk = dev.Merk, Type = dev.Type, Serienummer = dev.Serienummer, IsPcCompatibel = dev.IsPcCompatibel, FabrieksNummer = dev.FabrieksNummer });
            }
        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Device dev = new Device();
                dev.IdDevice = Convert.ToInt16(Request.Form["IdDevice"]);
                dev.Merk = Request.Form["Merk"];
                dev.Type = Request.Form["Type"];
                dev.Serienummer = Request.Form["Serienummer"];
                dev.IsPcCompatibel = Convert.ToBoolean(Request.Form["IsPcCompatibel"]);
                dev.FabrieksNummer = Request.Form["FabrieksNummer"];

                TempData["action"] = Request.Form["Merk"] + " werd aangepast";

                client.DeviceUpdate(dev);
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
                    client.DeviceDelete(id);
                }
                if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " devices werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " device werd verwijderd";
                }
            }
            return RedirectToAction("Index");
        }
    }
}