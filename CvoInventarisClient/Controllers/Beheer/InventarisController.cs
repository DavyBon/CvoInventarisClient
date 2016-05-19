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
                if (!model.Inventaris.Exists(i => i.Object.Id == o.Id)) { model.Objecten.Add(new SelectListItem { Text = o.Kenmerken, Value = o.Id.ToString() }); }
            }
            foreach (LokaalModel l in TblLokaal.GetAll())
            {
                model.Lokalen.Add(new SelectListItem { Text = l.LokaalNaam, Value = l.IdLokaal.ToString() });
            }
            foreach (VerzekeringModel v in TblVerzekering.GetAll())
            {
                model.Verzekeringen.Add(new SelectListItem { Text = v.Omschrijving, Value = v.IdVerzekering.ToString() });
            }
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
                inventaris.Lokaal = new LokaalModel() { IdLokaal = Convert.ToInt16(Request.Form["Lokalen"]) };
                inventaris.Verzekering = new VerzekeringModel() { IdVerzekering = Convert.ToInt16(Request.Form["Verzekeringen"]) };
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
                if (!(l.IdLokaal == i.Lokaal.IdLokaal))
                {
                    model.Lokalen.Add(new SelectListItem { Text = l.LokaalNaam, Value = l.IdLokaal.ToString() });
                }
            }
            foreach (VerzekeringModel v in TblVerzekering.GetAll())
            {
                if (!(v.IdVerzekering == i.Verzekering.IdVerzekering))
                {
                    model.Verzekeringen.Add(new SelectListItem { Text = v.Omschrijving, Value = v.IdVerzekering.ToString() });
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
            if (!String.IsNullOrWhiteSpace(Request.Form["Lokalen"])) { inventaris.Lokaal = new LokaalModel() { IdLokaal = Convert.ToInt16(Request.Form["Lokalen"]) }; }
            else { inventaris.Lokaal = new LokaalModel() { IdLokaal = Convert.ToInt16(Request.Form["defaultIdLokaal"]) }; }

            if (!String.IsNullOrWhiteSpace(Request.Form["Verzekeringen"])) { inventaris.Verzekering = new VerzekeringModel() { IdVerzekering = Convert.ToInt16(Request.Form["Verzekeringen"]) }; }
            else { inventaris.Verzekering = new VerzekeringModel() { IdVerzekering = Convert.ToInt16(Request.Form["defaultIdVerzekering"]) }; }

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
            TblInventaris.Update(inventaris);

            TempData["action"] = "Object in inventaris werd gewijzigd";
            return RedirectToAction("Index");
        }



        // POST: Inventaris/Delete/5
        [HttpPost]
        public ActionResult Delete(int[] idArray, FormCollection collection)
        {
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
    }
}

