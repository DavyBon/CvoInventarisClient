using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Controllers
{
    public class InventarisController : Controller
    {
        // GET: Inventaris
        public ActionResult Index()
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                //WCF servicereference objecten collection naar InventarisModel objecten collection
                InventarisViewModel model = new InventarisViewModel();
                model.Inventaris = new List<InventarisModel>();
                model.Objecten = new List<SelectListItem>();
                model.Lokalen = new List<SelectListItem>();
                model.Verzekeringen = new List<SelectListItem>();

                foreach (Inventaris i in client.InventarisGetAll())
                {
                    InventarisModel inventaris = new InventarisModel();
                    inventaris.Id = i.Id;
                    inventaris.IsAanwezig = i.IsAanwezig;
                    inventaris.IsActief = i.IsActief;
                    inventaris.Label = i.Label;
                    inventaris.Lokaal = new LokaalModel() { IdLokaal = i.Lokaal.IdLokaal, AantalPlaatsen = i.Lokaal.AantalPlaatsen, IsComputerLokaal = i.Lokaal.IsComputerLokaal, LokaalNaam = i.Lokaal.LokaalNaam, Netwerk = new NetwerkModel() { Id = i.Lokaal.Netwerk.Id, Driver = i.Lokaal.Netwerk.Driver, Merk = i.Lokaal.Netwerk.Merk, Snelheid = i.Lokaal.Netwerk.Snelheid, Type = i.Lokaal.Netwerk.Type } };
                    inventaris.Object = new ObjectModel() { Id = i.Object.Id, Kenmerken = i.Object.Kenmerken, Leverancier = new LeverancierModel() { IdLeverancier = i.Object.Leverancier.IdLeverancier, Afkorting = i.Object.Leverancier.Afkorting, Bic = i.Object.Leverancier.Bic, BtwNummer = i.Object.Leverancier.BtwNummer, BusNummer = i.Object.Leverancier.BusNummer, Email = i.Object.Leverancier.Email, Fax = i.Object.Leverancier.Fax, HuisNummer = i.Object.Leverancier.HuisNummer, Iban = i.Object.Leverancier.Iban, Naam = i.Object.Leverancier.Naam, Postcode = i.Object.Leverancier.Postcode, Straat = i.Object.Leverancier.Straat, Telefoon = i.Object.Leverancier.Telefoon, ToegevoegdOp = i.Object.Leverancier.ToegevoegdOp, Website = i.Object.Leverancier.Website }, ObjectType = new ObjectTypeModel() { IdObjectType = i.Object.ObjectType.Id, Omschrijving = i.Object.ObjectType.Omschrijving } };
                    inventaris.Verzekering = new VerzekeringModel() { IdVerzekering = i.Verzekering.Id, Omschrijving = i.Verzekering.Omschrijving };
                    inventaris.Aankoopjaar = i.Aankoopjaar;
                    inventaris.Afschrijvingsperiode = i.Afschrijvingsperiode;
                    inventaris.Historiek = i.Historiek;
                    model.Inventaris.Add(inventaris);
                }
                foreach (ServiceReference.Object o in client.ObjectGetAll())
                {
                    if (!model.Inventaris.Exists(i => i.Object.Id == o.Id)) { model.Objecten.Add(new SelectListItem { Text = o.Kenmerken, Value = o.Id.ToString() }); }
                }
                foreach (Lokaal l in client.LokaalGetAll())
                {
                    model.Lokalen.Add(new SelectListItem { Text = l.LokaalNaam, Value = l.IdLokaal.ToString() });
                }
                foreach (Verzekering v in client.VerzekeringGetAll())
                {
                    model.Verzekeringen.Add(new SelectListItem { Text = v.Omschrijving, Value = v.Id.ToString() });
                }
                return View(model);
            }
        }

        // GET: Inventaris/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inventaris/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                ViewBag.action = Request.Form["label"] + " was added";

                Inventaris inventaris = new Inventaris();
                inventaris.Aankoopjaar = Convert.ToInt32(Request.Form["aankoopjaar"]);
                inventaris.Afschrijvingsperiode = Convert.ToInt32(Request.Form["afschrijvingsperiode"]);
                inventaris.Historiek = Request.Form["historiek"];
                inventaris.Object = new ServiceReference.Object() { Id = Convert.ToInt16(Request.Form["Objecten"]) };
                inventaris.Lokaal = new ServiceReference.Lokaal() { IdLokaal = Convert.ToInt16(Request.Form["Lokalen"]) };
                inventaris.Verzekering = new ServiceReference.Verzekering() { Id = Convert.ToInt16(Request.Form["Verzekeringen"]) };
                if (Request.Form["isActief"] != null) { inventaris.IsActief = true; }
                else
                {
                    inventaris.IsActief = false;
                };
                if (Request.Form["isAanwezig"] != null) { inventaris.IsAanwezig = true; }
                else
                {
                    inventaris.IsAanwezig = false;
                };
                inventaris.Label = "TBA";
                client.InventarisCreate(inventaris);
            }
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                //WCF servicereference objecten collection naar InventarisModel objecten collection
                InventarisViewModel model = new InventarisViewModel();
                model.Inventaris = new List<InventarisModel>();
                model.Objecten = new List<SelectListItem>();
                model.Lokalen = new List<SelectListItem>();
                model.Verzekeringen = new List<SelectListItem>();

                Inventaris i = client.InventarisGetById(id);
                InventarisModel inventaris = new InventarisModel();
                inventaris.Id = i.Id;
                inventaris.IsAanwezig = i.IsAanwezig;
                inventaris.IsActief = i.IsActief;
                inventaris.Label = i.Label;
                inventaris.Lokaal = new LokaalModel() { IdLokaal = i.Lokaal.IdLokaal, AantalPlaatsen = i.Lokaal.AantalPlaatsen, IsComputerLokaal = i.Lokaal.IsComputerLokaal, LokaalNaam = i.Lokaal.LokaalNaam, Netwerk = new NetwerkModel() { Id = i.Lokaal.Netwerk.Id, Driver = i.Lokaal.Netwerk.Driver, Merk = i.Lokaal.Netwerk.Merk, Snelheid = i.Lokaal.Netwerk.Snelheid, Type = i.Lokaal.Netwerk.Type } };
                inventaris.Object = new ObjectModel() { Id = i.Object.Id, Kenmerken = i.Object.Kenmerken, Leverancier = new LeverancierModel() { IdLeverancier = i.Object.Leverancier.IdLeverancier, Afkorting = i.Object.Leverancier.Afkorting, Bic = i.Object.Leverancier.Bic, BtwNummer = i.Object.Leverancier.BtwNummer, BusNummer = i.Object.Leverancier.BusNummer, Email = i.Object.Leverancier.Email, Fax = i.Object.Leverancier.Fax, HuisNummer = i.Object.Leverancier.HuisNummer, Iban = i.Object.Leverancier.Iban, Naam = i.Object.Leverancier.Naam, Postcode = i.Object.Leverancier.Postcode, Straat = i.Object.Leverancier.Straat, Telefoon = i.Object.Leverancier.Telefoon, ToegevoegdOp = i.Object.Leverancier.ToegevoegdOp, Website = i.Object.Leverancier.Website }, ObjectType = new ObjectTypeModel() { IdObjectType = i.Object.ObjectType.Id, Omschrijving = i.Object.ObjectType.Omschrijving } };
                inventaris.Verzekering = new VerzekeringModel() { IdVerzekering = i.Verzekering.Id, Omschrijving = i.Verzekering.Omschrijving };
                inventaris.Aankoopjaar = i.Aankoopjaar;
                inventaris.Afschrijvingsperiode = i.Afschrijvingsperiode;
                inventaris.Historiek = i.Historiek;
                model.Inventaris.Add(inventaris);

                foreach (Lokaal l in client.LokaalGetAll())
                {
                    if (!(l.IdLokaal == inventaris.Lokaal.IdLokaal))
                    {
                        model.Lokalen.Add(new SelectListItem { Text = l.LokaalNaam, Value = l.IdLokaal.ToString() });
                    }
                }
                foreach (Verzekering v in client.VerzekeringGetAll())
                {
                    if (!(v.Id == inventaris.Verzekering.IdVerzekering))
                    {
                        model.Verzekeringen.Add(new SelectListItem { Text = v.Omschrijving, Value = v.Id.ToString() });
                    }
                }
                return View(model);
            }
        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                ViewBag.action = Request.Form["label"] + " was added";

                Inventaris inventaris = new Inventaris();
                inventaris.Id = Convert.ToInt16(Request.Form["idInventaris"]);
                inventaris.Aankoopjaar = Convert.ToInt32(Request.Form["aankoopjaar"]);
                inventaris.Afschrijvingsperiode = Convert.ToInt32(Request.Form["afschrijvingsperiode"]);
                inventaris.Historiek = Request.Form["historiek"];
                inventaris.Object = new ServiceReference.Object() { Id = Convert.ToInt16(Request.Form["idObject"]) };
                string test = Request.Form["Lokalen"];
                if (!String.IsNullOrWhiteSpace(Request.Form["Lokalen"])) { inventaris.Lokaal = new ServiceReference.Lokaal() { IdLokaal = Convert.ToInt16(Request.Form["Lokalen"]) }; }
                else { inventaris.Lokaal = new ServiceReference.Lokaal() { IdLokaal = Convert.ToInt16(Request.Form["defaultIdLokaal"]) }; }

                if (!String.IsNullOrWhiteSpace(Request.Form["Verzekeringen"])) { inventaris.Verzekering = new ServiceReference.Verzekering() { Id = Convert.ToInt16(Request.Form["Verzekeringen"]) }; }
                else { inventaris.Verzekering = new ServiceReference.Verzekering() { Id = Convert.ToInt16(Request.Form["defaultIdVerzekering"]) }; }

                if (Request.Form["isActief"] != null) { inventaris.IsActief = true; }
                else
                {
                    inventaris.IsActief = false;
                };
                if (Request.Form["isAanwezig"] != null) { inventaris.IsAanwezig = true; }
                else
                {
                    inventaris.IsAanwezig = false;
                };
                inventaris.Label = "TBA";
                client.InventarisUpdate(inventaris);
            }
            return RedirectToAction("Index");
        }

        // POST: Inventaris/Delete/5
        [HttpPost]
        public ActionResult Delete(int[] idArray, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                foreach (int id in idArray)
                {
                    client.InventarisDelete(id);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
