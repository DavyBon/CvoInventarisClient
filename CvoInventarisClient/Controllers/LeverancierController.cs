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

            Leverancier[] arrLeveranciers = new Leverancier[] { };

            try
            {
                arrLeveranciers = sr.LeverancierGetAll();
            }
            catch(Exception e)
            {

            }

            List<LeverancierModel> listLeveranciers = new List<LeverancierModel>();

            foreach (Leverancier leverancier in arrLeveranciers)
            {
                LeverancierModel lv = new LeverancierModel();
                lv.idLeverancier = leverancier.idLeverancier;
                lv.naam = leverancier.naam;
                lv.afkorting = leverancier.afkorting;
                lv.straat = leverancier.straat;
                lv.huisNummer = leverancier.huisNummer;
                lv.busNummer = leverancier.busNummer;
                lv.postcode = leverancier.postcode;
                lv.telefoon = leverancier.telefoon;
                lv.fax = leverancier.fax;
                lv.email = leverancier.email;
                lv.website = leverancier.website;
                lv.btwNummer = leverancier.btwNummer;
                lv.iban = leverancier.iban;
                lv.bic = leverancier.bic;
                lv.toegevoegdOp = leverancier.toegevoegdOp;
                listLeveranciers.Add(lv);
            }

            return listLeveranciers;
        }


        // INSERT:

        [HttpGet]
        public ActionResult Insert()
        {
            return View(new LeverancierModel());
        }

        [HttpPost]
        public ActionResult insertLeverancier(LeverancierModel lv)
        {
            if (InsertLeverancier(lv) >= 0)
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

        public int InsertLeverancier(LeverancierModel lv)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Leverancier leverancier = new Leverancier();
            leverancier.naam = lv.naam;
            leverancier.afkorting = lv.afkorting;
            leverancier.straat = lv.straat;
            leverancier.huisNummer = lv.huisNummer;
            leverancier.busNummer = lv.busNummer;
            leverancier.postcode = lv.postcode;
            leverancier.telefoon = lv.telefoon;
            leverancier.fax = lv.fax;
            leverancier.email = lv.email;
            leverancier.website = lv.website;
            leverancier.btwNummer = lv.btwNummer;
            leverancier.iban = lv.iban;
            leverancier.bic = lv.bic;
            leverancier.toegevoegdOp = lv.toegevoegdOp;

            try
            {
                return sr.LeverancierCreate(leverancier);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        // DETAILS:

        public ActionResult Details(int? id)
        {
            return View(GetLeverancierById((int)id));
        }

        public LeverancierModel GetLeverancierById(int id)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Leverancier leverancier = new Leverancier();

            try
            {
                leverancier = sr.LeverancierGetById(id);
            }
            catch (Exception e)
            {

            }

            LeverancierModel lv = new LeverancierModel();
            lv.idLeverancier = leverancier.idLeverancier;
            lv.naam = leverancier.naam;
            lv.afkorting = leverancier.afkorting;
            lv.straat = leverancier.straat;
            lv.huisNummer = leverancier.huisNummer;
            lv.busNummer = leverancier.busNummer;
            lv.postcode = leverancier.postcode;
            lv.telefoon = leverancier.telefoon;
            lv.fax = leverancier.fax;
            lv.email = leverancier.email;
            lv.website = leverancier.website;
            lv.btwNummer = leverancier.btwNummer;
            lv.iban = leverancier.iban;
            lv.bic = leverancier.bic;
            lv.toegevoegdOp = leverancier.toegevoegdOp;

            return lv;
        }


        // UPDATE:

        [HttpGet]
        public ActionResult Update(int? id)
        {
            return View(GetLeverancierById((int)id));
        }

        [HttpPost]
        public ActionResult updateLeverancier(LeverancierModel lv)
        {
            if (UpdateLeverancier(lv))
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

        public bool UpdateLeverancier(LeverancierModel lv)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Leverancier leverancier = new Leverancier();
            leverancier.idLeverancier = lv.idLeverancier;
            leverancier.naam = lv.naam;
            leverancier.afkorting = lv.afkorting;
            leverancier.straat = lv.straat;
            leverancier.huisNummer = lv.huisNummer;
            leverancier.busNummer = lv.busNummer;
            leverancier.postcode = lv.postcode;
            leverancier.telefoon = lv.telefoon;
            leverancier.fax = lv.fax;
            leverancier.email = lv.email;
            leverancier.website = lv.website;
            leverancier.btwNummer = lv.btwNummer;
            leverancier.iban = lv.iban;
            leverancier.bic = lv.bic;
            leverancier.toegevoegdOp = lv.toegevoegdOp;

            try
            {
                return sr.LeverancierUpdate(leverancier);
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
            return View(GetLeverancierById((int)id));
        }

        [HttpPost]
        public ActionResult deleteLeverancier(LeverancierModel lv)
        {
            if (DeleteLeverancier(lv))
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

        public bool DeleteLeverancier(LeverancierModel lv)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            int id = lv.idLeverancier;

            try
            {
                return sr.LeverancierDelete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}