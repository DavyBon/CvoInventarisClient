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
                model.Objecten = new List<ObjectModel>();
                model.Lokalen = new List<SelectListItem>();
                model.Verzekeringen = new List<SelectListItem>();

                foreach (Inventaris i in client.InventarisGetAll())
                {
                    InventarisModel inventaris = new InventarisModel();
                    inventaris.Id = i.Id;
                    inventaris.IsAanwezig = i.IsAanwezig;
                    inventaris.IsActief = i.IsActief;
                    inventaris.Label = i.Label;
                    inventaris.Lokaal = new LokaalModel() { IdLokaal = i.Lokaal.IdLokaal, AantalPlaatsen = i.Lokaal.AantalPlaatsen, IsComputerLokaal = i.Lokaal.IsComputerLokaal, LokaalNaam = i.Lokaal.LokaalNaam,  Netwerk = new NetwerkModel() { Id = i.Lokaal.Netwerk.Id, Driver = i.Lokaal.Netwerk.Driver, Merk = i.Lokaal.Netwerk.Merk, Snelheid = i.Lokaal.Netwerk.Snelheid, Type = i.Lokaal.Netwerk.Type } };
                    inventaris.Object = new ObjectModel() { Id = i.Object.Id, Kenmerken = i.Object.Kenmerken, Leverancier = new LeverancierModel() { IdLeverancier = i.Object.Leverancier.IdLeverancier, Afkorting = i.Object.Leverancier.Afkorting, Bic = i.Object.Leverancier.Bic, BtwNummer = i.Object.Leverancier.BtwNummer, BusNummer = i.Object.Leverancier.BusNummer, Email = i.Object.Leverancier.Email, Fax = i.Object.Leverancier.Fax, HuisNummer = i.Object.Leverancier.HuisNummer, Iban = i.Object.Leverancier.Iban, Naam = i.Object.Leverancier.Naam, Postcode = i.Object.Leverancier.Postcode, Straat = i.Object.Leverancier.Straat, Telefoon = i.Object.Leverancier.Telefoon, ToegevoegdOp = i.Object.Leverancier.ToegevoegdOp, Website = i.Object.Leverancier.Website }, ObjectType = new ObjectTypeModel() { IdObjectType = i.Object.ObjectType.Id, Omschrijving = i.Object.ObjectType.Omschrijving}};
                    inventaris.Verzekering = new VerzekeringModel() { IdVerzekering = i.Verzekering.Id, Omschrijving = i.Verzekering.Omschrijving};
                    inventaris.Aankoopjaar = i.Aankoopjaar;
                    inventaris.Afschrijvingsperiode = i.Afschrijvingsperiode;
                    inventaris.Historiek = i.Historiek;
                    model.Inventaris.Add(inventaris);
                }
                foreach (ServiceReference.Object o in client.ObjectGetAll())
                {
                    ObjectModel obj = new ObjectModel();
                    obj.Id = o.Id;
                    obj.Kenmerken = o.Kenmerken;
                    obj.Factuur = new FactuurModel() { Afschrijfperiode = o.Factuur.Afschrijfperiode, Boekjaar = o.Factuur.Boekjaar, CvoVolgNummer = o.Factuur.CvoVolgNummer, DatumInsert = o.Factuur.DatumInsert, DatumModified = o.Factuur.DatumModified, FactuurDatum = o.Factuur.FactuurDatum, FactuurNummer = o.Factuur.FactuurNummer, FactuurStatusGetekend = o.Factuur.FactuurStatusGetekend, Garantie = o.Factuur.Garantie, IdFactuur = o.Factuur.IdFactuur, OleDoc = o.Factuur.OleDoc, OleDocFileName = o.Factuur.OleDocFileName, OleDocPath = o.Factuur.OleDocPath, Omschrijving = o.Factuur.Omschrijving, Opmerking = o.Factuur.Opmerking, Prijs = o.Factuur.Prijs, UserInsert = o.Factuur.UserInsert, UserModified = o.Factuur.UserModified, VerwerkingsDatum = o.Factuur.VerwerkingsDatum, Leverancier = new LeverancierModel() { Afkorting = o.Factuur.Leverancier.Afkorting, Bic = o.Factuur.Leverancier.Bic, BtwNummer = o.Factuur.Leverancier.BtwNummer, BusNummer = o.Factuur.Leverancier.BusNummer, Email = o.Factuur.Leverancier.Email, Fax = o.Factuur.Leverancier.Fax, HuisNummer = o.Factuur.Leverancier.HuisNummer, Iban = o.Factuur.Leverancier.Iban, IdLeverancier = o.Factuur.Leverancier.IdLeverancier, Naam = o.Factuur.Leverancier.Naam, Postcode = o.Factuur.Leverancier.Postcode, Straat = o.Factuur.Leverancier.Straat, Telefoon = o.Factuur.Leverancier.Telefoon, ToegevoegdOp = o.Factuur.Leverancier.ToegevoegdOp, Website = o.Factuur.Leverancier.Website} };
                    obj.ObjectType = new ObjectTypeModel() { IdObjectType = o.ObjectType.Id, Omschrijving = o.ObjectType.Omschrijving};
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
                inventaris.Lokaal.IdLokaal = Convert.ToInt32(Request.Form["idLokaal"]);
                inventaris.Object.Id = Convert.ToInt32(Request.Form["idObject"]);
                inventaris.Verzekering.Id = Convert.ToInt32(Request.Form["idVerzekering"]);
                //inventaris.isAanwezig = Boolean.Parse(Request.Form["isAanwezig"]);
                //inventaris.isActief = Convert.ToBoolean(Request.Form["isActief"]);
                inventaris.Label = Request.Form["label"];
                client.InventarisCreate(inventaris);
            }
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.InventarisGetById(id));
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
                inventaris.Id = id;
                inventaris.Aankoopjaar = Convert.ToInt32(Request.Form["aankoopjaar"]);
                inventaris.Afschrijvingsperiode = Convert.ToInt32(Request.Form["afschrijvingsperiode"]);
                inventaris.Historiek = Request.Form["historiek"];
                inventaris.Lokaal.IdLokaal = Convert.ToInt32(Request.Form["idLokaal"]);
                inventaris.Object.Id = Convert.ToInt32(Request.Form["idObject"]);
                inventaris.Verzekering.Id = Convert.ToInt32(Request.Form["idVerzekering"]);
                //inventaris.isAanwezig = Boolean.Parse(Request.Form["isAanwezig"]);
                //inventaris.isActief = Convert.ToBoolean(Request.Form["isActief"]);
                inventaris.Label = Request.Form["label"];
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
