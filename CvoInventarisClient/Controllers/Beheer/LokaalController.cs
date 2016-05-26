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
        public ActionResult Index(int? amount, string order, bool? refresh)
        {
            ViewBag.action = TempData["action"];

            LokaalViewModel model = new LokaalViewModel();

            if (Session["lokaalviewmodel"] == null || refresh == true)
            {
                DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
                DAL.TblCampus tblCampus = new DAL.TblCampus();

                model.Lokalen = new List<LokaalModel>();
                model.Campussen = new List<SelectListItem>();

                model.Lokalen = tblLokaal.GetAll().OrderBy(i => i.Id).Reverse().ToList();

                foreach (CampusModel c in tblCampus.GetAll())
                {
                    model.Campussen.Add(new SelectListItem { Text = c.Naam, Value = c.Id.ToString() });
                }
            }
            else
            {
                model = (LokaalViewModel)Session["lokaalviewmodel"];
            }
            Session["lokaalviewmodel"] = model.Clone();
            if (amount == null)
            {
                model.Lokalen = model.Lokalen.Take(100).ToList();
                ViewBag.amount = "100";
            }
            else
            {
                model.Lokalen = model.Lokalen.Take((int)amount).ToList();
                ViewBag.amount = amount.ToString();
            }

            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("Oudst"))
                {
                    model.Lokalen.Reverse();
                }
                else if (order.Equals("Lokaal"))
                {
                    model.Lokalen = model.Lokalen.OrderBy(i => i.Id).ToList();
                }
                ViewBag.ordertype = order.ToString();
            }
            else
            {
                ViewBag.ordertype = "Meest recent";
            }

            ViewBag.Heading = this.ControllerContext.RouteData.Values["controller"].ToString() + " (" + model.Lokalen.Count() + ")";

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

            return RedirectToAction("Index", new { refresh = true });
        }

        // EDIT:
        public ActionResult Edit(int id)
        {
            DAL.TblLokaal TblLokaal = new DAL.TblLokaal();
            DAL.TblCampus TblCampus = new DAL.TblCampus();

            LokaalViewModel model = new LokaalViewModel();
            model.Lokalen = new List<LokaalModel>();
            model.Campussen = new List<SelectListItem>();

            LokaalModel l = TblLokaal.GetById(id);
            model.Lokalen.Add(l);

            foreach (CampusModel c in TblCampus.GetAll())
            {
                if (!(c.Id == l.Campus.Id))
                {
                    model.Campussen.Add(new SelectListItem { Text = c.Naam, Value = c.Id.ToString() });
                }
            }
            return View(model);
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

            TempData["action"] = "lokaal " + Request.Form["lokaalNaam"] + " werd gewijzigd";

            return RedirectToAction("Index", new { refresh = true });
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

            return RedirectToAction("Index", new { refresh = true });
        }

        [HttpPost]
        public ActionResult Filter(string computerLokaalFilter, string lokaalNaamFilter, string filterAantalPlaatsenSecondary,
            string filterAantalPlaatsen, int campusFilter, int[] modelList)
        {
            ViewBag.action = TempData["action"];

            LokaalViewModel model = new LokaalViewModel();

            if (Session["lokaalviewmodel"] == null)
            {
                DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
                DAL.TblCampus tblCampus = new DAL.TblCampus();

                model.Lokalen = new List<LokaalModel>();
                model.Campussen = new List<SelectListItem>();

                model.Lokalen = tblLokaal.GetAll().OrderBy(i => i.Id).Reverse().ToList();

                foreach (CampusModel c in tblCampus.GetAll())
                {
                    model.Campussen.Add(new SelectListItem { Text = c.Naam, Value = c.Id.ToString() });
                }
            }
            else
            {
                model = (LokaalViewModel)Session["lokaalviewmodel"];
            }

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