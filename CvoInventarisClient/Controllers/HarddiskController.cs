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
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Harddisk[] arrHarddisks = new Harddisk[] { };

            try
            {
                arrHarddisks = sr.HarddiskGetAll();
            }
            catch (Exception e)
            {

            }
           
            List<HarddiskModel> listHarddisks = new List<HarddiskModel>();

            foreach (Harddisk harddisk in arrHarddisks)
            {
                HarddiskModel hd = new HarddiskModel();
                hd.idHarddisk = harddisk.idHarddisk;
                hd.merk = harddisk.merk;
                hd.grootte = harddisk.grootte;
                hd.fabrieksNummer = harddisk.fabrieksNummer;
                listHarddisks.Add(hd);
            }

            return listHarddisks;
        }


        // INSERT:
        
        [HttpGet]
        public ActionResult Insert()
        {
            return View(new HarddiskModel());
        }
        
        [HttpPost]
        public ActionResult Insert(HarddiskModel hd)
        {
            if(insertHarddisk(hd) >= 0)
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
        
        public int insertHarddisk(HarddiskModel hd)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Harddisk harddisk = new Harddisk();
            harddisk.merk = hd.merk;
            harddisk.grootte = hd.grootte;
            harddisk.fabrieksNummer = hd.fabrieksNummer;

            try
            {
                return sr.HarddiskCreate(harddisk);
            }
            catch(Exception)
            {
                return -1;
            }
        }


        // DETAILS:
        
        public ActionResult Details(int? id)
        {
            return View(GetHarddiskById((int)id));
        }
        
        public HarddiskModel GetHarddiskById(int id)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Harddisk harddisk = new Harddisk();

            try
            {
                harddisk = sr.HarddiskGetById(id);
            }
            catch (Exception e)
            {

            }

            HarddiskModel hd = new HarddiskModel();
            hd.idHarddisk = harddisk.idHarddisk;
            hd.merk = harddisk.merk;
            hd.grootte = harddisk.grootte;
            hd.fabrieksNummer = harddisk.fabrieksNummer;

            return hd;
        }


        // UPDATE:

        [HttpGet]
        public ActionResult Update(int? id)
        {
            return View(GetHarddiskById((int)id));
        }

        
        [HttpPost]
        public ActionResult Update(HarddiskModel hd)
        {
            if(UpdateHarddisk(hd))
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
        
        public bool UpdateHarddisk(HarddiskModel hd)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Harddisk harddisk = new Harddisk();
            harddisk.idHarddisk = hd.idHarddisk;
            harddisk.merk = hd.merk;
            harddisk.grootte = hd.grootte;
            harddisk.fabrieksNummer = hd.fabrieksNummer;

            try
            {
                return sr.HarddiskUpdate(harddisk);
            }
            catch(Exception)
            {
                return false;
            }
        }


        // DELETE:
        
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return View(GetHarddiskById((int)id));
        }
        
        [HttpPost]
        public ActionResult deleteHarddisk(HarddiskModel hd)
        {
            if(DeleteHarddisk(hd))
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
        
        public bool DeleteHarddisk(HarddiskModel hd)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            int id = hd.idHarddisk;

            try
            {
                return sr.HarddiskDelete(id);
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}