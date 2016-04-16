using CvoInventarisClient.ServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Controllers
{
    public class NetwerkController : Controller
    {
        // GET: Inventaris
        public ActionResult Index()
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.NetwerkGetAll());
            }
        }

        // GET: Inventaris/Details/5
        public ActionResult Details(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.NetwerkGetById(id));
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
                ViewBag.action = Request.Form["merk"] + " was added";

                Netwerk netwerk = new Netwerk();
                netwerk.driver = Request.Form["driver"];
                netwerk.merk = Request.Form["merk"];
                netwerk.type = Request.Form["type"];
                netwerk.snelheid = Request.Form["snelheid"];


                client.NetwerkCreate(netwerk);
            }
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.NetwerkGetById(id));
            }
        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                ViewBag.action = Request.Form["merk"] + " was changed";

                Netwerk netwerk = new Netwerk();
                netwerk.driver = Request.Form["driver"];
                netwerk.merk = Request.Form["merk"];
                netwerk.type = Request.Form["type"];
                netwerk.snelheid = Request.Form["snelheid"];


                client.NetwerkUpdate(netwerk);
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
                client.NetwerkDelete(id);
            }
            return RedirectToAction("Index");
        }
    }
}
