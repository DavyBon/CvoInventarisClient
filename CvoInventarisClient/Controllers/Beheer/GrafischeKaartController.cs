using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class GrafischeKaartController : Controller
    {
        public ActionResult Index()
        {
            return View(GetGrafischeKaarten());
        }

        public ActionResult Edit(int id)
        {
            return View(GetGrafischeKaartById(id));
        }

        [HttpPost]
        public ActionResult Edit(GrafischeKaartModel gkm)
        {
            if (UpdateGrafischeKaart(gkm))
            {
                ViewBag.EditMesage = "Row updated";
                //return View("Index");
                return View();
            }
            else
            {
                ViewBag.EditMesage = "Row not updated";
                return View();
            }
        }

        public ActionResult Create()
        {
            return View(new GrafischeKaartModel());
        }

        [HttpPost]
        public ActionResult Create(GrafischeKaartModel gkm)
        {
            if (InsertGrafischeKaart(gkm) >= 0)
            {
                ViewBag.CreateMesage = "Row inserted";
                //return View("Index");
                return View();
            }
            else
            {
                ViewBag.CreateMesage = "Row not inserted";
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            return View(GetGrafischeKaartById(id));
        }

        public ActionResult Delete(int id)
        {
            return View(GetGrafischeKaartById(id));
        }

        [HttpPost]
        public ActionResult Delete(GrafischeKaartModel gkm)
        {
            if (DeleteGrafischeKaart(gkm))
            {
                ViewBag.DeleteMesage = "Row deleted";
                //return View("Index");
                return View();
            }
            else
            {
                ViewBag.DeleteMesage = "Row not deleted";
                return View();
            }
        }

        public List<GrafischeKaartModel> GetGrafischeKaarten()
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            GrafischeKaart[] g = new GrafischeKaart[] { };

            try
            {
                g = service.GrafischeKaartGetAll();
            }
            catch
            {
            }

            List<GrafischeKaartModel> gks = new List<GrafischeKaartModel>();

            foreach (GrafischeKaart gk in g)
            {
                GrafischeKaartModel gkm = new GrafischeKaartModel();
                gkm.IdGrafischeKaart = gk.IdGrafischeKaart;
                gkm.Merk = gk.Merk;
                gkm.Type = gk.Type;
                gkm.Driver = gk.Driver;
                gkm.FabrieksNummer = gk.FabrieksNummer;
                gks.Add(gkm);
            }

            return gks;
        }

        public GrafischeKaartModel GetGrafischeKaartById(int id)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            GrafischeKaart gk = new GrafischeKaart();

            try
            {
                gk = service.GrafischeKaartGetById(id);
            }
            catch
            {
            }

            GrafischeKaartModel gkm = new GrafischeKaartModel();
            gkm.IdGrafischeKaart = gk.IdGrafischeKaart;
            gkm.Merk = gk.Merk;
            gkm.Type = gk.Type;
            gkm.Driver = gk.Driver;
            gkm.FabrieksNummer = gk.FabrieksNummer;

            return gkm;
        }

        public bool UpdateGrafischeKaart(GrafischeKaartModel gkm)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            GrafischeKaart gk = new GrafischeKaart();
            gk.IdGrafischeKaart = gkm.IdGrafischeKaart;
            gk.Merk = gkm.Merk;
            gk.Type = gkm.Type;
            gk.Driver = gkm.Driver;
            gk.FabrieksNummer = gkm.FabrieksNummer;

            try
            {
                return service.GrafischeKaartUpdate(gk);
            }
            catch
            {
                return false;
            }
        }

        public int InsertGrafischeKaart(GrafischeKaartModel gkm)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            GrafischeKaart gk = new GrafischeKaart();
            gk.IdGrafischeKaart = gkm.IdGrafischeKaart;
            gk.Merk = gkm.Merk;
            gk.Type = gkm.Type;
            gk.Driver = gkm.Driver;
            gk.FabrieksNummer = gkm.FabrieksNummer;

            try
            {
                return service.GrafischeKaartCreate(gk);
            }
            catch
            {
                return -1;
            }
        }

        public bool DeleteGrafischeKaart(GrafischeKaartModel gkm)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            int id = gkm.IdGrafischeKaart;

            try
            {
                return service.GrafischeKaartDelete(id);
            }
            catch
            {
                return false;
            }
        }
    }
}