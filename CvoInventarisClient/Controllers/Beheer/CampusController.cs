using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            CampusViewModel model = new CampusViewModel();
            TblCampus TblCampus = new TblCampus();

            List<CampusModel> campusmodel = new List<CampusModel>();
            foreach (CampusModel campus in TblCampus.GetAll())
            {
                campusmodel.Add(new CampusModel() { Id = campus.Id, Naam = campus.Naam, Postcode = campus.Postcode, Straat = campus.Straat, Nummer = campus.Nummer });
            }
            model.campussen = campusmodel;
            this.Session["campusview"] = model;
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
            campus.Id = Convert.ToInt16(Request.Form["idCampus"]);
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
        [HttpPost]
        public ActionResult Filter(string naamFilter,string postcodekFilter,string straatFilter,string nummmerFilter,int[] modelList)
        {
            ViewBag.action = TempData["action"];
            CampusViewModel model = (CampusViewModel)(Session["campusview"] as CampusViewModel).Clone();
            //var new1 = new List<MyObject>(a1.Select(x => x.Clone()));

            if (!String.IsNullOrWhiteSpace(naamFilter))
            {
                model.campussen.RemoveAll(x => !x.Naam.ToLower().Contains(naamFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(postcodekFilter))
            {
                model.campussen.RemoveAll(x => !x.Postcode.ToLower().Contains(postcodekFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(straatFilter))
            {
                model.campussen.RemoveAll(x => !x.Straat.ToLower().Contains(straatFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(nummmerFilter))
            {
                model.campussen.RemoveAll(x => !x.Nummer.ToLower().Contains(nummmerFilter.ToLower()));
            }
            return View("index", model);
        }
    }
}