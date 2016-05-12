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
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                List<Models.ObjectTypeModel> model = new List<Models.ObjectTypeModel>();
                foreach (ObjectTypes objectType in client.ObjectTypeGetAll())
                {
                    model.Add(new Models.ObjectTypeModel() { IdObjectType = objectType.Id, Omschrijving = objectType.Omschrijving });
                }
                return model;
            }
        }
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                List<Models.ObjectTypeModel> model = new List<Models.ObjectTypeModel>();
                foreach (ObjectTypes objectType in client.ObjectTypeGetAll())
                {
                    model.Add(new Models.ObjectTypeModel() { IdObjectType = objectType.Id, Omschrijving = objectType.Omschrijving });
                }
                return View(model);
            }
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                ObjectTypes objectType = client.ObjectTypeGetById(id);
                return View(new Models.ObjectTypeModel() { IdObjectType = objectType.Id,Omschrijving = objectType.Omschrijving });
            }
        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                ObjectTypes objectType = new ObjectTypes();
                objectType.Id = Convert.ToInt16(Request.Form["idObjectType"]);
                objectType.Omschrijving = Request.Form["omschrijving"];

                TempData["action"] = Request.Form["omschrijving"] + " werd aangepast";

                client.ObjectTypeUpdate(objectType);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                ObjectTypes objectType = new ObjectTypes();
                objectType.Id = Convert.ToInt16(Request.Form["driver"]);
                objectType.Omschrijving = Request.Form["omschrijving"];
                client.ObjectTypeCreate(objectType);

                TempData["action"] = "objectType" + " " + Request.Form["omschrijving"] + " werd toegevoegd";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                foreach (int id in idArray)
                {
                    client.ObjectTypeDelete(id);
                }
                if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " objectTypen werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " objectType werd verwijderd";
                }
            }
            return RedirectToAction("Index");
        }

    }
}