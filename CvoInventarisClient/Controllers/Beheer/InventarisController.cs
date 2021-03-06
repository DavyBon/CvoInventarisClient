﻿using CvoInventarisClient.Models;
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
        public ActionResult Index(int? amount, string order)
        {
            ViewBag.action = TempData["action"];
            ViewBag.warning = TempData["warning"];

            InventarisViewModel model = new InventarisViewModel();

            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();
            DAL.TblObject TblObject = new DAL.TblObject();
            DAL.TblLokaal TblLokaal = new DAL.TblLokaal();
            DAL.TblVerzekering TblVerzekering = new DAL.TblVerzekering();
            DAL.TblFactuur TblFactuur = new DAL.TblFactuur();
            DAL.TblObjectType TblObjecttype = new DAL.TblObjectType();
            DAL.TblCampus TblCampus = new DAL.TblCampus();
            DAL.TblLeverancier TblLeverancier = new DAL.TblLeverancier();

            model.Inventaris = new List<InventarisModel>();
            model.Objecten = new List<SelectListItem>();
            model.Lokalen = new List<SelectListItem>();
            model.Verzekeringen = new List<SelectListItem>();
            model.Objecttypen = new List<SelectListItem>();
            model.Campussen = new List<SelectListItem>();
            model.Leverancieren = new List<SelectListItem>();
            model.Facturen = new List<SelectListItem>();

            model.Inventaris = TblInventaris.GetTop(amount);

            foreach (ObjectModel o in TblObject.GetAll().OrderBy(x => x.Omschrijving))
            {
                model.Objecten.Add(new SelectListItem { Text = o.Omschrijving, Value = o.Id.ToString() });
            }
            foreach (LokaalModel l in TblLokaal.GetAll().OrderBy(x => x.LokaalNaam))
            {
                model.Lokalen.Add(new SelectListItem { Text = l.LokaalNaam, Value = l.Id.ToString() });
            }
            foreach (VerzekeringModel v in TblVerzekering.GetAll().OrderBy(x => x.Omschrijving))
            {
                model.Verzekeringen.Add(new SelectListItem { Text = v.Omschrijving, Value = v.Id.ToString() });
            }
            foreach (ObjectTypeModel ot in TblObjecttype.GetAll().OrderBy(x => x.Omschrijving))
            {
                model.Objecttypen.Add(new SelectListItem { Text = ot.Omschrijving, Value = ot.Id.ToString() });
            }
            foreach (FactuurModel f in TblFactuur.GetAll().OrderBy(x => x.CvoFactuurNummer))
            {
                model.Facturen.Add(new SelectListItem { Text = f.CvoFactuurNummer, Value = f.Id.ToString() });
            }
            foreach (CampusModel c in TblCampus.GetAll().OrderBy(x => x.Naam))
            {
                model.Campussen.Add(new SelectListItem { Text = c.Naam, Value = c.Id.ToString() });
            }

            foreach (LeverancierModel l in TblLeverancier.GetAll().OrderBy(x => x.Naam))
            {
                model.Leverancieren.Add(new SelectListItem { Text = l.Naam, Value = l.Id.ToString() });
            }

            ViewBag.amount = model.Inventaris.Count.ToString();

            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("Oudst"))
                {
                    model.Inventaris.Reverse();
                }
                else if (order.Equals("Lokaal"))
                {
                    model.Inventaris = model.Inventaris.OrderBy(i => i.Lokaal.Id).ToList();
                }
                ViewBag.ordertype = order.ToString();
            }
            else
            {
                ViewBag.ordertype = "Meest recent";
            }

            ViewBag.Heading = this.ControllerContext.RouteData.Values["controller"].ToString() + " (" + model.Inventaris.Count() + ")";

            return View(model);

        }
        public ActionResult Create()
        {
            InventarisViewModel model = new InventarisViewModel();

            DAL.TblObject TblObject = new DAL.TblObject();
            DAL.TblLokaal TblLokaal = new DAL.TblLokaal();
            DAL.TblVerzekering TblVerzekering = new DAL.TblVerzekering();
            DAL.TblFactuur TblFactuur = new DAL.TblFactuur();
            DAL.TblObjectType TblObjecttype = new DAL.TblObjectType();
            DAL.TblLeverancier TblLeverancier = new DAL.TblLeverancier();

            model.Objecten = new List<SelectListItem>();
            model.Lokalen = new List<SelectListItem>();
            model.Verzekeringen = new List<SelectListItem>();
            model.Objecttypen = new List<SelectListItem>();
            model.Facturen = new List<SelectListItem>();
            model.Leverancieren = new List<SelectListItem>();


            foreach (ObjectModel o in TblObject.GetAll().OrderBy(x => x.Omschrijving))
            {
                model.Objecten.Add(new SelectListItem { Text = o.Omschrijving, Value = o.Id.ToString() });
            }
            foreach (LokaalModel l in TblLokaal.GetAll().OrderBy(x => x.Campus.Naam))
            {
                model.Lokalen.Add(new SelectListItem { Text = l.LokaalNaam + ", " + l.Campus.Naam, Value = l.Id.ToString() });
            }
            foreach (VerzekeringModel v in TblVerzekering.GetAll().OrderBy(x => x.Omschrijving))
            {
                model.Verzekeringen.Add(new SelectListItem { Text = v.Omschrijving, Value = v.Id.ToString() });
            }
            foreach (FactuurModel f in TblFactuur.GetAll().OrderBy(x => x.CvoFactuurNummer))
            {
                model.Facturen.Add(new SelectListItem { Text = f.CvoFactuurNummer, Value = f.Id.ToString() });
            }
            foreach (LeverancierModel l in TblLeverancier.GetAll())
            {
                model.Leverancieren.Add(new SelectListItem { Text = l.Naam, Value = l.Id.ToString() });
            }

            return View(model);
        }

        // POST: Inventaris/Create
        [HttpPost]
        public ActionResult Create(int? Objecten, int? Lokalen, int? Verzekeringen, int? Facturen, int? Leverancieren, string waarde, int? aankoopjaar, int? afschrijvingsperiode, string costcenter,string boekhoudnr, int? aantal)
        {
            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();

            List<String> duplicates = new List<string>();
            List<String> toegevoegd = new List<string>();

            for (int i = 0; i < aantal; i++)
            {
                int labelnr = Convert.ToInt32(Request.Form["volgnummer"]) + i;
                InventarisModel inventaris = new InventarisModel();
                inventaris.Aankoopjaar = aankoopjaar;
                inventaris.Afschrijvingsperiode = afschrijvingsperiode;
                inventaris.Historiek = Request.Form["historiek"];
                inventaris.Label = Request.Form["reeks"] + labelnr.ToString().PadLeft(4, '0');
                inventaris.Costcenter = costcenter;
                inventaris.Boekhoudnr = boekhoudnr;
                if (!string.IsNullOrWhiteSpace(waarde))
                {
                    inventaris.Waarde = Convert.ToDecimal(waarde.Replace(".", ","));
                }
                inventaris.Object = new ObjectModel() { Id = Objecten };
                inventaris.Lokaal = new LokaalModel() { Id = Lokalen };
                inventaris.Factuur = new FactuurModel() { Id = Facturen };
                inventaris.Verzekering = new VerzekeringModel() { Id = Verzekeringen };
                inventaris.Factuur = new FactuurModel() { Id = Facturen };
                inventaris.Leverancier = new LeverancierModel() { Id = Leverancieren };

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

                int dalresponse = TblInventaris.Create(inventaris);

                if (dalresponse == -1)
                {
                    duplicates.Add(inventaris.Label);
                }
                else if (dalresponse > 0)
                {
                    toegevoegd.Add(inventaris.Label);
                }
            }

            if (duplicates.Any())
            {
                TempData["warning"] = "label(s) \"" + String.Join(",", duplicates) + "\" werd(en) niet toegevoegd, bestaat al";
            }
            if (toegevoegd.Any())
            {
                TempData["action"] = "\"" + String.Join(",", toegevoegd) + " \" werd(en) toegevoegd aan de inventaris";
            }
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();
            DAL.TblVerzekering TblVerzekering = new DAL.TblVerzekering();
            DAL.TblFactuur TblFactuur = new DAL.TblFactuur();
            DAL.TblLokaal TblLokaal = new DAL.TblLokaal();
            DAL.TblObject TblObject = new DAL.TblObject();
            DAL.TblLeverancier TblLeverancier = new DAL.TblLeverancier();
            InventarisViewModel model = new InventarisViewModel();

            model.Inventaris = new List<InventarisModel>();
            model.Objecten = new List<SelectListItem>();
            model.Lokalen = new List<SelectListItem>();
            model.Verzekeringen = new List<SelectListItem>();
            model.Facturen = new List<SelectListItem>();
            model.Leverancieren = new List<SelectListItem>();

            InventarisModel i = TblInventaris.GetById(id);
            model.Inventaris.Add(i);

            foreach (ObjectModel o in TblObject.GetAll().OrderBy(x => x.Omschrijving))
            {
                model.Objecten.Add(new SelectListItem { Text = o.Omschrijving, Value = o.Id.ToString() });
            }
            foreach (LokaalModel l in TblLokaal.GetAll().OrderBy(x => x.Campus.Naam))
            {
                model.Lokalen.Add(new SelectListItem { Text = l.LokaalNaam + ", " + l.Campus.Naam, Value = l.Id.ToString() });
            }
            foreach (VerzekeringModel v in TblVerzekering.GetAll().OrderBy(x => x.Omschrijving))
            {
                model.Verzekeringen.Add(new SelectListItem { Text = v.Omschrijving, Value = v.Id.ToString() });
            }
            foreach (FactuurModel f in TblFactuur.GetAll().OrderBy(x => x.CvoFactuurNummer))
            {
                model.Facturen.Add(new SelectListItem { Text = f.CvoFactuurNummer, Value = f.Id.ToString() });
            }
            foreach (LeverancierModel l in TblLeverancier.GetAll())
            {
                model.Leverancieren.Add(new SelectListItem { Text = l.Naam, Value = l.Id.ToString() });
            }
            return View(model);

        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int? Objecten, int? Lokalen, int? Verzekeringen, int? Facturen, int? Leverancieren, string waarde, int? aankoopjaar, int? afschrijvingsperiode, string costcenter, string boekhoudnr)
        {
            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();


            InventarisModel inventaris = new InventarisModel();
            inventaris.Id = Convert.ToInt16(Request.Form["idInventaris"]);
            inventaris.Aankoopjaar = aankoopjaar;
            inventaris.Afschrijvingsperiode = afschrijvingsperiode;
            inventaris.Historiek = Request.Form["historiek"];
            inventaris.Costcenter = costcenter;
            inventaris.Boekhoudnr = boekhoudnr;
            if (string.IsNullOrWhiteSpace(waarde))
            {
                inventaris.Waarde = null;
            }
            else
            {
                inventaris.Waarde = Convert.ToDecimal(waarde.Replace(".", ","));
            }
            inventaris.Object = new ObjectModel() { Id = Objecten };
            inventaris.Lokaal = new LokaalModel() { Id = Lokalen };
            inventaris.Factuur = new FactuurModel() { Id = Facturen };
            inventaris.Verzekering = new VerzekeringModel() { Id = Verzekeringen };
            inventaris.Factuur = new FactuurModel() { Id = Facturen };
            inventaris.Leverancier = new LeverancierModel() { Id = Leverancieren };
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
                TempData["action"] = idArray.Length + " object werd verwijderd uit de inventaris";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Filter(int objectFilter, string aanwezigFilter, string actiefFilter, int lokaalFilter, string historiekFilter, string filterAankoopjaar,
            string filterAankoopjaarSecondary, string filterAfschrijvingsperiode, string filterAfschrijvingsperiodeSecondary, int verzekeringFilter,
            int? objecttypeFilter, int? factuurFilter, string waardeFilter, string waardeFilterSecondary, string costcenterFilter, string afschrijvingInJaarFilter,
            string boekhoudnrFilter, int? campusFilter, int? leverancierFilter, string filterLabel, int[] modelList)
        {
            ViewBag.action = TempData["action"];


            InventarisViewModel model = new InventarisViewModel();


            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();
            model.Inventaris = new List<InventarisModel>();

            model.Inventaris = TblInventaris.GetAll().OrderBy(i => i.Id).Reverse().ToList();


            Session["inventarisviewmodel"] = model.Clone();



            // Hier start filteren
            if (objecttypeFilter >= 0)
            {
                model.Inventaris.RemoveAll(x => x.Object.ObjectType == null);
                model.Inventaris.RemoveAll(x => x.Object.ObjectType.Id != objecttypeFilter);
            }


            if (objectFilter >= 0)
            {
                model.Inventaris.RemoveAll(x => x.Object.Id != objectFilter);
            }

            if (!String.IsNullOrWhiteSpace(aanwezigFilter))
            {
                if (aanwezigFilter.Equals("true"))
                {
                    model.Inventaris.RemoveAll(x => x.IsAanwezig != true);
                }
                else
                {
                    model.Inventaris.RemoveAll(x => x.IsAanwezig != false);
                }
            }
            if (!String.IsNullOrWhiteSpace(actiefFilter))
            {
                if (actiefFilter.Equals("true"))
                {
                    model.Inventaris.RemoveAll(x => x.IsActief != true);
                }
                else
                {
                    model.Inventaris.RemoveAll(x => x.IsActief != false);
                }
            }

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

            if (!String.IsNullOrWhiteSpace(waardeFilter))
            {
                if (waardeFilterSecondary.Equals("="))
                {
                    model.Inventaris.RemoveAll(x => x.Waarde != Convert.ToDecimal(waardeFilter.Replace(".", ",")));
                }
                else if (waardeFilterSecondary.Equals("<"))
                {
                    model.Inventaris.RemoveAll(x => x.Waarde > Convert.ToDecimal(waardeFilter.Replace(".", ",")));
                }
                else if (waardeFilterSecondary.Equals(">"))
                {
                    model.Inventaris.RemoveAll(x => x.Waarde < Convert.ToDecimal(waardeFilter.Replace(".", ",")));
                }
            }

            if (!String.IsNullOrWhiteSpace(costcenterFilter))
            {
                model.Inventaris.RemoveAll(x => !x.Costcenter.ToLower().Contains(costcenterFilter.ToLower()));
            }

            if (!String.IsNullOrWhiteSpace(boekhoudnrFilter))
            {
                model.Inventaris.RemoveAll(x => !x.Boekhoudnr.ToLower().Contains(boekhoudnrFilter.ToLower()));
            }

            if (!String.IsNullOrWhiteSpace(afschrijvingInJaarFilter))
            {
                model.Inventaris.RemoveAll(x => (x.Aankoopjaar + Convert.ToInt16(x.Afschrijvingsperiode)).ToString() != afschrijvingInJaarFilter);
            }

            if (factuurFilter >= 0)
            {
                model.Inventaris.RemoveAll(x => x.Factuur.Id != factuurFilter);
            }

            if (campusFilter >= 0)
            {
                model.Inventaris.RemoveAll(x => x.Lokaal.Campus.Id == null);
                model.Inventaris.RemoveAll(x => x.Lokaal.Campus.Id != campusFilter);
            }

            if (leverancierFilter >= 0)
            {
                model.Inventaris.RemoveAll(x => (x.Leverancier.Id != leverancierFilter));
            }

            if (verzekeringFilter >= 0)
            {
                model.Inventaris.RemoveAll(x => x.Verzekering.Id != verzekeringFilter);
            }
            ViewBag.Heading = this.ControllerContext.RouteData.Values["controller"].ToString() + " (" + model.Inventaris.Count() + ")";



            if (!String.IsNullOrWhiteSpace(filterLabel))
            {
                model.Inventaris.RemoveAll(x => !x.Label.ToLower().Contains(filterLabel.ToLower()));
            }

            decimal? totaalWaarde = 0;
            foreach (var item in model.Inventaris)
            {
                if (item.Waarde == null)
                {
                    item.Waarde = 0;
                }

                totaalWaarde += item.Waarde;
            }

            ViewBag.totaalWaarde = " (Totaalwaarde: € " + totaalWaarde.ToString() + ")";

            return View("index", model);
        }
    }
}
