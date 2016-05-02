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
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                List<Models.NetwerkModel> model = new List<Models.NetwerkModel>();
                foreach (Netwerk net in client.NetwerkGetAll())
                {
                    model.Add(new Models.NetwerkModel() { Id = net.Id, Driver = net.Driver, Merk = net.Merk, Snelheid = net.Snelheid, Type = net.Type });
                }
                return View(model);
            }
        }

        // POST: Inventaris/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Netwerk netwerk = new Netwerk();
                netwerk.Driver = Request.Form["driver"];
                netwerk.Merk = Request.Form["merk"];
                netwerk.Type = Request.Form["type"];
                netwerk.Snelheid = Request.Form["snelheid"];
                client.NetwerkCreate(netwerk);

                TempData["action"] = "netwerk" + " " + Request.Form["merk"] + " werd toegevoegd";
            }
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Netwerk net = client.NetwerkGetById(id);
                return View(new Models.NetwerkModel() {Id=net.Id, Driver= net.Driver, Merk = net.Merk, Snelheid = net.Snelheid, Type = net.Type});
            }
        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {             
                Netwerk netwerk = new Netwerk();
                netwerk.Id = Convert.ToInt16(Request.Form["idNetwerk"]);
                netwerk.Driver = Request.Form["driver"];
                netwerk.Merk = Request.Form["merk"];
                netwerk.Type = Request.Form["type"];
                netwerk.Snelheid = Request.Form["snelheid"];

                TempData["action"] = Request.Form["merk"] + " werd aangepast";

                client.NetwerkUpdate(netwerk);
            }
            return RedirectToAction("Index");
        }

        // POST: Inventaris/Delete/5
        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                foreach (int id in idArray)
                {
                    client.NetwerkDelete(id);
                }
                if(idArray.Length>=2)
                {
                    TempData["action"] = idArray.Length + " netwerken werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " netwerk werd verwijderd";
                }
            }
            return RedirectToAction("Index");
        }
    }
}
