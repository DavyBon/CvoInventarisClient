using System;
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

        // INDEX:
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];

            TblFactuur TblFactuur = new TblFactuur();
            TblLeverancier TblLeverancier = new TblLeverancier();

            FactuurViewModel model = new FactuurViewModel();
            model.Facturen = new List<FactuurModel>();
            model.Leveranciers = new List<SelectListItem>();

            foreach (FactuurModel f in TblFactuur.GetAll())
            {
                model.Facturen.Add(f);
            }
            foreach (LeverancierModel l in TblLeverancier.GetAll())
            {
                model.Leveranciers.Add(new SelectListItem { Text = l.Naam, Value = l.Id.ToString() });
            }

            this.Session["factuurView"] = model;

            return View(model);
        }

        // CREATE:
        [HttpPost]
        public ActionResult Create(int? Leveranciers)
        {

            string idAccount = HttpContext.User.Identity.Name;
            TblAccount TblAccount = new TblAccount();
            AccountModel account = TblAccount.GetById(Convert.ToInt32(idAccount));
            string email = account.Email;

            TblFactuur TblFactuur = new TblFactuur();

            FactuurModel factuur = new FactuurModel();

            factuur.Boekjaar = Request.Form["boekjaar"];
            factuur.CvoVolgNummer = Request.Form["cvoVolgNummer"];
            factuur.FactuurNummer = Request.Form["factuurNummer"];
            factuur.ScholengroepNummer = Request.Form["scholengroepNummer"];
            factuur.FactuurDatum = Request.Form["factuurDatum"];
            factuur.Leverancier = new LeverancierModel() { Id = Leveranciers };
            factuur.Prijs = Request.Form["prijs"];
            factuur.Garantie = Convert.ToInt32(Request.Form["garantie"]);
            factuur.Omschrijving = Request.Form["omschrijving"];
            factuur.Opmerking = Request.Form["opmerking"];
            factuur.Afschrijfperiode = Convert.ToInt32(Request.Form["afschrijfperiode"]);
            factuur.DatumInsert = DateTime.Now.ToString("dd/MM/yyyy");
            factuur.UserInsert = email;

            TblFactuur.Create(factuur);

            TempData["action"] = "factuur met factuurnummer" + " " + Request.Form["factuurNummer"] + " werd toegevoegd";
            return RedirectToAction("Index");
        }

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

            foreach (LeverancierModel l in TblLeverancier.GetAll())
            {
                if (!(l.Id == factuur.Leverancier.Id))
                {
                    model.Leveranciers.Add(new SelectListItem { Text = l.Naam, Value = l.Id.ToString() });
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {

            string idAccount = HttpContext.User.Identity.Name;
            TblAccount TblAccount = new TblAccount();
            AccountModel account = TblAccount.GetById(Convert.ToInt32(idAccount));
            string email = account.Email;

            TblFactuur TblFactuur = new TblFactuur();

            FactuurModel factuur = new FactuurModel();
            factuur.Id = Convert.ToInt16(Request.Form["idFactuur"]);
            factuur.Boekjaar = Request.Form["boekjaar"];
            factuur.CvoVolgNummer = Request.Form["cvoVolgNummer"];
            factuur.FactuurNummer = Request.Form["factuurNummer"];
            factuur.ScholengroepNummer = Request.Form["scholengroepNummer"];
            factuur.FactuurDatum = Request.Form["factuurDatum"];
            factuur.Prijs = Request.Form["prijs"];
            factuur.Garantie = Convert.ToInt32(Request.Form["garantie"]);
            factuur.Omschrijving = Request.Form["omschrijving"];
            factuur.Opmerking = Request.Form["opmerking"];
            factuur.Afschrijfperiode = Convert.ToInt32(Request.Form["afschrijfperiode"]);
            factuur.DatumInsert = Request.Form["datumInsert"].ToString();
            factuur.UserInsert = Request.Form["userInsert"];
            factuur.DatumModified = DateTime.Now.ToString("dd/MM/yyyy");
            factuur.UserModified = email;
            if (!String.IsNullOrWhiteSpace(Request.Form["Leveranciers"])) { factuur.Leverancier = new LeverancierModel() { Id = Convert.ToInt16(Request.Form["Leveranciers"]) }; }
            else { factuur.Leverancier = new LeverancierModel() { Id = Convert.ToInt16(Request.Form["defaultIdLeverancier"]) }; }

            TblFactuur.Update(factuur);

            TempData["action"] = "factuur met factuurnummer " + Request.Form["factuurNummer"] + " werd aangepast";
            return RedirectToAction("Index");
        }



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


        // FILTER:
        [HttpPost]
        public ActionResult Filter(string boekjaarFilter, string boekjaarFilterSecondary, string cvoVolgNummerFilter, string factuurNummerFilter,
            string scholengroepNummerFilter, string factuurdatumFilter, int leverancierFilter, string prijsFilter, 
            string prijsFilterSecondary, string garantieFilter, string garantieFilterSecondary, string afschrijfperiodeFilter,
            string afschrijfperiodeFilterSecondary , int[] modelList)
        {
            ViewBag.action = TempData["action"];

            FactuurViewModel model = (FactuurViewModel)(Session["factuurView"] as FactuurViewModel).Clone();

            // Hier start filteren

            if (!String.IsNullOrWhiteSpace(boekjaarFilter))
            {
                if (boekjaarFilterSecondary.Equals("="))
                {
                    model.Facturen.RemoveAll(x => Convert.ToInt32(x.Boekjaar) != Convert.ToInt32(boekjaarFilter));
                }
                else if (boekjaarFilterSecondary.Equals("<"))
                {
                    model.Facturen.RemoveAll(x => Convert.ToInt32(x.Boekjaar) > Convert.ToInt32(boekjaarFilter));
                }
                else if (boekjaarFilterSecondary.Equals(">"))
                {
                    model.Facturen.RemoveAll(x => Convert.ToInt32(x.Boekjaar) < Convert.ToInt32(boekjaarFilter));
                }
            }
            if (!String.IsNullOrWhiteSpace(cvoVolgNummerFilter))
            {
                model.Facturen.RemoveAll(x => !x.CvoVolgNummer.ToLower().Contains(cvoVolgNummerFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(factuurNummerFilter))
            {
                model.Facturen.RemoveAll(x => !x.FactuurNummer.ToLower().Contains(factuurNummerFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(scholengroepNummerFilter))
            {
                model.Facturen.RemoveAll(x => !x.ScholengroepNummer.ToLower().Contains(scholengroepNummerFilter.ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(factuurdatumFilter))
            {
                model.Facturen.RemoveAll(x => !x.FactuurDatum.ToLower().Contains(factuurdatumFilter.ToLower()));
            }
            if (leverancierFilter >= 0)
            {
                model.Facturen.RemoveAll(x => x.Leverancier.Id != leverancierFilter);
            }
            if (!String.IsNullOrWhiteSpace(prijsFilter))
            {
                if (prijsFilterSecondary.Equals("="))
                {
                    model.Facturen.RemoveAll(x => Convert.ToInt32(x.Prijs) != Convert.ToInt32(prijsFilter));
                }
                else if (prijsFilterSecondary.Equals("<"))
                {
                    model.Facturen.RemoveAll(x => Convert.ToInt32(x.Prijs) > Convert.ToInt32(prijsFilter));
                }
                else if (prijsFilterSecondary.Equals(">"))
                {
                    model.Facturen.RemoveAll(x => Convert.ToInt32(x.Prijs) < Convert.ToInt32(prijsFilter));
                }
            }
            if (!String.IsNullOrWhiteSpace(garantieFilter))
            {
                if (garantieFilterSecondary.Equals("="))
                {
                    model.Facturen.RemoveAll(x => Convert.ToInt32(x.Garantie) != Convert.ToInt32(garantieFilter));
                }
                else if (garantieFilterSecondary.Equals("<"))
                {
                    model.Facturen.RemoveAll(x => Convert.ToInt32(x.Garantie) > Convert.ToInt32(garantieFilter));
                }
                else if (garantieFilterSecondary.Equals(">"))
                {
                    model.Facturen.RemoveAll(x => Convert.ToInt32(x.Garantie) < Convert.ToInt32(garantieFilter));
                }
            }
            if (!String.IsNullOrWhiteSpace(afschrijfperiodeFilter))
            {
                if (afschrijfperiodeFilterSecondary.Equals("="))
                {
                    model.Facturen.RemoveAll(x => Convert.ToInt32(x.Afschrijfperiode) != Convert.ToInt32(afschrijfperiodeFilter));
                }
                else if (afschrijfperiodeFilterSecondary.Equals("<"))
                {
                    model.Facturen.RemoveAll(x => Convert.ToInt32(x.Afschrijfperiode) > Convert.ToInt32(afschrijfperiodeFilter));
                }
                else if (afschrijfperiodeFilterSecondary.Equals(">"))
                {
                    model.Facturen.RemoveAll(x => Convert.ToInt32(x.Afschrijfperiode) < Convert.ToInt32(afschrijfperiodeFilter));
                }
            }

            return View("index", model);
        }
    }
}