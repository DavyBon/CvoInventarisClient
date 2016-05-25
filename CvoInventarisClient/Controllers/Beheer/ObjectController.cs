using CvoInventarisClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Controllers
{
    [Authorize]
    public class ObjectController : Controller
    {
        // GET: Inventaris
        public ActionResult Index(int? amount, string order, bool? refresh)
        {
            ViewBag.action = TempData["action"];

            ObjectViewModel model = new ObjectViewModel();

            if (Session["objectviewmodel"] == null || refresh == true)
            {

                DAL.TblObject TblObject = new DAL.TblObject();
                DAL.TblFactuur TblFactuur = new DAL.TblFactuur();
                DAL.TblObjectType TblObjectType = new DAL.TblObjectType();

                model.Objecten = new List<ObjectModel>();
                model.Facturen = new List<SelectListItem>();
                model.ObjectTypes = new List<SelectListItem>();


                model.Objecten = TblObject.GetAll().OrderBy(i => i.Id).Reverse().ToList();

                foreach (FactuurModel f in TblFactuur.GetAll())
                {
                    model.Facturen.Add(new SelectListItem { Text = f.CvoFactuurNummer, Value = f.Id.ToString() });
                }
                foreach (ObjectTypeModel ot in TblObjectType.GetAll())
                {
                    model.ObjectTypes.Add(new SelectListItem { Text = ot.Omschrijving, Value = ot.Id.ToString() });
                }
            }
            else
            {
                model = (ObjectViewModel)Session["objectviewmodel"];
            }
            this.Session["objectviewmodel"] = model;
            Session["inventarisviewmodel"] = model.Clone();
            if (amount == null)
            {
                model.Objecten = model.Objecten.Take(100).ToList();
                ViewBag.amount = "100";
            }
            else
            {
                model.Objecten = model.Objecten.Take((int)amount).ToList();
                ViewBag.amount = amount.ToString();
            }

            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("Oudst"))
                {
                    model.Objecten.Reverse();
                }
                else if (order.Equals("Type"))
                {
                    model.Objecten = model.Objecten.OrderBy(o => o.ObjectType.Id).ToList();
                }
                ViewBag.ordertype = order.ToString();
            }
            else
            {
                ViewBag.ordertype = "Meest recent";
            }

            ViewBag.Heading = this.ControllerContext.RouteData.Values["controller"].ToString() + " (" + model.Objecten.Count() + ")";

            return View(model);

        }

        // POST: Inventaris/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            DAL.TblObject TblObject = new DAL.TblObject();

            ObjectModel obj = new ObjectModel();
            obj.Kenmerken = Request.Form["kenmerken"];

            obj.ObjectType = new ObjectTypeModel() { Id = Convert.ToInt32(Request.Form["ObjectTypes"]) };
            TblObject.Create(obj);

            TempData["action"] = "Object werd toegevoegd";
            return RedirectToAction("Index", new { refresh = true });
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            DAL.TblObject TblObject = new DAL.TblObject();
            DAL.TblFactuur TblFactuur = new DAL.TblFactuur();
            DAL.TblObjectType TblObjectType = new DAL.TblObjectType();

            ObjectViewModel model = new ObjectViewModel();
            model.Objecten = new List<ObjectModel>();
            model.Facturen = new List<SelectListItem>();
            model.ObjectTypes = new List<SelectListItem>();

            ObjectModel o = TblObject.GetById(id);

            model.Objecten.Add(o);

            foreach (FactuurModel f in TblFactuur.GetAll())
            {
                model.Facturen.Add(new SelectListItem { Text = f.CvoFactuurNummer, Value = f.Id.ToString() });
            }
            foreach (ObjectTypeModel ot in TblObjectType.GetAll())
            {
                model.ObjectTypes.Add(new SelectListItem { Text = ot.Omschrijving, Value = ot.Id.ToString() });
            }
            return View(model);

        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            DAL.TblObject TblObject = new DAL.TblObject();

            ViewBag.action = Request.Form["kenmerken"] + " was changed";

            ObjectModel obj = new ObjectModel();
            obj.Id = Convert.ToInt32(Request.Form["idObject"]);
            obj.Kenmerken = Request.Form["kenmerken"];
            if (!String.IsNullOrWhiteSpace(Request.Form["ObjectTypes"])) { obj.ObjectType = new ObjectTypeModel() { Id = Convert.ToInt16(Request.Form["ObjectTypes"]) }; }
            else { obj.ObjectType = new ObjectTypeModel() { Id = Convert.ToInt16(Request.Form["defaultIdObjectType"]) }; }

            TblObject.Update(obj);

            TempData["action"] = "Object werd gewijzigd";
            return RedirectToAction("Index", new { refresh = true });
        }

        // POST: Inventaris/Delete/5
        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            if (idArray == null) { return RedirectToAction("Index"); }
            DAL.TblObject TblObject = new DAL.TblObject();
            foreach (int id in idArray)
            {
                TblObject.Delete(id);
            }

            if (idArray.Length >= 2)
            {
                TempData["action"] = idArray.Length + " objecten werden verwijderd uit objecten";
            }
            else
            {
                TempData["action"] = idArray.Length + " object werd verwijderd uit objecten";
            }
            return RedirectToAction("Index", new { refresh = true });
        }

        [HttpPost]
        public ActionResult Filter(string kenmerkenFilter, int ObjectTypeFilter, int[] modelList)
        {

            ViewBag.action = TempData["action"];


            ObjectViewModel model = new ObjectViewModel();

            if (Session["objectviewmodel"] == null)
            {

                DAL.TblObject TblObject = new DAL.TblObject();
                DAL.TblFactuur TblFactuur = new DAL.TblFactuur();
                DAL.TblObjectType TblObjectType = new DAL.TblObjectType();

                model.Objecten = new List<ObjectModel>();
                model.Facturen = new List<SelectListItem>();
                model.ObjectTypes = new List<SelectListItem>();


                model.Objecten = TblObject.GetAll().OrderBy(i => i.Id).Reverse().ToList();

                foreach (FactuurModel f in TblFactuur.GetAll())
                {
                    model.Facturen.Add(new SelectListItem { Text = f.CvoFactuurNummer, Value = f.Id.ToString() });
                }
                foreach (ObjectTypeModel ot in TblObjectType.GetAll())
                {
                    model.ObjectTypes.Add(new SelectListItem { Text = ot.Omschrijving, Value = ot.Id.ToString() });
                }
            }
            else
            {
                model = (ObjectViewModel)Session["objectviewmodel"];
            }

            if (!String.IsNullOrWhiteSpace(kenmerkenFilter))
            {
                model.Objecten.RemoveAll(x => !x.Kenmerken.ToLower().Contains(kenmerkenFilter.ToLower()));
            }

            if (ObjectTypeFilter >= 0)
            {
                model.Objecten.RemoveAll(x => x.ObjectType.Id != ObjectTypeFilter);
            }


            return View("index", model);
        }
    }
}