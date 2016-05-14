using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class LokaalController : Controller
    {
        // INDEX:
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                List<LokaalModel> model = new List<LokaalModel>();
                foreach (Lokaal lokaal in client.LokaalGetAll())
                {
                    model.Add(new LokaalModel() { IdLokaal = lokaal.IdLokaal, LokaalNaam = lokaal.LokaalNaam, AantalPlaatsen = lokaal.AantalPlaatsen, IsComputerLokaal = lokaal.IsComputerLokaal });
                }
                return View(model);
            }
        }
     
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Lokaal lokaal = new Lokaal();
                lokaal.LokaalNaam = Request.Form["lokaalNaam"];
                lokaal.AantalPlaatsen = Convert.ToInt32(Request.Form["aantalPlaatsen"]);

                if (Request.Form["isComputerLokaal"] != null)
                {
                    lokaal.IsComputerLokaal = true;
                }
                else
                {
                    lokaal.IsComputerLokaal = false;
                }

                client.LokaalCreate(lokaal);

                TempData["action"] = "lokaal" + " " + Request.Form["lokaalNaam"] + " werd toegevoegd";
            }
            return RedirectToAction("Index");
        }

        // EDIT:
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Lokaal lokaal = client.LokaalGetById(id);
                return View(new LokaalModel() { IdLokaal = lokaal.IdLokaal, LokaalNaam = lokaal.LokaalNaam, AantalPlaatsen = lokaal.AantalPlaatsen, IsComputerLokaal = lokaal.IsComputerLokaal });
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Lokaal lokaal = new Lokaal();
                lokaal.IdLokaal = Convert.ToInt16(Request.Form["idLokaal"]);
                lokaal.LokaalNaam = Request.Form["lokaalNaam"];
                lokaal.AantalPlaatsen = Convert.ToInt32(Request.Form["aantalPlaatsen"]);

                if (Request.Form["isComputerLokaal"] != null) { lokaal.IsComputerLokaal = true; }
                else
                {
                    lokaal.IsComputerLokaal = false;
                }

                client.LokaalUpdate(lokaal);

                TempData["action"] = "lokaal " + Request.Form["lokaalNaam"] + " werd aangepast";
            }
            return RedirectToAction("Index");
        }


        // DELETE:
        [HttpPost]
        public ActionResult Delete(int[] idArray, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                foreach (int id in idArray)
                {
                    client.LokaalDelete(id);
                }
                if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " lokalen werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " lokaal werd verwijderd";
                }
            }
            return RedirectToAction("Index");            
        }
    }
}