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
                lv.IdLeverancier = leverancier.IdLeverancier;
                lv.Naam = leverancier.Naam;
                lv.Afkorting = leverancier.Afkorting;
                lv.Straat = leverancier.Straat;
                lv.HuisNummer = leverancier.HuisNummer;
                lv.BusNummer = leverancier.BusNummer;
                lv.Postcode = leverancier.Postcode;
                lv.Telefoon = leverancier.Telefoon;
                lv.Fax = leverancier.Fax;
                lv.Email = leverancier.Email;
                lv.Website = leverancier.Website;
                lv.BtwNummer = leverancier.BtwNummer;
                lv.Iban = leverancier.Iban;
                lv.Bic = leverancier.Bic;
                lv.ToegevoegdOp = leverancier.ToegevoegdOp;
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
            leverancier.Naam = lv.Naam;
            leverancier.Afkorting = lv.Afkorting;
            leverancier.Straat = lv.Straat;
            leverancier.HuisNummer = lv.HuisNummer;
            leverancier.BusNummer = lv.BusNummer;
            leverancier.Postcode = lv.Postcode;
            leverancier.Telefoon = lv.Telefoon;
            leverancier.Fax = lv.Fax;
            leverancier.Email = lv.Email;
            leverancier.Website = lv.Website;
            leverancier.BtwNummer = lv.BtwNummer;
            leverancier.Iban = lv.Iban;
            leverancier.Bic = lv.Bic;
            leverancier.ToegevoegdOp = lv.ToegevoegdOp;

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
            lv.IdLeverancier = leverancier.IdLeverancier;
            lv.Naam = leverancier.Naam;
            lv.Afkorting = leverancier.Afkorting;
            lv.Straat = leverancier.Straat;
            lv.HuisNummer = leverancier.HuisNummer;
            lv.BusNummer = leverancier.BusNummer;
            lv.Postcode = leverancier.Postcode;
            lv.Telefoon = leverancier.Telefoon;
            lv.Fax = leverancier.Fax;
            lv.Email = leverancier.Email;
            lv.Website = leverancier.Website;
            lv.BtwNummer = leverancier.BtwNummer;
            lv.Iban = leverancier.Iban;
            lv.Bic = leverancier.Bic;
            lv.ToegevoegdOp = leverancier.ToegevoegdOp;

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
            leverancier.IdLeverancier = lv.IdLeverancier;
            leverancier.Naam = lv.Naam;
            leverancier.Afkorting = lv.Afkorting;
            leverancier.Straat = lv.Straat;
            leverancier.HuisNummer = lv.HuisNummer;
            leverancier.BusNummer = lv.BusNummer;
            leverancier.Postcode = lv.Postcode;
            leverancier.Telefoon = lv.Telefoon;
            leverancier.Fax = lv.Fax;
            leverancier.Email = lv.Email;
            leverancier.Website = lv.Website;
            leverancier.BtwNummer = lv.BtwNummer;
            leverancier.Iban = lv.Iban;
            leverancier.Bic = lv.Bic;
            leverancier.ToegevoegdOp = lv.ToegevoegdOp;

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

            int id = lv.IdLeverancier;

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