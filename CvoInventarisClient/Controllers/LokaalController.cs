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
            return View(ReadAll());
        }

        private List<LokaalModel> ReadAll()
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Lokaal[] arrLokalen = sr.LokaalGetAll();

            List<LokaalModel> listLokalen = new List<LokaalModel>();

            foreach (var item in arrLokalen)
            {
                LokaalModel lk = new LokaalModel();
                lk.idLokaal = item.idLokaal;
                lk.lokaalNaam = item.lokaalNaam;
                lk.aantalPlaatsen = item.aantalPlaatsen;
                lk.isComputerLokaal = item.isComputerLokaal;
                lk.idNetwerk = item.idNetwerk;
                listLokalen.Add(lk);
            }

            return listLokalen;
        }


        // INSERT:

        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult insertLokaal(Lokaal lk)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            sr.LokaalCreate(lk);

            return View("Index", ReadAll());
        }


        // UPDATE:

        [HttpGet]
        public ActionResult Update(int? id)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Lokaal lokaal = sr.LokaalGetById((int)id);

            LokaalModel lokaalModel = new LokaalModel();

            lokaalModel.idLokaal = (int)id;
            lokaalModel.lokaalNaam = lokaal.lokaalNaam;
            lokaalModel.aantalPlaatsen = lokaal.aantalPlaatsen;
            lokaalModel.isComputerLokaal = lokaal.isComputerLokaal;
            lokaalModel.idNetwerk = lokaal.idNetwerk;

            return View(lokaalModel);
        }

        [HttpPost]
        public ActionResult updateLokaal(Lokaal lk)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            sr.LokaalUpdate(lk);

            return View("Index", ReadAll());
        }


        // DELETE:

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Lokaal lokaal = sr.LokaalGetById((int)id);

            LokaalModel lokaalModel = new LokaalModel();

            lokaalModel.idLokaal = (int)id;
            lokaalModel.lokaalNaam = lokaal.lokaalNaam;
            lokaalModel.aantalPlaatsen = lokaal.aantalPlaatsen;
            lokaalModel.isComputerLokaal = lokaal.isComputerLokaal;
            lokaalModel.idNetwerk = lokaal.idNetwerk;

            return View(lokaalModel);
        }

        [HttpPost]
        public ActionResult deleteLokaal(Lokaal lk)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            sr.LokaalDelete(lk.idLokaal);

            return View("Index", ReadAll());
        }
    }
}