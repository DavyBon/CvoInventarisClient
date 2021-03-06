﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using System.Globalization;
using CvoInventarisClient.DAL;

namespace CvoInventarisClient.Controllers
{
    [Authorize]
    public class FactuurController : Controller
    {

        #region Get Index

        // INDEX:
        public ActionResult Index(int? amount, string order)
        {
            ViewBag.action = TempData["action"];

            FactuurViewModel model = new FactuurViewModel();


            TblFactuur TblFactuur = new TblFactuur();
            TblLeverancier TblLeverancier = new TblLeverancier();

            model.Facturen = new List<FactuurModel>();
            model.Leveranciers = new List<SelectListItem>();

            model.Facturen = TblFactuur.GetAll().OrderBy(f => f.Id).Reverse().ToList();

            foreach (LeverancierModel l in TblLeverancier.GetAll().OrderBy(x => x.Naam))
            {
                model.Leveranciers.Add(new SelectListItem { Text = l.Naam, Value = l.Id.ToString() });
            }


            if (amount == null)
            {
                model.Facturen = model.Facturen.Take(100).ToList();
                ViewBag.amount = "100";
            }
            else
            {
                model.Facturen = model.Facturen.Take((int)amount).ToList();
                ViewBag.amount = amount.ToString();
            }

            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("Oudst"))
                {
                    model.Facturen.Reverse();
                }
                else if (order.Equals("Leverancier"))
                {
                    model.Facturen = model.Facturen.OrderBy(f => f.Leverancier.Naam).ToList();
                }
                ViewBag.ordertype = order.ToString();
            }
            else
            {
                ViewBag.ordertype = "Meest recent";
            }

            ViewBag.Heading = this.ControllerContext.RouteData.Values["controller"].ToString() + " (" + model.Facturen.Count() + ")";

