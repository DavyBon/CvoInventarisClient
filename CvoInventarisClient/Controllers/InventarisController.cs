using CvoInventarisClient.ServiceReference;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Controllers
{
    public class InventarisController : Controller
    {
        // GET: Inventaris
        public ActionResult Index()
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.InventarisGetAll());
            }
        }

        // GET: Inventaris/Details/5
        public ActionResult Details(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.InventarisGetById(id));
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
                ViewBag.action = Request.Form["label"] + " was added";

                Inventaris inventaris = new Inventaris();
                inventaris.aankoopjaar = Convert.ToInt32(Request.Form["aankoopjaar"]);
                inventaris.afschrijvingsperiode = Convert.ToInt32(Request.Form["afschrijvingsperiode"]);
                inventaris.historiek = Request.Form["historiek"];
                inventaris.idLokaal = Convert.ToInt32(Request.Form["idLokaal"]);
                inventaris.idObject = Convert.ToInt32(Request.Form["idObject"]);
                inventaris.idVerzekering = Convert.ToInt32(Request.Form["idVerzekering"]);
                //inventaris.isAanwezig = Boolean.Parse(Request.Form["isAanwezig"]);
                //inventaris.isActief = Convert.ToBoolean(Request.Form["isActief"]);
                inventaris.label = Request.Form["label"];
                client.InventarisCreate(inventaris);
            }
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.InventarisGetById(id));
            }
        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                ViewBag.action = Request.Form["label"] + " was added";

                Inventaris inventaris = new Inventaris();
                inventaris.id = id;
                inventaris.aankoopjaar = Convert.ToInt32(Request.Form["aankoopjaar"]);
                inventaris.afschrijvingsperiode = Convert.ToInt32(Request.Form["afschrijvingsperiode"]);
                inventaris.historiek = Request.Form["historiek"];
                inventaris.idLokaal = Convert.ToInt32(Request.Form["idLokaal"]);
                inventaris.idObject = Convert.ToInt32(Request.Form["idObject"]);
                inventaris.idVerzekering = Convert.ToInt32(Request.Form["idVerzekering"]);
                //inventaris.isAanwezig = Boolean.Parse(Request.Form["isAanwezig"]);
                //inventaris.isActief = Convert.ToBoolean(Request.Form["isActief"]);
                inventaris.label = Request.Form["label"];
                client.InventarisUpdate(inventaris);
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
                client.InventarisDelete(id);
            }
            return RedirectToAction("Index");
        }
    }
}
