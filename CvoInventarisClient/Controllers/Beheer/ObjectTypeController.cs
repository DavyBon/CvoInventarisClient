using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;
namespace CvoInventarisClient.Controllers
{
    public class ObjectTypeController : Controller
    {
        public List<ObjectTypeModel> GetObjectTypes()
        {
            DAL.TblObjectType tblObjectType = new DAL.TblObjectType();
            List<ObjectTypeModel> model = tblObjectType.GetAll();
            return model;
            
        }
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            DAL.TblObjectType tblObjectType = new DAL.TblObjectType();
            List<ObjectTypeModel> model = tblObjectType.GetAll();
            return View(model);
            
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            DAL.TblObjectType tblObjectType = new DAL.TblObjectType();
            ObjectTypeModel objectType = new ObjectTypeModel();
            objectType = tblObjectType.GetById(id);
            return View(objectType);
            
        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            DAL.TblObjectType tblObjectType = new DAL.TblObjectType();
            ObjectTypeModel objectType = new ObjectTypeModel();
            objectType.IdObjectType = Convert.ToInt16(Request.Form["idObjectType"]);
            objectType.Omschrijving = Request.Form["omschrijving"];
            TempData["action"] = Request.Form["omschrijving"] + " werd aangepast";
            tblObjectType.Update(objectType);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            DAL.TblObjectType tblObjectType = new DAL.TblObjectType();
            ObjectTypeModel objectType = new ObjectTypeModel();
            objectType.IdObjectType = Convert.ToInt16(Request.Form["driver"]);
            objectType.Omschrijving = Request.Form["omschrijving"];
            tblObjectType.Create(objectType);
            TempData["action"] = "objectType" + " " + Request.Form["omschrijving"] + " werd toegevoegd";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
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