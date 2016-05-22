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
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
            LeverancierViewModel model = new LeverancierViewModel();
            model.Leveranciers = tblLeverancier.GetAll();
            this.Session["leverancierview"] = model;
            return View(model);
        }

        // CREATE:
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
            LeverancierModel leverancier = new LeverancierModel();
            leverancier.Naam = Request.Form["naam"];
            leverancier.Afkorting = Request.Form["afkorting"];
            leverancier.Straat = Request.Form["straat"];
            leverancier.HuisNummer = Request.Form["huisNummer"];
            leverancier.BusNummer = Request.Form["busNummer"];
            leverancier.Postcode = Convert.ToInt32(Request.Form["postcode"]);
            leverancier.Telefoon = Request.Form["telefoon"];
            leverancier.Fax = Request.Form["fax"];
            leverancier.Email = Request.Form["email"];
            leverancier.Website = Request.Form["website"];
            leverancier.BtwNummer = Request.Form["btwNummer"];
            leverancier.Iban = Request.Form["iban"];
            leverancier.Bic = Request.Form["bic"];
            leverancier.ToegevoegdOp = Request.Form["toegevoegdOp"];

            tblLeverancier.Create(leverancier);

            TempData["action"] = "leverancier" + " " + Request.Form["naam"] + " werd toegevoegd";

            return RedirectToAction("Index");
        }

        // EDIT:
        public ActionResult Edit(int id)
        {
            DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
            return View(tblLeverancier.GetById(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
            LeverancierModel leverancier = new LeverancierModel();
            leverancier.IdLeverancier = Convert.ToInt16(Request.Form["idLeverancier"]);
            leverancier.Naam = Request.Form["naam"];
            leverancier.Afkorting = Request.Form["afkorting"];
            leverancier.Straat = Request.Form["straat"];
            leverancier.HuisNummer = Request.Form["huisNummer"];
            leverancier.BusNummer = Request.Form["busNummer"];
            leverancier.Postcode = Convert.ToInt32(Request.Form["postcode"]);
            leverancier.Telefoon = Request.Form["telefoon"];
            leverancier.Fax = Request.Form["fax"];
            leverancier.Email = Request.Form["email"];
            leverancier.Website = Request.Form["website"];
            leverancier.BtwNummer = Request.Form["btwNummer"];
            leverancier.Iban = Request.Form["iban"];
            leverancier.Bic = Request.Form["bic"];
            leverancier.ToegevoegdOp = Request.Form["toegevoegdOp"];

            tblLeverancier.Update(leverancier);

            TempData["action"] = "leverancier" + " " + Request.Form["naam"] + " werd aangepast";

            return RedirectToAction("Index");
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

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Filter(string naamFilter, string afkortingFilter, string straatFilter, string huisnummerFilter, string busnummerFilter,
            string filterPostcodeSecondary, int filterPostcode, string telefoonFilter, string faxFilter, string websiteFilter, string btwnummerFilter,
            string ibanFilter, string bicFilter, string toegevoegdOpFilter, int[] modelList)
        {
            ViewBag.action = TempData["action"];

            LeverancierViewModel model = (LeverancierViewModel)(Session["leverancierview"] as LeverancierViewModel).Clone();

            // Hier start filteren
            if (!String.IsNullOrWhiteSpace(naamFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.Naam.ToLower().Contains(naamFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(afkortingFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.Afkorting.ToLower().Contains(afkortingFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(straatFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.Straat.ToLower().Contains(straatFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(huisnummerFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.HuisNummer.ToLower().Contains(huisnummerFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(busnummerFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.BusNummer.ToLower().Contains(busnummerFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(filterPostcode.ToString()))
            {
                if (filterPostcodeSecondary.Equals("="))
                {
                    model.Leveranciers.RemoveAll(x => !(x.Postcode != Convert.ToInt32(filterPostcode)));
                }
                else if (filterPostcodeSecondary.Equals("<"))
                {
                    model.Leveranciers.RemoveAll(x => !(x.Postcode > Convert.ToInt32(filterPostcode)));
                }
                else if (filterPostcodeSecondary.Equals(">"))
                {
                    model.Leveranciers.RemoveAll(x => !(x.Postcode < Convert.ToInt32(filterPostcode)));
                }
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
            if (!String.IsNullOrWhiteSpace(ibanFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.Iban.ToLower().Contains(ibanFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(bicFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.Bic.ToLower().Contains(bicFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(toegevoegdOpFilter))
            {
                model.Leveranciers.RemoveAll(x => !x.ToegevoegdOp.ToLower().Contains(toegevoegdOpFilter.ToLower()));
            }
            return View("index", model);
        }
    }
}