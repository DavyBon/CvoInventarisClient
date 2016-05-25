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
        public ActionResult Index(int? amount, string order, bool? refresh)
        {
            ViewBag.action = TempData["action"];

            InventarisViewModel model = new InventarisViewModel();

            if (Session["inventarisviewmodel"] == null || refresh == true)
            {
                DAL.TblInventaris TblInventaris = new DAL.TblInventaris();
                DAL.TblObject TblObject = new DAL.TblObject();
                DAL.TblLokaal TblLokaal = new DAL.TblLokaal();
                DAL.TblVerzekering TblVerzekering = new DAL.TblVerzekering();
                DAL.TblFactuur TblFactuur = new DAL.TblFactuur();
                DAL.TblObjectType TblObjecttype = new DAL.TblObjectType();

                model.Inventaris = new List<InventarisModel>();
                model.Objecten = new List<SelectListItem>();
                model.Lokalen = new List<SelectListItem>();
                model.Verzekeringen = new List<SelectListItem>();
                model.Objecttypen = new List<SelectListItem>();
                model.Facturen = new List<SelectListItem>();

                model.Inventaris = TblInventaris.GetAll().OrderBy(i => i.Id).Reverse().ToList();

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
                foreach (ObjectTypeModel ot in TblObjecttype.GetAll())
                {
                    model.Objecttypen.Add(new SelectListItem { Text = ot.Omschrijving, Value = ot.Id.ToString() });
                }
                foreach (FactuurModel f in TblFactuur.GetAll())
                {
                    model.Facturen.Add(new SelectListItem { Text = f.FactuurNummer, Value = f.Id.ToString() });
                }
            }
            else
            {
                model = (InventarisViewModel)Session["inventarisviewmodel"];
            }
            Session["inventarisviewmodel"] = model.Clone();
            if (amount == null)
            {
                model.Inventaris = model.Inventaris.Take(100).ToList();
                ViewBag.amount = "100";
            }
            else
            {
                model.Inventaris = model.Inventaris.Take((int)amount).ToList();
                ViewBag.amount = amount.ToString();
            }

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

        // POST: Inventaris/Create
        [HttpPost]
        public ActionResult Create(int? Objecten, int? Lokalen, int? Verzekeringen)
        {
            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();

            for (int i = 0; i < Convert.ToInt32(Request.Form["aantal"]); i++)
            {
                int labelnr = Convert.ToInt32(Request.Form["volgnummer"]) + i;
                InventarisModel inventaris = new InventarisModel();
                inventaris.Aankoopjaar = Convert.ToInt32(Request.Form["aankoopjaar"]);
                inventaris.Afschrijvingsperiode = Convert.ToInt32(Request.Form["afschrijvingsperiode"]);
                inventaris.Historiek = Request.Form["historiek"];
                inventaris.Label = Request.Form["reeks"] + labelnr.ToString().PadLeft(4, '0');
                inventaris.Object = new ObjectModel() { Id = Objecten };
                inventaris.Lokaal = new LokaalModel() { Id = Lokalen };
                inventaris.Verzekering = new VerzekeringModel() { Id = Verzekeringen };
                inventaris.Factuur = new FactuurModel();
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
            return RedirectToAction("Index", new { refresh = true });
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();
            DAL.TblVerzekering TblVerzekering = new DAL.TblVerzekering();
            DAL.TblFactuur TblFactuur = new DAL.TblFactuur();
            DAL.TblLokaal TblLokaal = new DAL.TblLokaal();
            InventarisViewModel model = new InventarisViewModel();

            model.Inventaris = new List<InventarisModel>();
            model.Objecten = new List<SelectListItem>();
            model.Lokalen = new List<SelectListItem>();
            model.Verzekeringen = new List<SelectListItem>();
            model.Facturen = new List<SelectListItem>();

            InventarisModel i = TblInventaris.GetById(id);
            model.Inventaris.Add(i);

            foreach (LokaalModel l in TblLokaal.GetAll())
            {
                model.Lokalen.Add(new SelectListItem { Text = l.LokaalNaam, Value = l.Id.ToString() });
            }
            foreach (VerzekeringModel v in TblVerzekering.GetAll())
            {
                model.Verzekeringen.Add(new SelectListItem { Text = v.Omschrijving, Value = v.Id.ToString() });
            }
            foreach (FactuurModel f in TblFactuur.GetAll())
            {
                model.Facturen.Add(new SelectListItem { Text = f.FactuurNummer, Value = f.Id.ToString() });
            }
            return View(model);

        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int? idObject, int? Lokalen, int? Verzekeringen, int? Facturen)
        {
            DAL.TblInventaris TblInventaris = new DAL.TblInventaris();

            InventarisModel inventaris = new InventarisModel();
            inventaris.Id = Convert.ToInt16(Request.Form["idInventaris"]);
            inventaris.Aankoopjaar = Convert.ToInt32(Request.Form["aankoopjaar"]);
            inventaris.Afschrijvingsperiode = Convert.ToInt32(Request.Form["afschrijvingsperiode"]);
            inventaris.Historiek = Request.Form["historiek"];
            inventaris.Object = new ObjectModel() { Id = idObject };
            inventaris.Lokaal = new LokaalModel() { Id = Lokalen };
            inventaris.Verzekering = new VerzekeringModel() { Id = Verzekeringen };
            inventaris.Factuur = new FactuurModel() { Id = Facturen };


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
            return RedirectToAction("Index", new { refresh = true });
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
            return RedirectToAction("Index", new { refresh = true });
        }

        [HttpPost]
        public ActionResult Filter(int objectFilter, string aanwezigFilter, string actiefFilter, int lokaalFilter, string historiekFilter, string filterAankoopjaar, string filterAankoopjaarSecondary, string filterAfschrijvingsperiode, string filterAfschrijvingsperiodeSecondary, int verzekeringFilter, int? objecttypeFilter, int? factuurFilter, int[] modelList)
        {
            ViewBag.action = TempData["action"];


            InventarisViewModel model = new InventarisViewModel();

            if (Session["inventarisviewmodel"] == null)
            {
                DAL.TblInventaris TblInventaris = new DAL.TblInventaris();
                DAL.TblObject TblObject = new DAL.TblObject();
                DAL.TblLokaal TblLokaal = new DAL.TblLokaal();
                DAL.TblVerzekering TblVerzekering = new DAL.TblVerzekering();
                DAL.TblFactuur TblFactuur = new DAL.TblFactuur();
                DAL.TblObjectType TblObjecttype = new DAL.TblObjectType();

                model.Inventaris = new List<InventarisModel>();
                model.Objecten = new List<SelectListItem>();
                model.Lokalen = new List<SelectListItem>();
                model.Verzekeringen = new List<SelectListItem>();
                model.Objecttypen = new List<SelectListItem>();
                model.Facturen = new List<SelectListItem>();

                model.Inventaris = TblInventaris.GetAll().OrderBy(i => i.Id).Reverse().ToList();

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
                foreach (ObjectTypeModel ot in TblObjecttype.GetAll())
                {
                    model.Objecttypen.Add(new SelectListItem { Text = ot.Omschrijving, Value = ot.Id.ToString() });
                }
                foreach (FactuurModel f in TblFactuur.GetAll())
                {
                    model.Facturen.Add(new SelectListItem { Text = f.FactuurNummer, Value = f.Id.ToString() });
                }

                Session["inventarisviewmodel"] = model.Clone();
            }
            else
            {
                model = (InventarisViewModel)Session["inventarisviewmodel"];
            }



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

            if (factuurFilter >= 0)
            {
                model.Inventaris.RemoveAll(x => x.Factuur.Id != factuurFilter);
            }

            if (verzekeringFilter >= 0)
            {
                model.Inventaris.RemoveAll(x => x.Verzekering.Id != verzekeringFilter);
            }
            ViewBag.Heading = this.ControllerContext.RouteData.Values["controller"].ToString() + " (" + model.Inventaris.Count() + ")";
            return View("index", model);
        }
    }
}
