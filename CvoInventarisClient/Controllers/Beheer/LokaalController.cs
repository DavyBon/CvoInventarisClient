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
                List<Models.LokaalModel> model = new List<Models.LokaalModel>();
                foreach (Lokaal lokaal in client.LokaalGetAll())
                {
                    model.Add(new Models.LokaalModel() { IdLokaal = lokaal.IdLokaal, LokaalNaam = lokaal.LokaalNaam, AantalPlaatsen = lokaal.AantalPlaatsen, IsComputerLokaal = lokaal.IsComputerLokaal });
                }
                return View(model);
            }
        }

        // CREATE:
        // ValidateInput(false) voorkomt dat er een errormessage verschijnt bij bv:
        // < of > input in velden (potentieel gevaarlijke input error)       
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection, Models.LokaalModel lk)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                // Wanneer ModelState.IsValid == true
                // Nieuw object word aangemaakt adhv Request.Form
                // User wordt ge-redirect naar Index pagina
                if (ModelState.IsValid)
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
                    return RedirectToAction("Index");
                }

                // Wanneer ModelState.IsValid == false
                // User wordt ge-redirect naar Index pagina en de errors zijn te zien bovenaan het scherm
                string validationErrors = string.Join(",", ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors).Select(E => E.ErrorMessage).ToArray());

                TempData["action"] = "lokaal niet toegevoegd: " + validationErrors;
                return RedirectToAction("Index");
            }
        }

        // EDIT:
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Lokaal lokaal = client.LokaalGetById(id);
                return View(new Models.LokaalModel() { IdLokaal = lokaal.IdLokaal, LokaalNaam = lokaal.LokaalNaam, AantalPlaatsen = lokaal.AantalPlaatsen, IsComputerLokaal = lokaal.IsComputerLokaal });
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, LokaalModel lk)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                if (ModelState.IsValid)
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
                    return RedirectToAction("Index");
                }

                Lokaal lokaalnv = client.LokaalGetById(id);
                return View(new Models.LokaalModel() { IdLokaal = lokaalnv.IdLokaal, LokaalNaam = lokaalnv.LokaalNaam, AantalPlaatsen = lokaalnv.AantalPlaatsen, IsComputerLokaal = lokaalnv.IsComputerLokaal });
            }
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