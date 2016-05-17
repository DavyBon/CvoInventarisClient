using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;
using System.Globalization;
using CvoInventarisClient.DAL;

namespace CvoInventarisClient.Controllers
{
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

            foreach (FactuurModel factuur in TblFactuur.GetAll())
            {

                FactuurModel factuurModel = new FactuurModel();
                factuurModel.IdFactuur = factuur.IdFactuur;
                factuurModel.Boekjaar = factuur.Boekjaar;
                factuurModel.CvoVolgNummer = factuur.CvoVolgNummer;
                factuurModel.FactuurNummer = factuur.FactuurNummer;
                factuurModel.ScholengroepNummer = factuur.ScholengroepNummer;
                factuurModel.FactuurDatum = factuur.FactuurDatum;
                factuurModel.Leverancier = new LeverancierModel() { IdLeverancier = factuur.Leverancier.IdLeverancier, Naam = factuur.Leverancier.Naam, Afkorting = factuur.Leverancier.Afkorting, Straat = factuur.Leverancier.Straat, HuisNummer = factuur.Leverancier.HuisNummer, BusNummer = factuur.Leverancier.BusNummer, Postcode = factuur.Leverancier.Postcode, Telefoon = factuur.Leverancier.Telefoon, Fax = factuur.Leverancier.Fax, Email = factuur.Leverancier.Email, Website = factuur.Leverancier.Website, BtwNummer = factuur.Leverancier.BtwNummer, Iban = factuur.Leverancier.Iban, Bic = factuur.Leverancier.Bic, ToegevoegdOp = factuur.Leverancier.ToegevoegdOp };
                factuurModel.Prijs = factuur.Prijs;
                factuurModel.Garantie = factuur.Garantie;
                factuurModel.Omschrijving = factuur.Omschrijving;
                factuurModel.Opmerking = factuur.Opmerking;
                factuurModel.Afschrijfperiode = factuur.Afschrijfperiode;
                factuurModel.DatumInsert = factuur.DatumInsert;
                factuurModel.UserInsert = factuur.UserInsert;
                factuurModel.DatumModified = factuur.DatumModified;
                factuurModel.UserModified = factuur.UserModified;
                model.Facturen.Add(factuurModel);
            }
            //foreach (Leverancier l in TblLeverancier.GetAll())
            //{
            //    model.Leveranciers.Add(new SelectListItem { Text = l.Naam, Value = l.IdLeverancier.ToString() });
            //}
            return View(model);
        }

        // CREATE:
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            TblFactuur TblFactuur = new TblFactuur();

            FactuurModel factuur = new FactuurModel();

            factuur.Boekjaar = Request.Form["boekjaar"];
            factuur.CvoVolgNummer = Request.Form["cvoVolgNummer"];
            factuur.FactuurNummer = Request.Form["factuurNummer"];
            factuur.ScholengroepNummer = Request.Form["scholengroepNummer"];
            factuur.FactuurDatum = Convert.ToDateTime(Request.Form["factuurDatum"]);
            factuur.Leverancier = new LeverancierModel() { IdLeverancier = Convert.ToInt16(Request.Form["Leveranciers"]) };
            factuur.Prijs = Request.Form["prijs"];
            factuur.Garantie = Convert.ToInt32(Request.Form["garantie"]);
            factuur.Omschrijving = Request.Form["omschrijving"];
            factuur.Opmerking = Request.Form["opmerking"];
            factuur.Afschrijfperiode = Convert.ToInt32(Request.Form["afschrijfperiode"]);
            factuur.DatumInsert = Convert.ToDateTime(Request.Form["datumInsert"]);
            factuur.UserInsert = Request.Form["userInsert"];
            if (String.IsNullOrWhiteSpace(Request.Form["datumModified"])) { factuur.DatumModified = Convert.ToDateTime(Request.Form["datumInsert"]); }
            else { factuur.DatumModified = Convert.ToDateTime(Request.Form["datumModified"]); }
            if (String.IsNullOrWhiteSpace(Request.Form["userModified"])) { factuur.UserModified = Request.Form["userInsert"]; }
            else { factuur.UserModified = Request.Form["userModified"]; }

            TblFactuur.Create(factuur);

            TempData["action"] = "factuur met factuurnummer" + " " + Request.Form["factuurNummer"] + " werd toegevoegd";
            return RedirectToAction("Index");
        }

        // EDIT:
        [HttpGet]
        public ActionResult Edit(int id)
        {
            TblFactuur TblFactuur = new TblFactuur();
            TblLeverancier TblLeverancier = new TblLeverancier();

            FactuurViewModel model = new FactuurViewModel();
            model.Facturen = new List<FactuurModel>();
            model.Leveranciers = new List<SelectListItem>();

            FactuurModel factuur = TblFactuur.GetById(id);
            model.Facturen.Add(factuur);

            //foreach (Leverancier l in TblLeverancier.GetAll())
            //{
            //    if (!(l.IdLeverancier == factuur.Leverancier.IdLeverancier))
            //    {
            //        model.Leveranciers.Add(new SelectListItem { Text = l.Naam, Value = l.IdLeverancier.ToString() });
            //    }
            //}
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            TblFactuur TblFactuur = new TblFactuur();

            FactuurModel factuur = new FactuurModel();
            factuur.IdFactuur = Convert.ToInt16(Request.Form["idFactuur"]);
            factuur.Boekjaar = Request.Form["boekjaar"];
            factuur.CvoVolgNummer = Request.Form["cvoVolgNummer"];
            factuur.FactuurNummer = Request.Form["factuurNummer"];
            factuur.ScholengroepNummer = Request.Form["scholengroepNummer"];
            factuur.FactuurDatum = Convert.ToDateTime(Request.Form["factuurDatum"]);
            factuur.Prijs = Request.Form["prijs"];
            factuur.Garantie = Convert.ToInt32(Request.Form["garantie"]);
            factuur.Omschrijving = Request.Form["omschrijving"];
            factuur.Opmerking = Request.Form["opmerking"];
            factuur.Afschrijfperiode = Convert.ToInt32(Request.Form["afschrijfperiode"]);
            factuur.DatumInsert = Convert.ToDateTime(Request.Form["datumInsert"]);
            factuur.UserInsert = Request.Form["userInsert"];
            if (String.IsNullOrWhiteSpace(Request.Form["datumModified"])) { factuur.DatumModified = Convert.ToDateTime(Request.Form["datumInsert"]); }
            else { factuur.DatumModified = Convert.ToDateTime(Request.Form["datumModified"]); }
            if (String.IsNullOrWhiteSpace(Request.Form["userModified"])) { factuur.UserModified = Request.Form["userInsert"]; }
            else { factuur.UserModified = Request.Form["userModified"]; }
            if (!String.IsNullOrWhiteSpace(Request.Form["Leveranciers"])) { factuur.Leverancier = new LeverancierModel() { IdLeverancier = Convert.ToInt16(Request.Form["Leveranciers"]) }; }
            else { factuur.Leverancier = new LeverancierModel() { IdLeverancier = Convert.ToInt16(Request.Form["defaultIdLeverancier"]) }; }

            TblFactuur.Update(factuur);

            TempData["action"] = "factuur met factuurnummer " + Request.Form["factuurNummer"] + " werd aangepast";
            return RedirectToAction("Index");
        }



        // DELETE:
        [HttpPost]
        public ActionResult Delete(int[] idArray, FormCollection collection)
        {
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
    }
}