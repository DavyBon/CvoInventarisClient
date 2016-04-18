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

            Lokaal[] arrLokalen = new Lokaal[] { };

            try
            {
                arrLokalen = sr.LokaalGetAll();
            }
            catch (Exception e)
            {

            }

            List<LokaalModel> listLokalen = new List<LokaalModel>();

            foreach (Lokaal lokaal in arrLokalen)
            {
                LokaalModel lk = new LokaalModel();
                lk.IdLokaal = lokaal.IdLokaal;
                lk.LokaalNaam = lokaal.LokaalNaam;
                lk.AantalPlaatsen = lokaal.AantalPlaatsen;
                lk.IsComputerLokaal = lokaal.IsComputerLokaal;
                lokaal.IdNetwerk = Convert.ToInt32(lk.Netwerk);
                //lk.IdNetwerk = lokaal.idNetwerk;
                listLokalen.Add(lk);
            }

            return listLokalen;
        }


        // INSERT:

        [HttpGet]
        public ActionResult Insert()
        {
            return View(new LokaalModel());
        }

        [HttpPost]
        public ActionResult insertLokaal(LokaalModel lk)
        {
            if (InsertLokaal(lk) >= 0)
            {
                ViewBag.CreateMessage = "Row inserted";
                return View("Index", ReadAll());
            }
            else
            {
                ViewBag.EditMessage = "Row not inserted";
                return View();
            }
        }

        public int InsertLokaal(LokaalModel lk)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Lokaal lokaal = new Lokaal();
            lokaal.LokaalNaam = lk.LokaalNaam;
            lokaal.AantalPlaatsen = lk.AantalPlaatsen;
            lokaal.IsComputerLokaal = lk.IsComputerLokaal;
            lokaal.IdNetwerk = Convert.ToInt32(lk.Netwerk);
            //lokaal.idNetwerk = lk.IdNetwerk;

            try
            {
                return sr.LokaalCreate(lokaal);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        // DETAILS:

        public ActionResult Details(int? id)
        {
            return View(GetLokaalById((int)id));
        }

        public LokaalModel GetLokaalById(int id)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Lokaal lokaal = new Lokaal();

            try
            {
                lokaal = sr.LokaalGetById(id);
            }
            catch (Exception e)
            {

            }

            LokaalModel lk = new LokaalModel();
            lk.IdLokaal = lokaal.IdLokaal;
            lk.LokaalNaam = lokaal.LokaalNaam;
            lk.AantalPlaatsen = lokaal.AantalPlaatsen;
            lk.IsComputerLokaal = lokaal.IsComputerLokaal;
            lokaal.IdNetwerk = Convert.ToInt32(lk.Netwerk);
            //lk.IdNetwerk = lokaal.idNetwerk;

            return lk;
        }


        // UPDATE:

        [HttpGet]
        public ActionResult Update(int? id)
        {
            return View(GetLokaalById((int)id));
        }

        [HttpPost]
        public ActionResult updateLokaal(LokaalModel lk)
        {
            if (UpdateLokaal(lk))
            {
                ViewBag.EditMessage = "Row Updated";
                return View("Index", ReadAll());
            }
            else
            {
                ViewBag.EditMessage = "Row not updated";
                return View();
            }
        }

        public bool UpdateLokaal(LokaalModel lk)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Lokaal lokaal = new Lokaal();
            lokaal.IdLokaal = lk.IdLokaal;
            lokaal.LokaalNaam = lk.LokaalNaam;
            lokaal.AantalPlaatsen = lk.AantalPlaatsen;
            lokaal.IsComputerLokaal = lk.IsComputerLokaal;
            lokaal.IdNetwerk = Convert.ToInt32(lk.Netwerk);
            //lokaal.idNetwerk = lk.IdNetwerk;

            try
            {
                return sr.LokaalUpdate(lokaal);
            }
            catch (Exception)
            {
                return false;
            }
        }


        // DELETE:

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return View(GetLokaalById((int)id));
        }

        [HttpPost]
        public ActionResult deleteLokaal(LokaalModel lk)
        {
            if (DeleteLokaal(lk))
            {
                ViewBag.DeleteMessage = "Row deleted";
                return View("Index", ReadAll());
            }
            else
            {
                ViewBag.DeletMessage = "Row not deleted";
                return View();
            }
        }

        public bool DeleteLokaal(LokaalModel lk)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            int id = lk.IdLokaal;

            try
            {
                return sr.LokaalDelete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}