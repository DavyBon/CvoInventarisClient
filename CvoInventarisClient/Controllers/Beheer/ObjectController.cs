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
            DAL.TblObject TblObject = new DAL.TblObject();
            DAL.TblFactuur TblFactuur = new DAL.TblFactuur();
            DAL.TblObjectType TblObjectType = new DAL.TblObjectType();

            ObjectViewModel model = new ObjectViewModel();
            model.Objecten = new List<ObjectModel>();
            model.Facturen = new List<SelectListItem>();
            model.ObjectTypes = new List<SelectListItem>();

            foreach (ObjectModel o in TblObject.GetAll())
            {
                model.Objecten.Add(o);
            }
            //uncomment na dal refactor
            //foreach (Factuur f in TblFactuur.FactuurGetAll())
            //{
            //    model.Facturen.Add(new SelectListItem { Text = f.FactuurNummer, Value = f.IdFactuur.ToString() });
            //}
            //foreach (ObjectTypes ot in TblOBjectType.ObjectTypeGetAll())
            //{
            //    model.ObjectTypes.Add(new SelectListItem { Text = ot.Omschrijving, Value = ot.Id.ToString() });
            //}
            return View(model);

        }

        // POST: Inventaris/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            DAL.TblObject TblObject = new DAL.TblObject();

            ObjectModel obj = new ObjectModel();
            obj.Kenmerken = Request.Form["kenmerken"];

            obj.Factuur = new FactuurModel() { IdFactuur = Convert.ToInt32(Request.Form["Facturen"]) };
            obj.ObjectType = new ObjectTypeModel() { IdObjectType = Convert.ToInt32(Request.Form["ObjectTypes"]) };
            TblObject.Create(obj);

            TempData["action"] = "Object werd toegevoegd";
            return RedirectToAction("Index");
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

            //foreach (Factuur f in   TblObject.FactuurGetAll())
            //{
            //    model.Facturen.Add(new SelectListItem { Text = f.FactuurNummer, Value = f.IdFactuur.ToString() });
            //}
            //foreach (ObjectTypes ot in TblObject.ObjectTypeGetAll())
            //{
            //    model.ObjectTypes.Add(new SelectListItem { Text = ot.Omschrijving, Value = ot.Id.ToString() });
            //}
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
            if (!String.IsNullOrWhiteSpace(Request.Form["facturen"])) { obj.Factuur = new FactuurModel() { IdFactuur = Convert.ToInt16(Request.Form["Facturen"]) }; }
            else { obj.Factuur = new FactuurModel() { IdFactuur = Convert.ToInt16(Request.Form["defaultIdFactuur"]) }; }
            if (!String.IsNullOrWhiteSpace(Request.Form["ObjectTypes"])) { obj.ObjectType = new ObjectTypeModel() { IdObjectType = Convert.ToInt16(Request.Form["ObjectTypes"]) }; }
            else { obj.ObjectType = new ObjectTypeModel() { IdObjectType = Convert.ToInt16(Request.Form["defaultIdObjectType"]) }; }

            TblObject.Update(obj);

            TempData["action"] = "Object werd gewijzigd";
            return RedirectToAction("Index");
        }

        // POST: Inventaris/Delete/5
        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
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
            return RedirectToAction("Index");
        }
    }
}