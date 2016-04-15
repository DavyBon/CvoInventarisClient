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

            Harddisk[] arrHarddisks = sr.HarddiskGetAll();

            List<HarddiskModel> listHarddisks = new List<HarddiskModel>();

            foreach (var item in arrHarddisks)
            {
                HarddiskModel hd = new HarddiskModel();
                hd.idHarddisk = item.idHarddisk;
                hd.merk = item.merk;
                hd.grootte = item.grootte;
                hd.fabrieksNummer = item.fabrieksNummer;
                listHarddisks.Add(hd);
            }

            return listHarddisks;
        }


        // INSERT:

        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult insertHarddisk(Harddisk hd)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            sr.HarddiskCreate(hd);

            return View("Index", ReadAll());
        }


        // UPDATE:

        [HttpGet]
        public ActionResult Update(int? id)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Harddisk harddisk = sr.HarddiskGetById((int)id);

            HarddiskModel harddiskModel = new HarddiskModel();

            harddiskModel.idHarddisk = (int)id;
            harddiskModel.merk = harddisk.merk;
            harddiskModel.grootte = harddisk.grootte;
            harddiskModel.fabrieksNummer = harddisk.fabrieksNummer;

            return View(harddiskModel);
        }

        [HttpPost]
        public ActionResult updateHarddisk(Harddisk hd)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            sr.HarddiskUpdate(hd);

            return View("Index", ReadAll());
        }


        // DELETE:

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Harddisk harddisk = sr.HarddiskGetById((int)id);

            HarddiskModel harddiskModel = new HarddiskModel();

            harddiskModel.idHarddisk = (int)id;
            harddiskModel.merk = harddisk.merk;
            harddiskModel.grootte = harddisk.grootte;
            harddiskModel.fabrieksNummer = harddisk.fabrieksNummer;

            return View(harddiskModel);
        }

        [HttpPost]
        public ActionResult deleteHarddisk(Harddisk hd)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            sr.HarddiskDelete(hd.idHarddisk);

            return View("Index", ReadAll());
        }
    }
}