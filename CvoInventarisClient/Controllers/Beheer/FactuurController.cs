using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class FactuurController : Controller
    {
        // INDEX:
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                FactuurViewModel model = new FactuurViewModel();
                model.Factuur = new List<FactuurModel>();
                model.Leverancier = new List<SelectListItem>();

                foreach (Factuur factuur in client.FactuurGetAll())
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
                    model.Factuur.Add(factuurModel);
                }
                foreach (Leverancier l in client.LeverancierGetAll())
                {
                    if (!model.Factuur.Exists(f => f.Leverancier.IdLeverancier == l.IdLeverancier)) { model.Leverancier.Add(new SelectListItem { Text = l.Naam, Value = l.IdLeverancier.ToString() }); }
                }
                return View(model);
            }
        }

        // CREATE:
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Factuur factuur = new Factuur();
                factuur.Boekjaar = Request.Form["boekjaar"];
                factuur.CvoVolgNummer = Request.Form["cvoVolgNummer"];
                factuur.FactuurNummer = Request.Form["factuurNummer"];
                factuur.ScholengroepNummer = Request.Form["scholengroepNummer"];
                factuur.FactuurDatum = Convert.ToDateTime(Request.Form["factuurDatum"]);
                factuur.Leverancier = new Leverancier() { IdLeverancier = Convert.ToInt16(Request.Form["Leverancier"]) };
                factuur.Prijs = Convert.ToInt32(Request.Form["prijs"]);
                factuur.Garantie = Convert.ToInt32(Request.Form["garantie"]);
                factuur.Omschrijving = Request.Form["omschrijving"];
                factuur.Opmerking = Request.Form["opmerking"];
                factuur.Afschrijfperiode = Convert.ToInt32(Request.Form["afschrijfperiode"]);
                factuur.DatumInsert = Convert.ToDateTime(Request.Form["datumInsert"]);
                factuur.UserInsert = Request.Form["userInsert"];
                factuur.DatumModified = Convert.ToDateTime(Request.Form["datumModified"]);
                factuur.UserModified = Request.Form["userModified"];

                client.FactuurCreate(factuur);
            }

            TempData["action"] = "factuur met factuurnummer" + " " + Request.Form["factuurNummer"] + " werd toegevoegd";
            return RedirectToAction("Index");
        }

        // EDIT:
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                FactuurViewModel model = new FactuurViewModel();
                model.Factuur = new List<FactuurModel>();
                model.Leverancier = new List<SelectListItem>();

                Factuur f = client.FactuurGetById(id);
                FactuurModel factuur = new FactuurModel();
                factuur.IdFactuur = f.IdFactuur;
                factuur.Boekjaar = f.Boekjaar;
                factuur.CvoVolgNummer = f.CvoVolgNummer;
                factuur.FactuurNummer = f.FactuurNummer;
                factuur.ScholengroepNummer = f.ScholengroepNummer;
                factuur.FactuurDatum = f.FactuurDatum;
                factuur.Prijs = f.Prijs;
                factuur.Garantie = f.Garantie;
                factuur.Omschrijving = f.Omschrijving;
                factuur.Opmerking = f.Opmerking;
                factuur.Afschrijfperiode = f.Afschrijfperiode;
                factuur.DatumInsert = f.DatumInsert;
                factuur.UserInsert = f.UserInsert;
                factuur.DatumModified = f.DatumModified;
                factuur.UserModified = f.UserModified;
                factuur.Leverancier = new LeverancierModel()
                {
                    IdLeverancier = f.Leverancier.IdLeverancier,
                    Naam = f.Leverancier.Naam,
                    Afkorting = f.Leverancier.Afkorting,
                    Straat = f.Leverancier.Straat,
                    HuisNummer = f.Leverancier.HuisNummer,
                    BusNummer = f.Leverancier.BusNummer,
                    Postcode = f.Leverancier.Postcode,
                    Telefoon = f.Leverancier.Telefoon,
                    Fax = f.Leverancier.Fax,
                    Email = f.Leverancier.Email,
                    Website = f.Leverancier.Website,
                    BtwNummer = f.Leverancier.BtwNummer,
                    Iban = f.Leverancier.Iban,
                    Bic = f.Leverancier.Bic,
                    ToegevoegdOp = f.Leverancier.ToegevoegdOp
                };
                model.Factuur.Add(factuur);

                foreach (Leverancier l in client.LeverancierGetAll())
                {
                    if (!(l.IdLeverancier == factuur.Leverancier.IdLeverancier))
                    {
                        model.Leverancier.Add(new SelectListItem { Text = l.Naam, Value = l.IdLeverancier.ToString() });
                    }
                }
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Factuur factuur = new Factuur();
                factuur.IdFactuur = Convert.ToInt16(Request.Form["idFactuur"]);
                factuur.Boekjaar = Request.Form["boekjaar"];
                factuur.CvoVolgNummer = Request.Form["cvoVolgNummer"];
                factuur.FactuurNummer = Request.Form["factuurNummer"];
                factuur.ScholengroepNummer = Request.Form["scholengroepNummer"];
                factuur.FactuurDatum = Convert.ToDateTime(Request.Form["factuurDatum"]);
                factuur.Prijs = Convert.ToInt32(Request.Form["prijs"]);
                factuur.Garantie = Convert.ToInt32(Request.Form["garantie"]);
                factuur.Omschrijving = Request.Form["omschrijving"];
                factuur.Opmerking = Request.Form["opmerking"];
                factuur.Afschrijfperiode = Convert.ToInt32(Request.Form["afschrijfperiode"]);
                factuur.DatumInsert = Convert.ToDateTime(Request.Form["datumInsert"]);
                factuur.UserInsert = Request.Form["userInsert"];
                factuur.DatumModified = Convert.ToDateTime(Request.Form["datumModified"]);
                factuur.UserModified = Request.Form["userModified"];
                if (!String.IsNullOrWhiteSpace(Request.Form["Leverancier"])) { factuur.Leverancier = new Leverancier() { IdLeverancier = Convert.ToInt16(Request.Form["Leverancier"]) }; }
                else { factuur.Leverancier = new Leverancier() { IdLeverancier = Convert.ToInt16(Request.Form["defaultIdLeverancier"]) }; }

                client.FactuurUpdate(factuur);

            }
            TempData["action"] = "factuur met factuurnummer " + Request.Form["factuurNummer"] + " werd aangepast";
            return RedirectToAction("Index");
        }


        // DELETE:
        [HttpPost]
        public ActionResult Delete(int[] idArray, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                foreach (int id in idArray)
                {
                    client.FactuurDelete(id);
                }
                if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " facturen werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " factuur werd verwijderd";
                }
            }
            return RedirectToAction("Index");
        }
    }
}