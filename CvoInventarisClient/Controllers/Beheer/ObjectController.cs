using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Controllers
{
    public class ObjectController : Controller
    {
        // GET: Inventaris
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                ObjectViewModel model = new ObjectViewModel();
                model.Objecten = new List<ObjectModel>();
                model.Facturen = new List<SelectListItem>();
                model.ObjectTypes = new List<SelectListItem>();

                foreach (ServiceReference.Object o in client.ObjectGetAll())
                {
                    ObjectModel obj = new ObjectModel();
                    obj.Id = o.Id;
                    obj.Kenmerken = o.Kenmerken;
                    obj.Factuur = new FactuurModel() { Afschrijfperiode = o.Factuur.Afschrijfperiode, Boekjaar = o.Factuur.Boekjaar, CvoVolgNummer = o.Factuur.CvoVolgNummer, DatumInsert = o.Factuur.DatumInsert, DatumModified = o.Factuur.DatumModified, FactuurDatum = o.Factuur.FactuurDatum, FactuurNummer = o.Factuur.FactuurNummer, Garantie = o.Factuur.Garantie, IdFactuur = o.Factuur.IdFactuur, Omschrijving = o.Factuur.Omschrijving, Opmerking = o.Factuur.Opmerking, Prijs = o.Factuur.Prijs, ScholengroepNummer = o.Factuur.ScholengroepNummer, UserInsert = o.Factuur.UserInsert, UserModified = o.Factuur.UserModified, Leverancier = new LeverancierModel() { Afkorting = o.Factuur.Leverancier.Afkorting, Bic = o.Factuur.Leverancier.Bic, BtwNummer = o.Factuur.Leverancier.BtwNummer, BusNummer = o.Factuur.Leverancier.BusNummer, Email = o.Factuur.Leverancier.Email, Fax = o.Factuur.Leverancier.Fax, HuisNummer = o.Factuur.Leverancier.HuisNummer, Iban = o.Factuur.Leverancier.Iban, IdLeverancier = o.Factuur.Leverancier.IdLeverancier, Naam = o.Factuur.Leverancier.Naam, Postcode = o.Factuur.Leverancier.Postcode, Straat = o.Factuur.Leverancier.Straat, Telefoon = o.Factuur.Leverancier.Telefoon, ToegevoegdOp = o.Factuur.Leverancier.ToegevoegdOp, Website = o.Factuur.Leverancier.Website } };
                    obj.ObjectType = new ObjectTypeModel() { IdObjectType = o.ObjectType.Id, Omschrijving = o.ObjectType.Omschrijving };
                    model.Objecten.Add(obj);
                }
                foreach (Factuur f in client.FactuurGetAll())
                {
                    model.Facturen.Add(new SelectListItem { Text = f.FactuurNummer, Value = f.IdFactuur.ToString() });
                }
                foreach (ObjectTypes ot in client.ObjectTypeGetAll())
                {
                    model.ObjectTypes.Add(new SelectListItem { Text = ot.Omschrijving, Value = ot.Id.ToString() });
                }
                return View(model);
            }
        }

        // POST: Inventaris/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                ServiceReference.Object obj = new ServiceReference.Object();
                obj.Kenmerken = Request.Form["kenmerken"];
                //Tijdelijk moet weg nadat leverancier uit inventaris wordt gehaald
                obj.Leverancier = new Leverancier() { IdLeverancier = 5 };
                obj.Factuur = new Factuur() { IdFactuur = Convert.ToInt32(Request.Form["Facturen"]) };
                obj.ObjectType = new ObjectTypes() { Id = Convert.ToInt32(Request.Form["ObjectTypes"]) };
                client.ObjectCreate(obj);
            }
            TempData["action"] = "Object werd toegevoegd";
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                ObjectViewModel model = new ObjectViewModel();
                model.Objecten = new List<ObjectModel>();
                model.Facturen = new List<SelectListItem>();
                model.ObjectTypes = new List<SelectListItem>();

                ServiceReference.Object o = client.ObjectGetById(id);

                ObjectModel obj = new ObjectModel();
                obj.Id = o.Id;
                obj.Kenmerken = o.Kenmerken;
                obj.Factuur = new FactuurModel() { Afschrijfperiode = o.Factuur.Afschrijfperiode, Boekjaar = o.Factuur.Boekjaar, CvoVolgNummer = o.Factuur.CvoVolgNummer, DatumInsert = o.Factuur.DatumInsert, DatumModified = o.Factuur.DatumModified, FactuurDatum = o.Factuur.FactuurDatum, FactuurNummer = o.Factuur.FactuurNummer, Garantie = o.Factuur.Garantie, IdFactuur = o.Factuur.IdFactuur, Omschrijving = o.Factuur.Omschrijving, Opmerking = o.Factuur.Opmerking, Prijs = o.Factuur.Prijs, ScholengroepNummer = o.Factuur.ScholengroepNummer, UserInsert = o.Factuur.UserInsert, UserModified = o.Factuur.UserModified, Leverancier = new LeverancierModel() { Afkorting = o.Factuur.Leverancier.Afkorting, Bic = o.Factuur.Leverancier.Bic, BtwNummer = o.Factuur.Leverancier.BtwNummer, BusNummer = o.Factuur.Leverancier.BusNummer, Email = o.Factuur.Leverancier.Email, Fax = o.Factuur.Leverancier.Fax, HuisNummer = o.Factuur.Leverancier.HuisNummer, Iban = o.Factuur.Leverancier.Iban, IdLeverancier = o.Factuur.Leverancier.IdLeverancier, Naam = o.Factuur.Leverancier.Naam, Postcode = o.Factuur.Leverancier.Postcode, Straat = o.Factuur.Leverancier.Straat, Telefoon = o.Factuur.Leverancier.Telefoon, ToegevoegdOp = o.Factuur.Leverancier.ToegevoegdOp, Website = o.Factuur.Leverancier.Website } };
                obj.ObjectType = new ObjectTypeModel() { IdObjectType = o.ObjectType.Id, Omschrijving = o.ObjectType.Omschrijving };
                model.Objecten.Add(obj);

                foreach (Factuur f in client.FactuurGetAll())
                {
                    model.Facturen.Add(new SelectListItem { Text = f.FactuurNummer, Value = f.IdFactuur.ToString() });
                }
                foreach (ObjectTypes ot in client.ObjectTypeGetAll())
                {
                    model.ObjectTypes.Add(new SelectListItem { Text = ot.Omschrijving, Value = ot.Id.ToString() });
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
                ViewBag.action = Request.Form["kenmerken"] + " was changed";

                CvoInventarisClient.ServiceReference.Object obj = new CvoInventarisClient.ServiceReference.Object();
                obj.Id = Convert.ToInt32(Request.Form["idObject"]);
                obj.Kenmerken = Request.Form["kenmerken"];
                if (!String.IsNullOrWhiteSpace(Request.Form["facturen"])) { obj.Factuur = new ServiceReference.Factuur() { IdFactuur = Convert.ToInt16(Request.Form["Facturen"]) }; }
                else { obj.Factuur = new ServiceReference.Factuur() { IdFactuur = Convert.ToInt16(Request.Form["defaultIdFactuur"]) }; }
                //Tijdelijk moet weg nadat leverancier uit inventaris wordt gehaald
                obj.Leverancier = new Leverancier() { IdLeverancier = 5};
                if (!String.IsNullOrWhiteSpace(Request.Form["ObjectTypes"])) { obj.ObjectType = new ServiceReference.ObjectTypes() { Id = Convert.ToInt16(Request.Form["ObjectTypes"]) }; }
                else { obj.ObjectType = new ServiceReference.ObjectTypes() { Id = Convert.ToInt16(Request.Form["defaultIdObjectType"]) }; }

                client.ObjectUpdate(obj);
            }
            TempData["action"] = "Object werd gewijzigd";
            return RedirectToAction("Index");
        }

        // POST: Inventaris/Delete/5
        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                foreach (int id in idArray)
                {
                    client.ObjectDelete(id);
                }
            }
            if (idArray.Length >= 2)
            {
                TempData["action"] = idArray.Length + " objecten werden verwijderd uit objecten";
            }
            else
            {
                TempData["action"] = idArray.Length + " object werd verwijderd uit objecten";
            }
            return RedirectToAction("Index");
        }
    }
}