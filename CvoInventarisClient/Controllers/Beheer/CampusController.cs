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

        #region Get Index

        // INDEX:
        public ActionResult Index(int? amount, string order, bool? refresh)
        {
            ViewBag.action = TempData["action"];

            CampusViewModel model = new CampusViewModel();

            if (Session["campusviewmodel"] == null || refresh == true)
            {
                TblCampus TblCampus = new TblCampus();
                TblPostcode TblPostcode = new TblPostcode();

                model.Campussen = new List<CampusModel>();
                model.Postcodes = new List<SelectListItem>();


                model.Campussen = TblCampus.GetAll().OrderBy(c => c.Id).Reverse().ToList();

                foreach (PostcodeModel p in TblPostcode.GetAll().OrderBy(x => x.Gemeente))
                {
                    model.Postcodes.Add(new SelectListItem { Text = p.Gemeente, Value = p.Id.ToString() });
                }
            }
            else
            {
                model = (CampusViewModel)Session["campusviewmodel"];
            }

            Session["campusviewmodel"] = model.Clone();

            if (amount == null)
            {
                model.Campussen = model.Campussen.Take(100).ToList();
                ViewBag.amount = "100";
            }
            else
            {
                model.Campussen = model.Campussen.Take((int)amount).ToList();
                ViewBag.amount = amount.ToString();
            }

            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("Oudst"))
                {
                    model.Campussen.Reverse();
                }
                else if (order.Equals("Gemeente"))
                {
                    model.Campussen = model.Campussen.OrderBy(p => p.Postcode.Gemeente).ToList();
                }
                ViewBag.ordertype = order.ToString();
            }
            else
            {
                ViewBag.ordertype = "Meest recent";
            }

            ViewBag.Heading = this.ControllerContext.RouteData.Values["controller"].ToString() + " (" + model.Campussen.Count() + ")";

            return View(model);
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            CampusViewModel model = new CampusViewModel();

            TblPostcode TblPostcode = new TblPostcode();

            model.Postcodes = new List<SelectListItem>();

            foreach (PostcodeModel p in TblPostcode.GetAll().OrderBy(x => x.Gemeente))
            {
                model.Postcodes.Add(new SelectListItem { Text = p.Gemeente + " - " + p.Postcode, Value = p.Id.ToString() });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(int? postcode)
        {
            TblCampus TblCampus = new TblCampus();

            CampusModel campus = new CampusModel();

            campus.Naam = Request.Form["naam"];
            campus.Postcode = new PostcodeModel() { Id = postcode };
            campus.Straat = Request.Form["straat"];
            campus.Nummer = Request.Form["nummer"];

            TblCampus.Create(campus);

            TempData["action"] = "campus met naam " + Request.Form["naam"] + " werd toegevoegd";

            return RedirectToAction("Index", new { refresh = true });
        }

        #endregion

        #region Get Edit view

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

            foreach (PostcodeModel p in TblPostcode.GetAll().OrderBy(x => x.Gemeente))
            {
                if (!(p.Id == c.Postcode.Id))
                {
                    model.Postcodes.Add(new SelectListItem { Text = p.Gemeente + " - " + p.Postcode, Value = p.Id.ToString() });
                }
            }

            return View(model);
        }

        #endregion

        #region Edit


        [HttpPost]
        public ActionResult Edit(int id, int? postcode)
        {
            TblCampus TblCampus = new TblCampus();

            CampusModel campus = new CampusModel();
            campus.Id = Convert.ToInt16(Request.Form["idCampus"]);
            campus.Naam = Request.Form["naam"];
            campus.Postcode = new PostcodeModel() { Id = postcode };
            campus.Straat = Request.Form["straat"];
            campus.Nummer = Request.Form["nummer"];

            TblCampus.Update(campus);

            TempData["action"] = "campus met naam " + Request.Form["naam"] + " werd aangepast";

            return RedirectToAction("Index", new { refresh = true });
        }

        #endregion

        #region Delete

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
            return RedirectToAction("Index", new { refresh = true });
        }

        #endregion

        #region Filter

        // FILTER:
        [HttpPost]
        public ActionResult Filter(string naamFilter, int postcodeFilter, string straatFilter, string nummmerFilter, bool? refresh, int[] modelList)
        {
            ViewBag.action = TempData["action"];

            CampusViewModel model = new CampusViewModel();

            if (Session["campusviewmodel"] == null || refresh == true)
            {
                TblCampus TblCampus = new TblCampus();
                TblPostcode TblPostcode = new TblPostcode();

                model.Campussen = new List<CampusModel>();
                model.Postcodes = new List<SelectListItem>();

                model.Campussen = TblCampus.GetAll().OrderBy(c => c.Id).Reverse().ToList();

                foreach (PostcodeModel p in TblPostcode.GetAll())
                {
                    model.Postcodes.Add(new SelectListItem { Text = p.Gemeente, Value = p.Id.ToString() });
                }

                Session["campusviewmodel"] = model.Clone();
            }
            else
            {
                model = (CampusViewModel)Session["campusviewmodel"];
            }

            // Hier start filteren

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

            ViewBag.Heading = this.ControllerContext.RouteData.Values["controller"].ToString() + " (" + model.Campussen.Count() + ")";

            return View("Index", model);
        }

        #endregion
    }
}