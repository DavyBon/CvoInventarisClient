using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
namespace CvoInventarisClient.Controllers
{
    [Authorize]
    public class ObjectTypeController : Controller
    {
        public List<ObjectTypeModel> GetObjectTypes()
        {
            DAL.TblObjectType tblObjectType = new DAL.TblObjectType();
            List<ObjectTypeModel> model = tblObjectType.GetAll();
            return model;
            
        }
        public ActionResult Index(int? amount, string order, bool? refresh)
        {
            ViewBag.action = TempData["action"];
            ObjectTypeViewModel model = new ObjectTypeViewModel();

                DAL.TblObjectType tblObjectType = new DAL.TblObjectType();
                List<ObjectTypeModel> objectType = tblObjectType.GetAll().OrderBy(i => i.Id).Reverse().ToList();
                model.objectTypes = objectType;

            Session["objectTypeviewmodel"] = model.Clone();
            if (amount == null)
            {
                model.objectTypes = model.objectTypes.Take(100).ToList();
                ViewBag.amount = "100";
            }
            else
            {
                model.objectTypes = model.objectTypes.Take((int)amount).ToList();
                ViewBag.amount = amount.ToString();
            }

            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("Oudst"))
                {
                    model.objectTypes.Reverse();
                }
                ViewBag.ordertype = order.ToString();
            }
            else
            {
                ViewBag.ordertype = "Meest recent";
            }

            ViewBag.Heading = this.ControllerContext.RouteData.Values["controller"].ToString() + " (" + model.objectTypes.Count() + ")";

            return View(model);
            
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            DAL.TblObjectType tblObjectType = new DAL.TblObjectType();

            ObjectTypeViewModel model = new ObjectTypeViewModel();
            model.objectTypes = new List<ObjectTypeModel>();

            ObjectTypeModel objectType = tblObjectType.GetById(id);
            model.objectTypes.Add(objectType);
            return View(model);

        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            DAL.TblObjectType tblObjectType = new DAL.TblObjectType();
            ObjectTypeModel objectType = new ObjectTypeModel();
            objectType.Id = Convert.ToInt16(Request.Form["idObjectType"]);
            objectType.Omschrijving = Request.Form["omschrijving"];
            TempData["action"] = Request.Form["omschrijving"] + " werd aangepast";
            tblObjectType.Update(objectType);
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            ObjectTypeViewModel model = new ObjectTypeViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            DAL.TblObjectType tblObjectType = new DAL.TblObjectType();
            ObjectTypeModel objectType = new ObjectTypeModel();
            objectType.Omschrijving = Request.Form["omschrijving"];
            tblObjectType.Create(objectType);
            TempData["action"] = "objectType" + " " + Request.Form["omschrijving"] + " werd toegevoegd";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            if (idArray == null) { return RedirectToAction("Index"); }
            DAL.TblObjectType tblObjectType = new DAL.TblObjectType();
                foreach (int id in idArray)
                {
                tblObjectType.Delete(id);
                }
                if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " objectTypen werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " objectType werd verwijderd";
                }
            return RedirectToAction("Index");
        }

    }
}