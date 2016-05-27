using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;

namespace CvoInventarisClient.Controllers
{
    [Authorize]
    public class LeverancierController : Controller
    {

        // INDEX:
        public ActionResult Index(int? amount, string order, bool? refresh)
        {
            ViewBag.action = TempData["action"];

            LeverancierViewModel model = new LeverancierViewModel();

            if (Session["leverancierviewmodel"] == null || refresh == true)
            {
                DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
                DAL.TblPostcode tblPostcode = new DAL.TblPostcode();

                model.Leveranciers = new List<LeverancierModel>();
                model.Postcodes = new List<SelectListItem>();

                model.Leveranciers = tblLeverancier.GetAll().OrderBy(i => i.Id).Reverse().ToList();

                foreach (PostcodeModel p in tblPostcode.GetAll().OrderBy(p => p.Gemeente))
                {
                    model.Postcodes.Add(new SelectListItem { Text = p.Gemeente, Value = p.Id.ToString() });
                }
            }
            else
            {
                model = (LeverancierViewModel)Session["leverancierviewmodel"];
            }
            Session["leverancierviewmodel"] = model.Clone();
            if (amount == null)
            {
                model.Leveranciers = model.Leveranciers.Take(100).ToList();
                ViewBag.amount = "100";
            }
            else
            {
                model.Leveranciers = model.Leveranciers.Take((int)amount).ToList();
                ViewBag.amount = amount.ToString();
            }

            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("Oudst"))
                {
                    model.Leveranciers.Reverse();
                }
                else if (order.Equals("Naam"))
                {
                    model.Leveranciers = model.Leveranciers.OrderBy(i => i.Naam).ToList();
                }
                ViewBag.ordertype = order.ToString();
            }
            else
            {
                ViewBag.ordertype = "Meest recent";
            }

            ViewBag.Heading = this.ControllerContext.RouteData.Values["controller"].ToString() + " (" + model.Leveranciers.Count() + ")";

