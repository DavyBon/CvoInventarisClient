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
        public ActionResult Index()
        {
            return View(GetVerzekering());
        }

        public ActionResult Edit(int id)
        {
            return View(VerzekeringGetById(id));
        }

        [HttpPost]
        public ActionResult Edit(VerzekeringModel vz)
        {
            if (UpdateVerzekering(vz))
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
            return View(new VerzekeringModel());
        }

        [HttpPost]
        public ActionResult Create(VerzekeringModel vz)
        {
            if (InsertVerzekering(vz) >= 0)
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
            return View(VerzekeringGetById(id));
        }

        public ActionResult Delete(int id)
        {
            return View(VerzekeringGetById(id));
        }

        [HttpPost]
        public ActionResult Delete(VerzekeringModel vz)
        {
            if (DeleteVerzekering(vz))
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

        public List<VerzekeringModel> GetVerzekering()
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            Verzekering[] v = new Verzekering[] { };

            try
            {
                v = service.VerzekeringGetAll();
            }
            catch (Exception e)
            {

            }

            List<VerzekeringModel> verzekeringen = new List<VerzekeringModel>();

            foreach (Verzekering verzekering in v)
            {
                VerzekeringModel vz = new VerzekeringModel();
                vz.IdVerzekering = verzekering.Id;
                vz.Omschrijving = verzekering.Omschrijving;
                verzekeringen.Add(vz);
            }

            return verzekeringen;
        }

        public VerzekeringModel VerzekeringGetById(int id)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            Verzekering verzekering = new Verzekering();

            try
            {
                verzekering = service.VerzekeringGetById(id);
            }
            catch (Exception e)
            {

            }

            VerzekeringModel vz = new VerzekeringModel();
            vz.IdVerzekering = verzekering.Id;
            vz.Omschrijving = verzekering.Omschrijving;
            return vz;
        }

        public bool UpdateVerzekering(VerzekeringModel verzekering)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            Verzekering vz = new Verzekering();
            vz.Id = verzekering.IdVerzekering;
            vz.Omschrijving = verzekering.Omschrijving;

            try
            {
                return service.VerzekeringUpdate(vz);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int InsertVerzekering(VerzekeringModel verzekering)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            Verzekering vz = new Verzekering();
            vz.Omschrijving = verzekering.Omschrijving;

            try
            {
                return service.VerzekeringCreate(vz);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool DeleteVerzekering(VerzekeringModel verzekering)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            int id = verzekering.IdVerzekering;

            try
            {
                return service.VerzekeringDelete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
}
}