using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class LeverancierController : Controller
    {
        // INDEX:

        [HttpGet]
        public ActionResult Index()
        {
            return View(ReadAll());
        }

        private List<LeverancierModel> ReadAll()
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Leverancier[] arrLeveranciers = sr.LeverancierGetAll();

            List<LeverancierModel> listLeveranciers = new List<LeverancierModel>();

            foreach (var item in arrLeveranciers)
            {
                LeverancierModel lv = new LeverancierModel();
                lv.idLeverancier = item.idLeverancier;
                lv.naam = item.naam;
                lv.afkorting = item.afkorting;
                lv.straat = item.straat;
                lv.huisNummer = item.huisNummer;
                lv.busNummer = item.busNummer;
                lv.postcode = item.postcode;
                lv.telefoon = item.telefoon;
                lv.fax = item.fax;
                lv.email = item.email;
                lv.website = item.website;
                lv.btwNummer = item.btwNummer;
                lv.iban = item.iban;
                lv.bic = item.bic;
                lv.toegevoegdOp = item.toegevoegdOp;
                listLeveranciers.Add(lv);
            }

            return listLeveranciers;
        }


        // INSERT:

        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult insertLeverancier(Leverancier lv)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            sr.LeverancierCreate(lv);

            return View("Index", ReadAll());
        }


        // UPDATE:

        [HttpGet]
        public ActionResult Update(int? id)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Leverancier leverancier = sr.LeverancierGetById((int)id);

            LeverancierModel leverancierModel = new LeverancierModel();

            leverancierModel.idLeverancier = (int)id;
            leverancierModel.naam = leverancier.naam;
            leverancierModel.afkorting = leverancier.afkorting;
            leverancierModel.straat = leverancier.straat;
            leverancierModel.huisNummer = leverancier.huisNummer;
            leverancierModel.busNummer = leverancier.busNummer;
            leverancierModel.postcode = leverancier.postcode;
            leverancierModel.telefoon = leverancier.telefoon;
            leverancierModel.fax = leverancier.fax;
            leverancierModel.email = leverancier.email;
            leverancierModel.website = leverancier.website;
            leverancierModel.btwNummer = leverancier.btwNummer;
            leverancierModel.iban = leverancier.iban;
            leverancierModel.bic = leverancier.bic;
            leverancierModel.toegevoegdOp = leverancier.toegevoegdOp;

            return View(leverancierModel);
        }

        [HttpPost]
        public ActionResult updateLeverancier(Leverancier lv)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            sr.LeverancierUpdate(lv);

            return View("Index", ReadAll());
        }


        // DELETE:

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Leverancier leverancier = sr.LeverancierGetById((int)id);

            LeverancierModel leverancierModel = new LeverancierModel();

            leverancierModel.idLeverancier = (int)id;
            leverancierModel.naam = leverancier.naam;
            leverancierModel.afkorting = leverancier.afkorting;
            leverancierModel.straat = leverancier.straat;
            leverancierModel.huisNummer = leverancier.huisNummer;
            leverancierModel.busNummer = leverancier.busNummer;
            leverancierModel.postcode = leverancier.postcode;
            leverancierModel.telefoon = leverancier.telefoon;
            leverancierModel.fax = leverancier.fax;
            leverancierModel.email = leverancier.email;
            leverancierModel.website = leverancier.website;
            leverancierModel.btwNummer = leverancier.btwNummer;
            leverancierModel.iban = leverancier.iban;
            leverancierModel.bic = leverancier.bic;
            leverancierModel.toegevoegdOp = leverancier.toegevoegdOp;

            return View(leverancierModel);
        }

        [HttpPost]
        public ActionResult deleteLeverancier(Leverancier lv)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            sr.LeverancierDelete(lv.idLeverancier);

            return View("Index", ReadAll());
        }
    }
}