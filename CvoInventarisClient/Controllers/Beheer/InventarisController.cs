using CvoInventarisClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Controllers
{
    [Authorize]
    public class InventarisController : Controller
    {
        // GET: Inventaris
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();
            DAL.TblObject TblObject = new DAL.TblObject();
            DAL.TblLokaal TblLokaal = new DAL.TblLokaal();
            DAL.TblVerzekering TblVerzekering = new DAL.TblVerzekering();

            //WCF servicereference objecten collection naar InventarisModel objecten collection
            InventarisViewModel model = new InventarisViewModel();
            model.Inventaris = new List<InventarisModel>();
            model.Objecten = new List<SelectListItem>();
            model.Lokalen = new List<SelectListItem>();
            model.Verzekeringen = new List<SelectListItem>();

            foreach (InventarisModel i in TblInventaris.GetAll())
            {
                model.Inventaris.Add(i);
            }
            foreach (ObjectModel o in TblObject.GetAll())
            {
                model.Objecten.Add(new SelectListItem { Text = o.Kenmerken, Value = o.Id.ToString() });
            }
            foreach (LokaalModel l in TblLokaal.GetAll())
            {
                model.Lokalen.Add(new SelectListItem { Text = l.LokaalNaam, Value = l.Id.ToString() });
            }
            foreach (VerzekeringModel v in TblVerzekering.GetAll())
            {
                model.Verzekeringen.Add(new SelectListItem { Text = v.Omschrijving, Value = v.Id.ToString() });
            }

            this.Session["inventarisview"] = model;

            return View(model);

        }

        // POST: Inventaris/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();

            for (int i = 0; i < Convert.ToInt32(Request.Form["aantal"]); i++)
            {
                int labelnr = Convert.ToInt32(Request.Form["volgnummer"]) + i;
                InventarisModel inventaris = new InventarisModel();
                inventaris.Aankoopjaar = Convert.ToInt32(Request.Form["aankoopjaar"]);
                inventaris.Afschrijvingsperiode = Convert.ToInt32(Request.Form["afschrijvingsperiode"]);
                inventaris.Historiek = Request.Form["historiek"];
                inventaris.Label = Request.Form["reeks"] + labelnr;
                inventaris.Object = new ObjectModel() { Id = Convert.ToInt16(Request.Form["Objecten"]) };
                inventaris.Lokaal = new LokaalModel() { Id = Convert.ToInt16(Request.Form["Lokalen"]) };
                inventaris.Verzekering = new VerzekeringModel() { Id = Convert.ToInt16(Request.Form["Verzekeringen"]) };
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
                TblInventaris.Create(inventaris);
            }

            TempData["action"] = "Object werd toegevoegd aan inventaris";
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();
            DAL.TblVerzekering TblVerzekering = new DAL.TblVerzekering();
            DAL.TblLokaal TblLokaal = new DAL.TblLokaal();
            InventarisViewModel model = new InventarisViewModel();

            model.Inventaris = new List<InventarisModel>();
            model.Objecten = new List<SelectListItem>();
            model.Lokalen = new List<SelectListItem>();
            model.Verzekeringen = new List<SelectListItem>();

            InventarisModel i = TblInventaris.GetById(id);
            model.Inventaris.Add(i);

            foreach (LokaalModel l in TblLokaal.GetAll())
            {
                if (!(l.Id == i.Lokaal.Id))
                {
                    model.Lokalen.Add(new SelectListItem { Text = l.LokaalNaam, Value = l.Id.ToString() });
                }
            }
            foreach (VerzekeringModel v in TblVerzekering.GetAll())
            {
                if (!(v.Id == i.Verzekering.Id))
                {
                    model.Verzekeringen.Add(new SelectListItem { Text = v.Omschrijving, Value = v.Id.ToString() });
                }
            }
            return View(model);

        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();

            InventarisModel inventaris = new InventarisModel();
            inventaris.Id = Convert.ToInt16(Request.Form["idInventaris"]);
            inventaris.Aankoopjaar = Convert.ToInt32(Request.Form["aankoopjaar"]);
            inventaris.Afschrijvingsperiode = Convert.ToInt32(Request.Form["afschrijvingsperiode"]);
            inventaris.Historiek = Request.Form["historiek"];
            inventaris.Object = new ObjectModel() { Id = Convert.ToInt16(Request.Form["idObject"]) };
            if (!String.IsNullOrWhiteSpace(Request.Form["Lokalen"])) { inventaris.Lokaal = new LokaalModel() { Id = Convert.ToInt16(Request.Form["Lokalen"]) }; }
            else { inventaris.Lokaal = new LokaalModel() { Id = Convert.ToInt16(Request.Form["defaultIdLokaal"]) }; }

            if (!String.IsNullOrWhiteSpace(Request.Form["Verzekeringen"])) { inventaris.Verzekering = new VerzekeringModel() { Id = Convert.ToInt16(Request.Form["Verzekeringen"]) }; }
            else { inventaris.Verzekering = new VerzekeringModel() { Id = Convert.ToInt16(Request.Form["defaultIdVerzekering"]) }; }

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
            TblInventaris.Update(inventaris);

            TempData["action"] = "Object in inventaris werd gewijzigd";
            return RedirectToAction("Index");
        }



        // POST: Inventaris/Delete/5
        [HttpPost]
        public ActionResult Delete(int[] idArray, FormCollection collection)
        {
            if (idArray == null) { return RedirectToAction("Index"); }
            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();

            foreach (int id in idArray)
            {
                TblInventaris.Delete(id);
            }

            if (idArray.Length >= 2)
            {
                TempData["action"] = idArray.Length + " objecten werden verwijderd uit de inventaris";
            }
            else
            {
                TempData["action"] = idArray.Length + " netwerk werd verwijderd uit de inventaris";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Filter(int objectFilter, string aanwezigFilter, string actiefFilter, int lokaalFilter, string historiekFilter, string filterAankoopjaar, string filterAankoopjaarSecondary, string filterAfschrijvingsperiode, string filterAfschrijvingsperiodeSecondary, int verzekeringFilter, int[] modelList)
        {
            ViewBag.action = TempData["action"];

            InventarisViewModel model = (InventarisViewModel)(Session["inventarisview"] as InventarisViewModel).Clone();

            bool isAanwezig;
            bool isActief;

            if (aanwezigFilter != null)
            {
                isAanwezig = true;
            }
            else
            {
                isAanwezig = false;
            }


            if (actiefFilter != null)
            {
                isActief = true;
            }
            else
            {
                isActief = false;
            }

            // Hier start filteren
            if (objectFilter >= 0)
            {
                model.Inventaris.RemoveAll(x => x.Object.Id != objectFilter);
            }

            model.Inventaris.RemoveAll(x => x.IsAanwezig != isAanwezig);
            model.Inventaris.RemoveAll(x => x.IsAanwezig != isActief);

            if (lokaalFilter >= 0)
            {
                model.Inventaris.RemoveAll(x => x.Lokaal.Id != lokaalFilter);
            }

            if (!String.IsNullOrWhiteSpace(historiekFilter))
            {
                model.Inventaris.RemoveAll(x => !x.Historiek.ToLower().Contains(historiekFilter.ToLower()));
            }

            if (!String.IsNullOrWhiteSpace(filterAankoopjaar))
            {
                if (filterAankoopjaarSecondary.Equals("="))
                {
                    model.Inventaris.RemoveAll(x => x.Aankoopjaar != Convert.ToInt32(filterAankoopjaar));
                }
                else if (filterAankoopjaarSecondary.Equals("<"))
                {
                    model.Inventaris.RemoveAll(x => x.Aankoopjaar > Convert.ToInt32(filterAankoopjaar));
                }
                else if (filterAankoopjaarSecondary.Equals(">"))
                {
                    model.Inventaris.RemoveAll(x => x.Aankoopjaar < Convert.ToInt32(filterAankoopjaar));
                }
            }

            if (!String.IsNullOrWhiteSpace(filterAfschrijvingsperiode))
            {
                if (filterAfschrijvingsperiodeSecondary.Equals("="))
                {
                    model.Inventaris.RemoveAll(x => x.Afschrijvingsperiode != Convert.ToInt32(filterAfschrijvingsperiode));
                }
                else if (filterAfschrijvingsperiodeSecondary.Equals("<"))
                {
                    model.Inventaris.RemoveAll(x => x.Afschrijvingsperiode > Convert.ToInt32(filterAfschrijvingsperiode));
                }
                else if (filterAfschrijvingsperiodeSecondary.Equals(">"))
                {
                    model.Inventaris.RemoveAll(x => x.Afschrijvingsperiode < Convert.ToInt32(filterAfschrijvingsperiode));
                }
            }

            if (verzekeringFilter >= 0)
            {
                model.Inventaris.RemoveAll(x => x.Verzekering.Id != verzekeringFilter);
            }
            return View("index", model);
        }
    }
}
