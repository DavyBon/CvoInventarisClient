using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class HarddiskController : Controller
    {

        // INDEX:
        
        [HttpGet]
        public ActionResult Index()
        {
            return View(ReadAll());
        }

        private List<HarddiskModel> ReadAll()
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {
                List<Harddisk> listHarddisk = sr.HarddiskGetAll().ToList();

                List<HarddiskModel> listHarddisks = new List<HarddiskModel>();

                foreach (Harddisk harddisk in listHarddisk)
                {
                    HarddiskModel harddiskModel = new HarddiskModel();
                    harddiskModel.IdHarddisk = harddisk.IdHarddisk;
                    harddiskModel.Merk = harddisk.Merk;
                    harddiskModel.Grootte = harddisk.Grootte;
                    harddiskModel.FabrieksNummer = harddisk.FabrieksNummer;
                    listHarddisks.Add(harddiskModel);
                }
                return listHarddisks;
            }
        }


        // INSERT:
        
        [HttpGet]
        public ActionResult Insert()
        {
            return View(new HarddiskModel());
        }
        
        [HttpPost]
        public ActionResult Insert(HarddiskModel harddiskModel)
        {
            if(insertHarddisk(harddiskModel) >= 0)
            {
                return View("Index", ReadAll());
            }
            else
            {
                return View("Insert");
            }
        }

        public int insertHarddisk(HarddiskModel harddiskModel)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                Harddisk harddisk = new Harddisk();
                harddisk.Merk = harddiskModel.Merk;
                harddisk.Grootte = harddiskModel.Grootte;
                harddisk.FabrieksNummer = harddiskModel.FabrieksNummer;

                try
                {
                    return sr.HarddiskCreate(harddisk);
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
            return View(GetHarddiskById((int)id));
        }

        public HarddiskModel GetHarddiskById(int id)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                Harddisk harddisk = new Harddisk();

                try
                {
                    harddisk = sr.HarddiskGetById(id);
                }
                catch (Exception e)
                {

                }

                HarddiskModel harddiskModel = new HarddiskModel();
                harddiskModel.IdHarddisk = harddisk.IdHarddisk;
                harddiskModel.Merk = harddisk.Merk;
                harddiskModel.Grootte = harddisk.Grootte;
                harddiskModel.FabrieksNummer = harddisk.FabrieksNummer;

                return harddiskModel;
            }
        }


        // UPDATE:

        [HttpGet]
        public ActionResult Update(int? id)
        {
            return View(GetHarddiskById((int)id));
        }

        
        [HttpPost]
        public ActionResult Update(HarddiskModel harddiskModel)
        {
            if(UpdateHarddisk(harddiskModel))
            {
                return View("Index", ReadAll());
            }
            else
            {
                return View("Update");
            }
        }

        public bool UpdateHarddisk(HarddiskModel harddiskModel)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                Harddisk harddisk = new Harddisk();
                harddisk.IdHarddisk = harddiskModel.IdHarddisk;
                harddisk.Merk = harddiskModel.Merk;
                harddisk.Grootte = harddiskModel.Grootte;
                harddisk.FabrieksNummer = harddiskModel.FabrieksNummer;

                try
                {
                    return sr.HarddiskUpdate(harddisk);
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
            return View(GetHarddiskById((int)id));
        }
        
        [HttpPost]
        public ActionResult deleteHarddisk(HarddiskModel harddiskModel)
        {
            if(DeleteHarddisk(harddiskModel))
            {
                return View("Index", ReadAll());
            }
            else
            {
                return View("Delete");
            }
        }
        
        public bool DeleteHarddisk(HarddiskModel harddiskModel)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                int id = harddiskModel.IdHarddisk;

                try
                {
                    return sr.HarddiskDelete(id);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}