            return View(model);
        }

        #endregion

        #region Get Create View

        // CREATE:

        public ActionResult Create()
        {
            FactuurViewModel model = new FactuurViewModel();

            TblFactuur TblFactuur = new TblFactuur();
            TblLeverancier TblLeverancier = new TblLeverancier();

            model.Facturen = new List<FactuurModel>();
            model.Leveranciers = new List<SelectListItem>();

            foreach (LeverancierModel l in TblLeverancier.GetAll().OrderBy(x => x.Naam))
            {
                model.Leveranciers.Add(new SelectListItem { Text = l.Naam, Value = l.Id.ToString() });
            }

            return View(model);
        }

        #endregion

        #region Create

        [HttpPost]
        public ActionResult Create(int? leverancier, string prijs, int? garantie, int? afschrijfperiode)
        {
            TblFactuur TblFactuur = new TblFactuur();

            FactuurModel factuur = new FactuurModel();

            factuur.CvoFactuurNummer = Request.Form["cvofactuurnummer"];
            factuur.LeverancierFactuurNummer = Request.Form["leverancierfactuurnummer"];
            factuur.VerwerkingsDatum = Request.Form["verwerkingsdatum"];
            factuur.ScholengroepNummer = Request.Form["scholengroepnummer"];
            factuur.Leverancier = new LeverancierModel() { Id = leverancier };
            if (!string.IsNullOrWhiteSpace(prijs))
            {
                factuur.Prijs = Convert.ToDecimal(prijs.Replace(".", ","));
            }
            factuur.Garantie = garantie;
            factuur.Omschrijving = Request.Form["omschrijving"];
            factuur.Afschrijfperiode = afschrijfperiode;

            TblFactuur.Create(factuur);

            TempData["action"] = "factuur met factuurnummer" + " " + Request.Form["cvofactuurnummer"] + " werd toegevoegd";
            return RedirectToAction("Index");
        }

        #endregion

        #region Get Edit view

        // EDIT:
        public ActionResult Edit(int id)
        {
            TblFactuur TblFactuur = new TblFactuur();
            TblLeverancier TblLeverancier = new TblLeverancier();

            FactuurViewModel model = new FactuurViewModel();
            model.Facturen = new List<FactuurModel>();
            model.Leveranciers = new List<SelectListItem>();

            FactuurModel factuur = TblFactuur.GetById(id);
            model.Facturen.Add(factuur);

            foreach (LeverancierModel l in TblLeverancier.GetAll().OrderBy(x => x.Naam))
            {
                if (!(l.Id == factuur.Leverancier.Id))
                {
                    model.Leveranciers.Add(new SelectListItem { Text = l.Naam, Value = l.Id.ToString() });
                }
            }
            return View(model);
        }

        #endregion

        #region Edit

        [HttpPost]
        public ActionResult Edit(int id, string prijs, int? leverancier, int? garantie, int? afschrijfperiode)
        {

            TblFactuur TblFactuur = new TblFactuur();

            FactuurModel factuur = new FactuurModel();
            factuur.Id = Convert.ToInt16(Request.Form["idFactuur"]);
            factuur.CvoFactuurNummer = Request.Form["cvofactuurnummer"];
            factuur.LeverancierFactuurNummer = Request.Form["leverancierfactuurnummer"];
            factuur.VerwerkingsDatum = Request.Form["verwerkingsdatum"];
            factuur.ScholengroepNummer = Request.Form["scholengroepnummer"];
            if (!string.IsNullOrWhiteSpace(prijs))
            {
                factuur.Prijs = Convert.ToDecimal(prijs.Replace(".", ","));
            }
            factuur.Garantie = garantie;
            factuur.Omschrijving = Request.Form["omschrijving"];
            factuur.Afschrijfperiode = afschrijfperiode;
            factuur.Leverancier = new LeverancierModel() { Id = leverancier };

            TblFactuur.Update(factuur);

            TempData["action"] = "factuur met factuurnummer " + Request.Form["factuurNummer"] + " werd aangepast";
            return RedirectToAction("Index");
        }

        #endregion

        #region Delete

        // DELETE:
        [HttpPost]
        public ActionResult Delete(int[] idArray, FormCollection collection)
        {
            if (idArray == null) { return RedirectToAction("Index"); }
            TblFactuur TblFactuur = new TblFactuur();

            foreach (int id in idArray)
            {
                TblFactuur.Delete(id);
            }

            if (idArray.Length >= 2)
            {
                TempData["action"] = idArray.Length + " facturen werden verwijderd";
            }
            else
            {
                TempData["action"] = idArray.Length + " factuur werd verwijderd";
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Filter

        // FILTER:
        [HttpPost]
        public ActionResult Filter(string cvoFactuurNummerFilter, string leverancierFactuurNummerFilter, string verwerkingsDatumFilter,
            string scholengroepNummerFilter, int leverancierFilter, string prijsFilter, string prijsFilterSecondary,
            string garantieFilter, string garantieFilterSecondary, string afschrijfperiodeFilter,
            string afschrijfperiodeFilterSecondary, int[] modelList)
        {
            ViewBag.action = TempData["action"];

            FactuurViewModel model = new FactuurViewModel();


            TblFactuur TblFactuur = new TblFactuur();
            TblLeverancier TblLeverancier = new TblLeverancier();

            model.Facturen = new List<FactuurModel>();
            model.Leveranciers = new List<SelectListItem>();

            model.Facturen = TblFactuur.GetAll().OrderBy(f => f.Id).Reverse().ToList();

            foreach (LeverancierModel l in TblLeverancier.GetAll())
            {
                model.Leveranciers.Add(new SelectListItem { Text = l.Naam, Value = l.Id.ToString() });
            }



            // Hier start filteren

            if (!String.IsNullOrWhiteSpace(cvoFactuurNummerFilter))
            {
                model.Facturen.RemoveAll(x => !x.CvoFactuurNummer.ToLower().Contains(cvoFactuurNummerFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(leverancierFactuurNummerFilter))
            {
                model.Facturen.RemoveAll(x => !x.LeverancierFactuurNummer.ToLower().Contains(leverancierFactuurNummerFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(verwerkingsDatumFilter))
            {
                model.Facturen.RemoveAll(x => !x.VerwerkingsDatum.ToLower().Contains(verwerkingsDatumFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(scholengroepNummerFilter))
            {
                model.Facturen.RemoveAll(x => !x.ScholengroepNummer.ToLower().Contains(scholengroepNummerFilter.ToLower()));
            }
            if (leverancierFilter >= 0)
            {
                model.Facturen.RemoveAll(x => x.Leverancier.Id != leverancierFilter);
            }
            if (!String.IsNullOrWhiteSpace(prijsFilter))
            {
                if (prijsFilterSecondary.Equals("="))
                {
                    model.Facturen.RemoveAll(x => x.Prijs != Convert.ToDecimal(prijsFilter.Replace(".", ",")));
                }
                else if (prijsFilterSecondary.Equals("<"))
                {
                    model.Facturen.RemoveAll(x => x.Prijs != Convert.ToDecimal(prijsFilter.Replace(".", ",")));
                }
                else if (prijsFilterSecondary.Equals(">"))
                {
                    model.Facturen.RemoveAll(x => x.Prijs != Convert.ToDecimal(prijsFilter.Replace(".", ",")));
                }
            }
            if (!String.IsNullOrWhiteSpace(garantieFilter))
            {
                if (garantieFilterSecondary.Equals("="))
                {
                    model.Facturen.RemoveAll(x => x.Garantie != Convert.ToInt32(garantieFilter));
                }
                else if (garantieFilterSecondary.Equals("<"))
                {
                    model.Facturen.RemoveAll(x => x.Garantie > Convert.ToInt32(garantieFilter));
                }
                else if (garantieFilterSecondary.Equals(">"))
                {
                    model.Facturen.RemoveAll(x => x.Garantie < Convert.ToInt32(garantieFilter));
                }
            }
            if (!String.IsNullOrWhiteSpace(afschrijfperiodeFilter))
            {
                if (afschrijfperiodeFilterSecondary.Equals("="))
                {
                    model.Facturen.RemoveAll(x => x.Afschrijfperiode != Convert.ToInt32(afschrijfperiodeFilter));
                }
                else if (afschrijfperiodeFilterSecondary.Equals("<"))
                {
                    model.Facturen.RemoveAll(x => x.Afschrijfperiode > Convert.ToInt32(afschrijfperiodeFilter));
                }
                else if (afschrijfperiodeFilterSecondary.Equals(">"))
                {
                    model.Facturen.RemoveAll(x => x.Afschrijfperiode < Convert.ToInt32(afschrijfperiodeFilter));
                }
            }

            ViewBag.Heading = this.ControllerContext.RouteData.Values["controller"].ToString() + " (" + model.Facturen.Count() + ")";

            decimal? totaalprijs = 0;
            foreach (var item in model.Facturen)
            {
                if (item.Prijs == null)
                {
                    item.Prijs = 0;
                }

                totaalprijs += item.Prijs;
            }

            ViewBag.totaalprijs = "(Totaal prijs: € " + totaalprijs.ToString() + ")";

            return View("Index", model);
        }

        #endregion
    }
}