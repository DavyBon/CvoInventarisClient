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
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            return View(ReadAll());
        }

        private FactuurViewModel ReadAll()
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                FactuurViewModel model = new FactuurViewModel();
                model.Factuur = new List<FactuurModel>();
                model.Leverancier = new List<LeverancierModel>();

                foreach (Factuur factuur in client.FactuurGetAll())
                {
                    FactuurModel factuurModel = new FactuurModel();
                    factuurModel.IdFactuur = factuur.IdFactuur;
                    factuurModel.Boekjaar = factuur.Boekjaar;
                    factuurModel.CvoVolgNummer = factuur.CvoVolgNummer;
                    factuurModel.FactuurNummer = factuur.FactuurNummer;
                    factuurModel.FactuurDatum = factuur.FactuurDatum;
                    factuurModel.FactuurStatusGetekend = factuur.FactuurStatusGetekend;
                    factuurModel.VerwerkingsDatum = factuur.VerwerkingsDatum;
                    factuurModel.Leverancier = new LeverancierModel() { IdLeverancier = factuur.Leverancier.IdLeverancier, Naam = factuur.Leverancier.Naam, Afkorting = factuur.Leverancier.Afkorting, Straat = factuur.Leverancier.Straat, HuisNummer = factuur.Leverancier.HuisNummer, BusNummer = factuur.Leverancier.BusNummer, Postcode = factuur.Leverancier.Postcode, Telefoon = factuur.Leverancier.Telefoon, Fax = factuur.Leverancier.Fax, Email = factuur.Leverancier.Email, Website = factuur.Leverancier.Website, BtwNummer = factuur.Leverancier.BtwNummer, Iban = factuur.Leverancier.Iban, Bic = factuur.Leverancier.Bic, ToegevoegdOp = factuur.Leverancier.ToegevoegdOp};
                    factuurModel.Prijs = factuur.Prijs;
                    factuurModel.Garantie = factuur.Garantie;
                    factuurModel.Omschrijving = factuur.Omschrijving;
                    factuurModel.Opmerking = factuur.Opmerking;
                    factuurModel.Afschrijfperiode = factuur.Afschrijfperiode;
                    factuurModel.OleDoc = factuur.OleDoc;
                    factuurModel.OleDocPath = factuur.OleDocPath;
                    factuurModel.OleDocFileName = factuur.OleDocFileName;
                    factuurModel.DatumInsert = factuur.DatumInsert;
                    factuurModel.UserInsert = factuur.UserInsert;
                    factuurModel.DatumModified = factuur.DatumModified;
                    factuurModel.UserModified = factuur.UserModified;
                    model.Factuur.Add(factuurModel);
                }

                foreach (Leverancier leverancier in client.LeverancierGetAll())
                {
                    LeverancierModel leverancierModel = new LeverancierModel();
                    leverancierModel.IdLeverancier = leverancier.IdLeverancier;
                    leverancierModel.Afkorting = leverancier.Afkorting;
                    leverancierModel.Bic = leverancier.Bic;
                    leverancierModel.BtwNummer = leverancier.BtwNummer;
                    leverancierModel.BusNummer = leverancier.BusNummer;
                    leverancierModel.Email = leverancier.Email;
                    leverancierModel.Fax = leverancier.Fax;
                    leverancierModel.HuisNummer = leverancier.HuisNummer;
                    leverancierModel.Iban = leverancier.Iban;
                    leverancierModel.Naam = leverancier.Naam;
                    leverancierModel.Postcode = leverancier.Postcode;
                    leverancierModel.Straat = leverancier.Straat;
                    leverancierModel.Telefoon = leverancier.Telefoon;
                    leverancierModel.ToegevoegdOp = leverancier.ToegevoegdOp;
                    leverancierModel.Website = leverancier.Website;
                    model.Leverancier.Add(leverancierModel);
                }
                return model;
            }
        }


        // CREATE:
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Factuur factuur = new Factuur();
                factuur.Boekjaar = Request.Form["boekjaar"];
                factuur.CvoVolgNummer = Request.Form["cvoVolgNummer"];
                factuur.FactuurNummer = Request.Form["factuurNummer"];
                factuur.FactuurDatum = Convert.ToDateTime(Request.Form["factuurDatum"]);
                factuur.FactuurStatusGetekend = Boolean.Parse(Request.Form["factuurStatusGetekend"]);
                factuur.VerwerkingsDatum = Convert.ToDateTime(Request.Form["verwerkingsDatum"]);
                factuur.Leverancier.IdLeverancier = Convert.ToInt32(Request.Form["idLeverancier"]);
                factuur.Prijs = Convert.ToInt32(Request.Form["prijs"]);
                factuur.Garantie = Convert.ToInt32(Request.Form["garantie"]);
                factuur.Omschrijving = Request.Form["omschrijving"];
                factuur.Opmerking = Request.Form["opmerking"];
                factuur.Afschrijfperiode = Convert.ToInt32(Request.Form["afschrijfperiode"]);
                factuur.OleDoc = Request.Form["oleDoc"];
                factuur.OleDocPath = Request.Form["oleDocPath"];
                factuur.OleDocFileName = Request.Form["oleDocFileName"];
                factuur.DatumInsert = Convert.ToDateTime(Request.Form["datumInsert"]);
                factuur.UserInsert = Request.Form["userInsert"];
                factuur.DatumModified = Convert.ToDateTime(Request.Form["datumModified"]);
                factuur.UserModified = Request.Form["userModified"];

                client.FactuurCreate(factuur);

                TempData["action"] = "factuur met factuurnummer" + " " + Request.Form["factuurNummer"] + " werd toegevoegd";
            }
            return RedirectToAction("Index");
        }

        // EDIT:
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.FactuurGetById(id));
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Factuur factuur = new Factuur();
                factuur.Boekjaar = Request.Form["boekjaar"];
                factuur.CvoVolgNummer = Request.Form["cvoVolgNummer"];
                factuur.FactuurNummer = Request.Form["factuurNummer"];
                factuur.FactuurDatum = Convert.ToDateTime(Request.Form["factuurDatum"]);
                factuur.FactuurStatusGetekend = Boolean.Parse(Request.Form["factuurStatusGetekend"]);
                factuur.VerwerkingsDatum = Convert.ToDateTime(Request.Form["verwerkingsDatum"]);
                factuur.Leverancier.IdLeverancier = Convert.ToInt32(Request.Form["idLeverancier"]);
                factuur.Prijs = Convert.ToInt32(Request.Form["prijs"]);
                factuur.Garantie = Convert.ToInt32(Request.Form["garantie"]);
                factuur.Omschrijving = Request.Form["omschrijving"];
                factuur.Opmerking = Request.Form["opmerking"];
                factuur.Afschrijfperiode = Convert.ToInt32(Request.Form["afschrijfperiode"]);
                factuur.OleDoc = Request.Form["oleDoc"];
                factuur.OleDocPath = Request.Form["oleDocPath"];
                factuur.OleDocFileName = Request.Form["oleDocFileName"];
                factuur.DatumInsert = Convert.ToDateTime(Request.Form["datumInsert"]);
                factuur.UserInsert = Request.Form["userInsert"];
                factuur.DatumModified = Convert.ToDateTime(Request.Form["datumModified"]);
                factuur.UserModified = Request.Form["userModified"];

                client.FactuurUpdate(factuur);

                TempData["action"] = "factuur met factuurnummer" + " " + Request.Form["factuurNummer"] + " werd aangepast";
            }
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