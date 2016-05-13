using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class CampusController : Controller
    {
        // INDEX:
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                List<Models.CampusModel> model = new List<Models.CampusModel>();
                foreach (Campus campus in client.CampusGetAll())
                {
                    model.Add(new Models.CampusModel() { IdCampus = campus.IdCampus, Naam = campus.Naam, Postcode = campus.Postcode, Straat = campus.Straat, Nummer = campus.Nummer});
                }
                return View(model);
            }
        }

        // CREATE:
        // ValidateInput(false) voorkomt dat er een errormessage verschijnt bij bv:
        // < of > input in velden (potentieel gevaarlijke input error)       
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection, Models.CampusModel c)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                // Wanneer ModelState.IsValid == true
                // Nieuw object word aangemaakt adhv Request.Form
                // User wordt ge-redirect naar Index pagina
                if (ModelState.IsValid)
                {
                    Campus campus = new Campus();
                    campus.Naam = Request.Form["naam"];
                    campus.Postcode = Request.Form["postcode"];
                    campus.Straat = Request.Form["straat"];
                    campus.Nummer = Request.Form["nummer"];

                    client.CampusCreate(campus);

                    TempData["action"] = "campus met naam " + Request.Form["naam"] + " werd toegevoegd";
                }

                // Wanneer ModelState.IsValid == false
                // User wordt ge-redirect naar Index pagina en de errors zijn te zien bovenaan het scherm
                string validationErrors = string.Join(",", ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors).Select(E => E.ErrorMessage).ToArray());

                TempData["action"] = "campus niet toegevoegd: " + validationErrors;
                return RedirectToAction("Index");
            }
        }

        // EDIT:
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Campus campus = client.CampusGetById(id);
                return View(new Models.CampusModel() { IdCampus = campus.IdCampus, Naam = campus.Naam, Postcode = campus.Postcode, Straat = campus.Straat, Nummer = campus.Nummer });
            }
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, Models.CampusModel c)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                // Wanneer ModelState.IsValid == true
                // Bestaand object word aangepast adhv Request.Form
                // User wordt ge-redirect naar de Index pagina
                if (ModelState.IsValid)
                {
                    Campus campus = new Campus();
                    campus.IdCampus = id;
                    campus.Naam = Request.Form["naam"];
                    campus.Postcode = Request.Form["postcode"];
                    campus.Straat = Request.Form["straat"];
                    campus.Nummer = Request.Form["nummer"];

                    client.CampusUpdate(campus);

                    TempData["action"] = "campus met naam " + Request.Form["naam"] + " werd aangepast";
                    return RedirectToAction("Index");
                }

                // Wanneer ModelState.IsValid == false
                // Model wordt aangemaakt adhv GetById(id) methode
                // User wordt naar dezelfde edit pagina ge-redirect
                // Velden worden opnieuw ingevuld met de originele waarden + de errormessages zijn te zien
                Campus campusnv = client.CampusGetById(id);
                Models.CampusModel campusmodel = new Models.CampusModel();
                campusmodel.IdCampus = id;
                campusmodel.Naam = campusnv.Naam;
                campusmodel.Postcode = campusnv.Postcode;
                campusmodel.Straat = campusnv.Straat;
                campusmodel.Nummer = campusnv.Nummer;

                return View(campusmodel);
            }
        }


        // DELETE:        
        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                foreach (int id in idArray)
                {
                    client.CampusDelete(id);
                }
                if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " campussen werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " campus werd verwijderd";
                }
            }
            return RedirectToAction("Index");
        }
    }
}