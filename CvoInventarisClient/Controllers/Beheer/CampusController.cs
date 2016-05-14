using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.ServiceReference;
using CvoInventarisClient.Models;

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
                List<CampusModel> model = new List<CampusModel>();
                foreach (Campus campus in client.CampusGetAll())
                {
                    model.Add(new CampusModel() { IdCampus = campus.IdCampus, Naam = campus.Naam, Postcode = campus.Postcode, Straat = campus.Straat, Nummer = campus.Nummer});
                }
                return View(model);
            }
        }
  
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Campus campus = new Campus();
                campus.Naam = Request.Form["naam"];
                campus.Postcode = Request.Form["postcode"];
                campus.Straat = Request.Form["straat"];
                campus.Nummer = Request.Form["nummer"];

                client.CampusCreate(campus);
                TempData["action"] = "campus met naam " + Request.Form["naam"] + " werd toegevoegd";
            }
            return RedirectToAction("Index");
        }

        // EDIT:
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Campus campus = client.CampusGetById(id);
                return View(new CampusModel() { IdCampus = campus.IdCampus, Naam = campus.Naam, Postcode = campus.Postcode, Straat = campus.Straat, Nummer = campus.Nummer });
            }
        }


        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Campus campus = new Campus();
                campus.IdCampus = Convert.ToInt16(Request.Form["idCampus"]);
                campus.Naam = Request.Form["naam"];
                campus.Postcode = Request.Form["postcode"];
                campus.Straat = Request.Form["straat"];
                campus.Nummer = Request.Form["nummer"];

                client.CampusUpdate(campus);

                TempData["action"] = "campus met naam " + Request.Form["naam"] + " werd aangepast";
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