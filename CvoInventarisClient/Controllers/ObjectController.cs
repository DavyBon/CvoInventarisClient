using CvoInventarisClient.ServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisMalik.Controllers
{
    public class ObjectController : Controller
    {
        // GET: Inventaris
        public ActionResult Index()
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.ObjectGetAll());
            }
        }

        // GET: Inventaris/Details/5
        public ActionResult Details(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.ObjectGetById(id));
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
                ViewBag.action = Request.Form["kenmerken"] + " was added";

                CvoInventarisClient.ServiceReference.Object obj = new CvoInventarisClient.ServiceReference.Object();
                obj.kenmerken = Request.Form["kenmerken"];
                obj.idFactuur = Convert.ToInt32(Request.Form["idFactuur"]);
                obj.idLeverancier = Convert.ToInt32(Request.Form["idLeverancier"]);
                obj.idObjectType = Convert.ToInt32(Request.Form["idObjectType"]);


                client.ObjectCreate(obj);
            }
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.ObjectGetById(id));
            }
        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                ViewBag.action = Request.Form["kenmerken"] + " was changed";

                CvoInventarisClient.ServiceReference.Object obj = new CvoInventarisClient.ServiceReference.Object();
                obj.id = id;
                obj.kenmerken = Request.Form["kenmerken"];
                obj.idFactuur = Convert.ToInt32(Request.Form["idFactuur"]);
                obj.idLeverancier = Convert.ToInt32(Request.Form["idLeverancier"]);
                obj.idObjectType = Convert.ToInt32(Request.Form["idObjectType"]);


                client.ObjectUpdate(obj);
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
                client.ObjectDelete(id);
            }
            return RedirectToAction("Index");
        }
    }
}