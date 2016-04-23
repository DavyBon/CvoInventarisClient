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
                List<Inventaris> inv = client.InventarisGetAll().ToList();
                List<InventarisModel> model = new List<InventarisModel>();
                foreach (Inventaris i in inv)
                {
                    InventarisModel inventarisModel = new InventarisModel();

                    //Objecten van webservice
                    ServiceReference.Object obj = client.ObjectGetById(i.IdObject);
                    Leverancier lev = client.LeverancierGetById(obj.IdLeverancier);
                    Factuur fac = client.FactuurGetById(obj.IdFactuur);
                    ObjectTypes objType = client.ObjectTypeGetById(obj.IdObjectType);
                    Verzekering ver = client.VerzekeringGetById(i.IdVerzekering);
                    Lokaal lok = client.LokaalGetById(i.IdLokaal);
                    Netwerk net = client.NetwerkGetById(lok.IdNetwerk);

                    //Objecten van wcf naar models
                    LeverancierModel leverancierModel = new LeverancierModel();
                    leverancierModel.IdLeverancier = lev.IdLeverancier;
                    leverancierModel.Afkorting = lev.Afkorting;
                    leverancierModel.Bic = lev.Bic;
                    leverancierModel.BtwNummer = lev.BtwNummer;
                    leverancierModel.BusNummer = lev.BusNummer;
                    leverancierModel.Email = lev.Email;
                    leverancierModel.Fax = lev.Fax;
                    leverancierModel.HuisNummer = lev.HuisNummer;
                    leverancierModel.Iban = lev.Iban;
                    leverancierModel.Naam = lev.Naam;
                    leverancierModel.Postcode = lev.Postcode;
                    leverancierModel.Straat = lev.Straat;
                    leverancierModel.Telefoon = lev.Telefoon;
                    leverancierModel.ToegevoegdOp = lev.ToegevoegdOp;
                    leverancierModel.Website = lev.Website;

                    FactuurModel factuurModel = new FactuurModel();
                    factuurModel.IdFactuur = fac.IdFactuur;
                    factuurModel.Boekjaar = fac.Boekjaar;
                    factuurModel.CvoVolgNummer = fac.CvoVolgNummer;
                    factuurModel.FactuurNummer = fac.FactuurNummer;
                    factuurModel.FactuurDatum = fac.FactuurDatum;
                    factuurModel.FactuurStatusGetekend = fac.FactuurStatusGetekend;
                    factuurModel.VerwerkingsDatum = fac.VerwerkingsDatum;
                    factuurModel.Leverancier = leverancierModel;
                    factuurModel.Prijs = fac.Prijs;
                    factuurModel.Garantie = fac.Garantie;
                    factuurModel.Omschrijving = fac.Omschrijving;
                    factuurModel.Opmerking = fac.Opmerking;
                    factuurModel.Afschrijfperiode = fac.Afschrijfperiode;
                    factuurModel.OleDoc = fac.OleDoc;
                    factuurModel.OleDocPath = fac.OleDocPath;
                    factuurModel.OleDocFileName = fac.OleDocFileName;
                    factuurModel.DatumInsert = fac.DatumInsert;
                    factuurModel.UserInsert = fac.UserInsert;
                    factuurModel.DatumModified = fac.DatumModified;
                    factuurModel.UserModified = fac.UserModified;

                    NetwerkModel netwerkModel = new NetwerkModel();
                    netwerkModel.Id = net.Id;
                    netwerkModel.Driver = net.Driver;
                    netwerkModel.Merk = net.Driver;
                    netwerkModel.Snelheid = net.Snelheid;
                    netwerkModel.Type = net.Type;

                    ObjectTypeModel objectTypeModel = new ObjectTypeModel();
                    objectTypeModel.IdObjectType = objType.Id;
                    objectTypeModel.Omschrijving = objType.Omschrijving;

                    ObjectModel objectModel = new ObjectModel();
                    objectModel.Id = obj.Id;
                    objectModel.Factuur = factuurModel;
                    objectModel.Leverancier = leverancierModel;
                    objectModel.ObjectType = objectTypeModel;
                    objectModel.Kenmerken = obj.Kenmerken;

                    VerzekeringModel verzekeringModel = new VerzekeringModel();
                    verzekeringModel.IdVerzekering = ver.Id;
                    verzekeringModel.Omschrijving = ver.Omschrijving;

                    LokaalModel lokaalModel = new LokaalModel();
                    lokaalModel.IdLokaal = lok.IdLokaal;
                    lokaalModel.AantalPlaatsen = lok.AantalPlaatsen;
                    lokaalModel.IsComputerLokaal = lok.IsComputerLokaal;
                    lokaalModel.LokaalNaam = lok.LokaalNaam;
                    lokaalModel.Netwerk = netwerkModel;


                    inventarisModel.Id = i.Id;
                    inventarisModel.IsAanwezig = i.IsAanwezig;
                    inventarisModel.IsActief = i.IsActief;
                    inventarisModel.Label = i.Label;
                    inventarisModel.Lokaal = lokaalModel;
                    inventarisModel.Object = objectModel;
                    inventarisModel.Verzekering = verzekeringModel;

                    model.Add(inventarisModel);
                }
                return View(model);
            }
        }

        // GET: Inventaris/Details/5
        public ActionResult Details(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.InventarisGetById(id));
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
                inventaris.IdLokaal = Convert.ToInt32(Request.Form["idLokaal"]);
                inventaris.IdObject = Convert.ToInt32(Request.Form["idObject"]);
                inventaris.IdVerzekering = Convert.ToInt32(Request.Form["idVerzekering"]);
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
                inventaris.IdLokaal = Convert.ToInt32(Request.Form["idLokaal"]);
                inventaris.IdObject = Convert.ToInt32(Request.Form["idObject"]);
                inventaris.IdVerzekering = Convert.ToInt32(Request.Form["idVerzekering"]);
                //inventaris.isAanwezig = Boolean.Parse(Request.Form["isAanwezig"]);
                //inventaris.isActief = Convert.ToBoolean(Request.Form["isActief"]);
                inventaris.Label = Request.Form["label"];
                client.InventarisUpdate(inventaris);
            }
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Inventaris/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                client.InventarisDelete(id);
            }
            return RedirectToAction("Index");
        }
    }
}
