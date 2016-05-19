using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.ServiceReference;
using CvoInventarisClient.Models;
using CvoInventarisClient.DAL;

namespace CvoInventarisClient.Controllers
{
    [Authorize]
    public class CampusController : Controller
    {
        // INDEX:
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            TblCampus TblCampus = new TblCampus();

            List<CampusModel> model = new List<CampusModel>();
            foreach (CampusModel campus in TblCampus.GetAll())
            {
                model.Add(new CampusModel() { IdCampus = campus.IdCampus, Naam = campus.Naam, Postcode = campus.Postcode, Straat = campus.Straat, Nummer = campus.Nummer });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            TblCampus TblCampus = new TblCampus();

            CampusModel campus = new CampusModel();

            campus.Naam = Request.Form["naam"];
            campus.Postcode = Request.Form["postcode"];
            campus.Straat = Request.Form["straat"];
            campus.Nummer = Request.Form["nummer"];

            TblCampus.Create(campus);
            TempData["action"] = "campus met naam " + Request.Form["naam"] + " werd toegevoegd";

            return RedirectToAction("Index");
        }

        // EDIT:
        public ActionResult Edit(int id)
        {
            TblCampus TblCampus = new TblCampus();

            CampusModel campus = TblCampus.GetById(id);

            return View(campus);
        }


        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            TblCampus TblCampus = new TblCampus();

            CampusModel campus = new CampusModel();
            campus.IdCampus = Convert.ToInt16(Request.Form["idCampus"]);
            campus.Naam = Request.Form["naam"];
            campus.Postcode = Request.Form["postcode"];
            campus.Straat = Request.Form["straat"];
            campus.Nummer = Request.Form["nummer"];

            TblCampus.Update(campus);

            TempData["action"] = "campus met naam " + Request.Form["naam"] + " werd aangepast";
            return RedirectToAction("Index");
        }


        // DELETE:        
        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            if (idArray == null) { return RedirectToAction("Index"); }
            TblCampus TblCampus = new TblCampus();

            foreach (int id in idArray)
            {
                TblCampus.Delete(id);
            }
            if (idArray.Length >= 2)
            {
                TempData["action"] = idArray.Length + " campussen werden verwijderd";
            }
            else
            {
                TempData["action"] = idArray.Length + " campus werd verwijderd";
            }
            return RedirectToAction("Index");
        }
    }
}