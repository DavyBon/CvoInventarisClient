using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class VerzekeringController : Controller
    {
        public List<VerzekeringModel> Getverzekeringen()
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                List<Models.VerzekeringModel> model = new List<Models.VerzekeringModel>();
                foreach (Verzekering verzekering in client.VerzekeringGetAll())
                {
                    model.Add(new Models.VerzekeringModel() { IdVerzekering = verzekering.Id, Omschrijving = verzekering.Omschrijving });
                }
                return model;
            }
            }
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                List<Models.VerzekeringModel> model = new List<Models.VerzekeringModel>();
                foreach (Verzekering verzekering in client.VerzekeringGetAll())
                {
                    model.Add(new Models.VerzekeringModel() { IdVerzekering = verzekering.Id,Omschrijving = verzekering.Omschrijving});
                }
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Verzekering verzekering = new Verzekering();
                verzekering.Omschrijving = Request.Form["omschrijving"];
                client.VerzekeringCreate(verzekering);

                TempData["action"] = "verzekering" + " " + Request.Form["omschrijving"] + " werd toegevoegd";
            }
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Verzekering verzekering = client.VerzekeringGetById(id);
                return View(new Models.VerzekeringModel() { IdVerzekering = verzekering.Id,Omschrijving = verzekering.Omschrijving });
            }
        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Verzekering verzekering = new Verzekering();
                verzekering.Id = Convert.ToInt16(Request.Form["idVerzekering"]);
                verzekering.Omschrijving = Request.Form["omschrijving"];

                TempData["action"] = Request.Form["omschrijving"] + " werd aangepast";

                client.VerzekeringUpdate(verzekering);
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
                    client.VerzekeringDelete(id);
                }
                if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " verzekeringen werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " verzekeringen werd verwijderd";
                }
            }
            return RedirectToAction("Index");
        }
    }
}