using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;

namespace CvoInventarisClient.Controllers
{
    [Authorize]
    public class LokaalController : Controller
    {
        // INDEX:
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
            DAL.TblCampus tblCampus = new DAL.TblCampus();

            LokaalViewModel model = new LokaalViewModel();
            model.Lokalen = new List<LokaalModel>();
            model.Campussen = new List<SelectListItem>();

            foreach (LokaalModel i in tblLokaal.GetAll())
            {
                model.Lokalen.Add(i);
            }
            foreach (CampusModel v in tblCampus.GetAll())
            {
                model.Campussen.Add(new SelectListItem { Text = v.Naam, Value = v.Id.ToString() });
            }

            this.Session["lokaalview"] = model;

            return View(model);            
        }
     
        [HttpPost]
        public ActionResult Create(int? Campussen)
        {
            DAL.TblLokaal tblLokaal = new DAL.TblLokaal();

            LokaalModel lokaal = new LokaalModel();
            lokaal.LokaalNaam = Request.Form["lokaalNaam"];
            lokaal.AantalPlaatsen = Convert.ToInt32(Request.Form["aantalPlaatsen"]);
            lokaal.Campus = new CampusModel() { Id = Campussen };
            
            if (Request.Form["isComputerLokaal"] != null)
            {
                lokaal.IsComputerLokaal = true;
            }
            else
            {
                lokaal.IsComputerLokaal = false;
            }

            tblLokaal.Create(lokaal);

            TempData["action"] = "lokaal" + " " + Request.Form["lokaalNaam"] + " werd toegevoegd";

            return RedirectToAction("Index");
        }

        // EDIT:
        public ActionResult Edit(int id)
        {
                DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
                return View(tblLokaal.GetById(id));
        }

        [HttpPost]
        public ActionResult Edit(int? Campussen)
        {
            LokaalModel lokaal = new LokaalModel();
            lokaal.Id = Convert.ToInt16(Request.Form["idLokaal"]);
            lokaal.LokaalNaam = Request.Form["lokaalNaam"];
            lokaal.AantalPlaatsen = Convert.ToInt32(Request.Form["aantalPlaatsen"]);
            lokaal.Campus = new CampusModel() { Id = Campussen };

            if (Request.Form["isComputerLokaal"] != null) { lokaal.IsComputerLokaal = true; }
            else
            {
                lokaal.IsComputerLokaal = false;
            }

            DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
            tblLokaal.Update(lokaal);

            TempData["action"] = "lokaal " + Request.Form["lokaalNaam"] + " werd aangepast";

            return RedirectToAction("Index");
        }


        // DELETE:
        [HttpPost]
        public ActionResult Delete(int[] idArray, FormCollection collection)
        {
            if (idArray == null) { return RedirectToAction("Index"); }
            DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
            foreach (int id in idArray)
            {
                tblLokaal.Delete(id);
            }
            if (idArray.Length >= 2)
            {
                TempData["action"] = idArray.Length + " lokalen werden verwijderd";
            }
            else
            {
                TempData["action"] = idArray.Length + " lokaal werd verwijderd";
            }

            return RedirectToAction("Index");            
        }

        [HttpPost]
        public ActionResult Filter(string computerLokaalFilter, string lokaalNaamFilter, string filterAantalPlaatsenSecondary, 
            string filterAantalPlaatsen, int campusFilter, int[] modelList)
        {
            ViewBag.action = TempData["action"];

            LokaalViewModel model = (LokaalViewModel)(Session["lokaalview"] as LokaalViewModel).Clone();

            // Hier start filteren
            if (!String.IsNullOrWhiteSpace(computerLokaalFilter))
            {
                if (computerLokaalFilter.Equals("true"))
                {
                    model.Lokalen.RemoveAll(x => x.IsComputerLokaal != true);
                }
                else
                {
                    model.Lokalen.RemoveAll(x => x.IsComputerLokaal != false);
                }
            }

            if (!String.IsNullOrWhiteSpace(lokaalNaamFilter))
            {
                model.Lokalen.RemoveAll(x => !x.LokaalNaam.ToLower().Contains(lokaalNaamFilter.ToLower()));
            }

            if (!String.IsNullOrWhiteSpace(filterAantalPlaatsen))
            {
                if (filterAantalPlaatsenSecondary.Equals("="))
                {
                    model.Lokalen.RemoveAll(x => x.AantalPlaatsen != Convert.ToInt32(filterAantalPlaatsen));
                }
                else if (filterAantalPlaatsenSecondary.Equals("<"))
                {
                    model.Lokalen.RemoveAll(x => x.AantalPlaatsen > Convert.ToInt32(filterAantalPlaatsen));
                }
                else if (filterAantalPlaatsenSecondary.Equals(">"))
                {
                    model.Lokalen.RemoveAll(x => x.AantalPlaatsen < Convert.ToInt32(filterAantalPlaatsen));
                }
            }            

            if (campusFilter >= 0)
            {
                model.Lokalen.RemoveAll(x => x.Campus.Id != campusFilter);
            }

            return View("index", model);
        }
    }
}