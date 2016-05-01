using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class LokaalController : Controller
    {
        // INDEX:
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            return View(ReadAll());
        }

        private LokaalViewModel ReadAll()
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                LokaalViewModel model = new LokaalViewModel();
                model.Lokaal = new List<LokaalModel>();
                model.Netwerk = new List<NetwerkModel>();

                foreach (Lokaal lokaal in client.LokaalGetAll())
                {
                    LokaalModel lokaalModel = new LokaalModel();
                    lokaalModel.IdLokaal = lokaal.IdLokaal;
                    lokaalModel.LokaalNaam = lokaal.LokaalNaam;
                    lokaalModel.AantalPlaatsen = lokaal.AantalPlaatsen;
                    lokaalModel.IsComputerLokaal = lokaal.IsComputerLokaal;
                    lokaalModel.Netwerk = new NetwerkModel() { Id = lokaal.Netwerk.Id, Driver = lokaal.Netwerk.Driver, Merk = lokaal.Netwerk.Merk, Snelheid = lokaal.Netwerk.Snelheid, Type = lokaal.Netwerk.Type };
                    model.Lokaal.Add(lokaalModel);
                }

                foreach (Netwerk netwerk in client.NetwerkGetAll())
                {
                    NetwerkModel netwerkModel = new NetwerkModel();
                    netwerkModel.Id = netwerk.Id;
                    netwerkModel.Driver = netwerk.Driver;
                    netwerkModel.Merk = netwerk.Merk;
                    netwerkModel.Snelheid = netwerk.Snelheid;
                    netwerkModel.Type = netwerk.Type;
                    model.Netwerk.Add(netwerkModel);
                }
                return model;
            }
        }


        // CREATE:
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Lokaal lokaal = new Lokaal();
                lokaal.LokaalNaam = Request.Form["lokaalNaam"];
                lokaal.AantalPlaatsen = Convert.ToInt32(Request.Form["aantalPlaatsen"]);
                lokaal.IsComputerLokaal = Boolean.Parse(Request.Form["isComputerLokaal"]);
                lokaal.Netwerk.Id = Convert.ToInt32(Request.Form["idNetwerk"]);

                client.LokaalCreate(lokaal);

                TempData["action"] = "lokaal" + " " + Request.Form["lokaalNaam"] + " werd toegevoegd";
            }
            return RedirectToAction("Index");
        }

        // EDIT:
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.LokaalGetById(id));
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Lokaal lokaal = new Lokaal();
                lokaal.LokaalNaam = Request.Form["lokaalNaam"];
                lokaal.AantalPlaatsen = Convert.ToInt32(Request.Form["aantalPlaatsen"]);
                lokaal.IsComputerLokaal = Convert.ToBoolean(Request.Form["isComputerLokaal"]);
                lokaal.Netwerk.Id = Convert.ToInt32(Request.Form["idNetwerk"]);

                client.LokaalUpdate(lokaal);

                TempData["action"] = "lokaal" + " " + Request.Form["lokaalNaam"] + " werd aangepast";
            }
            return RedirectToAction("Index");
        }


        // DELETE:
        [HttpPost]
        public ActionResult Delete(int[] idArray, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                foreach (int id in idArray)
                {
                    client.LokaalDelete(id);
                }
                if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " lokalen werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " lokaal werd verwijderd";
                }
            }
            return RedirectToAction("Index");            
        }
    }
}