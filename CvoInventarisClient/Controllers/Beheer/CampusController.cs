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
            model.Campussen = new List<CampusModel>();
            model.Postcodes = new List<SelectListItem>();

            TblCampus TblCampus = new TblCampus();
            TblPostcode TblPostcode = new TblPostcode();

            List<CampusModel> campusmodel = new List<CampusModel>();
            foreach (CampusModel c in TblCampus.GetAll())
            {
                model.Campussen.Add(c);
            }
            foreach (PostcodeModel p in TblPostcode.GetAll())
            {
                model.Postcodes.Add(new SelectListItem { Text = p.Postcode, Value = p.Id.ToString() });
            }

            this.Session["campusview"] = model;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(int? Postcodes)
        {
            TblCampus TblCampus = new TblCampus();

            CampusModel campus = new CampusModel();

            campus.Naam = Request.Form["naam"];
            campus.Postcode = new PostcodeModel() { Id = (int)Postcodes };
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
            TblPostcode TblPostcode = new TblPostcode();

            CampusViewModel model = new CampusViewModel();
            model.Campussen = new List<CampusModel>();
            model.Postcodes = new List<SelectListItem>();

            CampusModel c = TblCampus.GetById(id);
            model.Campussen.Add(c);

            foreach (PostcodeModel p in TblPostcode.GetAll())
            {
                if (!(p.Id == c.Postcode.Id))
                {
                    model.Postcodes.Add(new SelectListItem { Text = p.Postcode, Value = p.Id.ToString() });
                }
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            TblCampus TblCampus = new TblCampus();

            CampusModel campus = new CampusModel();
            campus.Id = Convert.ToInt16(Request.Form["idCampus"]);
            campus.Naam = Request.Form["naam"];
            if (!String.IsNullOrWhiteSpace(Request.Form["postcodes"])) { campus.Postcode = new PostcodeModel() { Id = Convert.ToInt16(Request.Form["Postcodes"]) }; }
            else { campus.Postcode = new PostcodeModel() { Id = Convert.ToInt16(Request.Form["defaultIdPostcode"]) }; }
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
        public ActionResult Filter(string naamFilter,int postcodeFilter,string straatFilter,string nummmerFilter,int[] modelList)
        {
            ViewBag.action = TempData["action"];
            CampusViewModel model = (CampusViewModel)(Session["campusview"] as CampusViewModel).Clone();
            //var new1 = new List<MyObject>(a1.Select(x => x.Clone()));

            if (!String.IsNullOrWhiteSpace(naamFilter))
            {
                model.Campussen.RemoveAll(x => !x.Naam.ToLower().Contains(naamFilter.ToLower()));
            }
            if (postcodeFilter >= 0)
            {
                model.Campussen.RemoveAll(x => x.Postcode.Id != postcodeFilter);
            }
            if (!String.IsNullOrWhiteSpace(straatFilter))
            {
                model.Campussen.RemoveAll(x => !x.Straat.ToLower().Contains(straatFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(nummmerFilter))
            {
                model.Campussen.RemoveAll(x => !x.Nummer.ToLower().Contains(nummmerFilter.ToLower()));
            }
            return View("index", model);
        }
    }
}