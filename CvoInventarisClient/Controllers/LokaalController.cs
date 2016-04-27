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
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {
                List<Lokaal> listLokaal = sr.LokaalGetAll().ToList();

                List<LokaalModel> listLokalen = new List<LokaalModel>();

                foreach (Lokaal lokaal in listLokaal)
                {
                    Netwerk netwerk = sr.NetwerkGetById(lokaal.Netwerk.Id);

                    NetwerkModel netwerkModel = new NetwerkModel();
                    netwerkModel.Id = netwerk.Id;
                    netwerkModel.Driver = netwerk.Driver;
                    netwerkModel.Merk = netwerk.Merk;
                    netwerkModel.Snelheid = netwerk.Snelheid;
                    netwerkModel.Type = netwerk.Type;

                    LokaalModel lokaalModel = new LokaalModel();
                    lokaalModel.IdLokaal = lokaal.IdLokaal;
                    lokaalModel.LokaalNaam = lokaal.LokaalNaam;
                    lokaalModel.AantalPlaatsen = lokaal.AantalPlaatsen;
                    lokaalModel.IsComputerLokaal = lokaal.IsComputerLokaal;
                    lokaalModel.Netwerk = netwerkModel;
                    listLokalen.Add(lokaalModel);
                }
                return listLokalen;
            }
        }


        // INSERT:

        [HttpGet]
        public ActionResult Insert()
        {
            return View(new LokaalModel());
        }

        [HttpPost]
        public ActionResult insertLokaal(LokaalModel lokaalModel)
        {
            if (InsertLokaal(lokaalModel) >= 0)
            {
                return View("Index", ReadAll());
            }
            else
            {
                return View("Insert");
            }
        }

        public int InsertLokaal(LokaalModel lokaalModel)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                Lokaal lokaal = new Lokaal();
                lokaal.LokaalNaam = lokaalModel.LokaalNaam;
                lokaal.AantalPlaatsen = lokaalModel.AantalPlaatsen;
                lokaal.IsComputerLokaal = lokaalModel.IsComputerLokaal;
                lokaal.Netwerk.Id = Convert.ToInt32(lokaalModel.Netwerk.Id);

                try
                {
                    return sr.LokaalCreate(lokaal);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        // DETAILS:

        public ActionResult Details(int? id)
        {
            return View(GetLokaalById((int)id));
        }

        public LokaalModel GetLokaalById(int id)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                Lokaal lokaal = new Lokaal();

                try
                {
                    lokaal = sr.LokaalGetById(id);
                }
                catch (Exception e)
                {

                }

                Netwerk netwerk = sr.NetwerkGetById(lokaal.Netwerk.Id);

                NetwerkModel netwerkModel = new NetwerkModel();
                netwerkModel.Id = netwerk.Id;
                netwerkModel.Driver = netwerk.Driver;
                netwerkModel.Merk = netwerk.Merk;
                netwerkModel.Snelheid = netwerk.Snelheid;
                netwerkModel.Type = netwerk.Type;

                LokaalModel lokaalModel = new LokaalModel();
                lokaalModel.IdLokaal = lokaal.IdLokaal;
                lokaalModel.LokaalNaam = lokaal.LokaalNaam;
                lokaalModel.AantalPlaatsen = lokaal.AantalPlaatsen;
                lokaalModel.IsComputerLokaal = lokaal.IsComputerLokaal;
                lokaalModel.Netwerk = netwerkModel;

                return lokaalModel;
            }
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
                return View("Index", ReadAll());
            }
            else
            {
                return View("Update");
            }
        }

        public bool UpdateLokaal(LokaalModel lokaalModel)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                Lokaal lokaal = new Lokaal();
                lokaal.IdLokaal = lokaalModel.IdLokaal;
                lokaal.LokaalNaam = lokaalModel.LokaalNaam;
                lokaal.AantalPlaatsen = lokaalModel.AantalPlaatsen;
                lokaal.IsComputerLokaal = lokaalModel.IsComputerLokaal;
                lokaal.Netwerk.Id = Convert.ToInt32(lokaalModel.Netwerk.Id);

                try
                {
                    return sr.LokaalUpdate(lokaal);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


        // DELETE:

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return View(GetLokaalById((int)id));
        }

        [HttpPost]
        public ActionResult deleteLokaal(LokaalModel lokaalModel)
        {
            if (DeleteLokaal(lokaalModel))
            {
                return View("Index", ReadAll());
            }
            else
            {
                return View("Delete");
            }
        }

        public bool DeleteLokaal(LokaalModel lokaalModel)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                int id = lokaalModel.IdLokaal;

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
}