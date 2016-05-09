using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers.Beheer
{
    public class GrafischeKaartController : Controller
    {
        // GET: Inventaris
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                List<Models.GrafischeKaartModel> model = new List<Models.GrafischeKaartModel>();
                foreach (GrafischeKaart gk in client.GrafischeKaartGetAll())
                {
                    model.Add(new Models.GrafischeKaartModel() { IdGrafischeKaart = gk.IdGrafischeKaart, Merk = gk.Merk, Type = gk.Type, Driver = gk.Driver, FabrieksNummer = gk.FabrieksNummer });
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
                GrafischeKaart gk = new GrafischeKaart();
                gk.Merk = Request.Form["Merk"];
                gk.Type = Request.Form["Type"];
                gk.Driver = Request.Form["Driver"];
                gk.FabrieksNummer = Request.Form["FabrieksNummer"];
                client.GrafischeKaartCreate(gk);

                TempData["action"] = "grafische kaart" + " " + Request.Form["Merk"] + " werd toegevoegd";
            }
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                GrafischeKaart gk = client.GrafischeKaartGetById(id);
                return View(new Models.GrafischeKaartModel() { IdGrafischeKaart = gk.IdGrafischeKaart, Merk = gk.Merk, Type = gk.Type, Driver = gk.Driver, FabrieksNummer = gk.FabrieksNummer });
            }
        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                GrafischeKaart gk = new GrafischeKaart();
                gk.IdGrafischeKaart = Convert.ToInt16(Request.Form["IdGrafischeKaart"]);
                gk.Merk = Request.Form["Merk"];
                gk.Type = Request.Form["Type"];
                gk.Driver = Request.Form["Driver"];
                gk.FabrieksNummer = Request.Form["FabrieksNummer"];

                TempData["action"] = Request.Form["Merk"] + " werd aangepast";

                client.GrafischeKaartUpdate(gk);
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
                    client.GrafischeKaartDelete(id);
                }
                if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " grafische kaarten werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " grafische kaart werd verwijderd";
                }
            }
            return RedirectToAction("Index");
        }
    }
}