            return View(model);
        }

        // CREATE:
        [HttpPost]
        public ActionResult Create(int? postcode)
        {
            DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
            LeverancierModel leverancier = new LeverancierModel();
            leverancier.Naam = Request.Form["naam"];
            leverancier.Straat = Request.Form["straat"];
            leverancier.StraatNummer = Request.Form["straatNummer"];
            leverancier.BusNummer = Request.Form["busNummer"];
            leverancier.Postcode = new PostcodeModel() { Id = (int)postcode };
            leverancier.Telefoon = Request.Form["telefoon"];
            leverancier.Fax = Request.Form["fax"];
            leverancier.Email = Request.Form["email"];
            leverancier.Website = Request.Form["website"];
            leverancier.BtwNummer = Request.Form["btwNummer"];
            leverancier.ActiefDatum = Request.Form["actiefDatum"];

            tblLeverancier.Create(leverancier);

            TempData["action"] = "leverancier" + " " + Request.Form["naam"] + " werd toegevoegd";

            return RedirectToAction("Index", new { refresh = true });
        }

        // EDIT:
        public ActionResult Edit(int id)
        {
            //DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
            //return View(tblLeverancier.GetById(id));

            DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
            DAL.TblPostcode tblPostcode = new DAL.TblPostcode();
            LeverancierViewModel model = new LeverancierViewModel();

            model.Leveranciers = new List<LeverancierModel>();
            model.Postcodes = new List<SelectListItem>();

            LeverancierModel l = tblLeverancier.GetById(id);
            model.Leveranciers.Add(l);

            foreach (PostcodeModel pm in tblPostcode.GetAll())
            {
                if (!(pm.Id == l.Postcode.Id))
                {
                    model.Postcodes.Add(new SelectListItem { Text = pm.Gemeente, Value = pm.Id.ToString() });
                }
                model.Postcodes.Add(new SelectListItem { Text = pm.Postcode, Value = pm.Id.ToString() });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, int? Postcodes)
        {
            DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();

            LeverancierModel leverancier = new LeverancierModel();
            leverancier.Id = Convert.ToInt16(Request.Form["idLeverancier"]);
            leverancier.Naam = Request.Form["naam"];
            leverancier.Straat = Request.Form["straat"];
            leverancier.StraatNummer = Request.Form["straatNummer"];
            leverancier.BusNummer = Request.Form["busNummer"];

            //if (!String.IsNullOrWhiteSpace(Request.Form["postcodes"])) { leverancier.Postcode = new PostcodeModel() { Id = Convert.ToInt16(Request.Form["Postcodes"]) }; }
            //else { leverancier.Postcode = new PostcodeModel() { Id = Convert.ToInt16(Request.Form["defaultIdPostcode"]) }; }

            leverancier.Postcode = new PostcodeModel() { Id = Postcodes };

            leverancier.Telefoon = Request.Form["telefoon"];
            leverancier.Fax = Request.Form["fax"];
            leverancier.Email = Request.Form["email"];
            leverancier.Website = Request.Form["website"];
            leverancier.BtwNummer = Request.Form["btwNummer"];
            leverancier.ActiefDatum = Request.Form["actiefDatum"];

            tblLeverancier.Update(leverancier);

            TempData["action"] = "leverancier" + " " + Request.Form["naam"] + " werd gewijzigd";

            return RedirectToAction("Index", new { refresh = true });
        }

        // DELETE:
        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            if (idArray == null) { return RedirectToAction("Index"); }
            DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();

            foreach (int id in idArray)
            {
                tblLeverancier.Delete(id);
            }
            if (idArray.Length >= 2)
            {
                TempData["action"] = idArray.Length + " leveranciers werden verwijderd";
            }
            else
            {
                TempData["action"] = idArray.Length + " leverancier werd verwijderd";
            }

            return RedirectToAction("Index", new { refresh = true });
        }

        [HttpPost]
        public ActionResult Filter(string naamFilter, string straatFilter, string straatnummerFilter, string busnummerFilter,
            int postcodeFilter, string telefoonFilter, string faxFilter, string websiteFilter, string btwnummerFilter,
            string actiefDatumFilter, bool? refresh, int[] modelList)
        {
            ViewBag.action = TempData["action"];

            LeverancierViewModel model = new LeverancierViewModel();

            if (Session["leverancierviewmodel"] == null || refresh == true)
            {
                DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
                DAL.TblPostcode tblPostcode = new DAL.TblPostcode();

                model.Leveranciers = new List<LeverancierModel>();
                model.Postcodes = new List<SelectListItem>();

                model.Leveranciers = tblLeverancier.GetAll().OrderBy(i => i.Id).Reverse().ToList();

                foreach (PostcodeModel p in tblPostcode.GetAll())
                {
                    model.Postcodes.Add(new SelectListItem { Text = p.Gemeente, Value = p.Id.ToString() });
                }
            }
            else
            {
                model = (LeverancierViewModel)Session["leverancierviewmodel"];
            }

            // Hier start filteren
            if (!String.IsNullOrWhiteSpace(naamFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.Naam.ToLower().Contains(naamFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(straatFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.Straat.ToLower().Contains(straatFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(straatnummerFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.StraatNummer.ToLower().Contains(straatnummerFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(busnummerFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.BusNummer.ToLower().Contains(busnummerFilter.ToLower()));
            }
            if (postcodeFilter >= 0)
            {
                model.Leveranciers.RemoveAll(x => x.Postcode.Id != postcodeFilter);
            }
            if (!String.IsNullOrWhiteSpace(telefoonFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.Telefoon.ToLower().Contains(telefoonFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(faxFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.Fax.ToLower().Contains(faxFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(websiteFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.Fax.ToLower().Contains(websiteFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(btwnummerFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.BtwNummer.ToLower().Contains(btwnummerFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(actiefDatumFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.ActiefDatum.ToLower().Contains(actiefDatumFilter.ToLower()));
            }
            ViewBag.Heading = this.ControllerContext.RouteData.Values["controller"].ToString() + " (" + model.Leveranciers.Count() + ")";
            return View("index", model);
        }
    }
}