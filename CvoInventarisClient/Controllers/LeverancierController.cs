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
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                List<Leverancier> listLeverancier = sr.LeverancierGetAll().ToList();

                List<LeverancierModel> listLeveranciers = new List<LeverancierModel>();

                foreach (Leverancier leverancier in listLeverancier)
                {
                    LeverancierModel leverancierModel = new LeverancierModel();
                    leverancierModel.IdLeverancier = leverancier.IdLeverancier;
                    leverancierModel.Naam = leverancier.Naam;
                    leverancierModel.Afkorting = leverancier.Afkorting;
                    leverancierModel.Straat = leverancier.Straat;
                    leverancierModel.HuisNummer = leverancier.HuisNummer;
                    leverancierModel.BusNummer = leverancier.BusNummer;
                    leverancierModel.Postcode = leverancier.Postcode;
                    leverancierModel.Telefoon = leverancier.Telefoon;
                    leverancierModel.Fax = leverancier.Fax;
                    leverancierModel.Email = leverancier.Email;
                    leverancierModel.Website = leverancier.Website;
                    leverancierModel.BtwNummer = leverancier.BtwNummer;
                    leverancierModel.Iban = leverancier.Iban;
                    leverancierModel.Bic = leverancier.Bic;
                    leverancierModel.ToegevoegdOp = leverancier.ToegevoegdOp;
                    listLeveranciers.Add(leverancierModel);
                }
                return listLeveranciers;
            }
        }


        // INSERT:

        [HttpGet]
        public ActionResult Insert()
        {
            return View(new LeverancierModel());
        }

        [HttpPost]
        public ActionResult insertLeverancier(LeverancierModel leverancierModel)
        {
            if (InsertLeverancier(leverancierModel) >= 0)
            {
                return View("Index", ReadAll());
            }
            else
            {
                return View("Insert");
            }
        }

        public int InsertLeverancier(LeverancierModel leverancierModel)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                Leverancier leverancier = new Leverancier();
                leverancier.Naam = leverancierModel.Naam;
                leverancier.Afkorting = leverancierModel.Afkorting;
                leverancier.Straat = leverancierModel.Straat;
                leverancier.HuisNummer = leverancierModel.HuisNummer;
                leverancier.BusNummer = leverancierModel.BusNummer;
                leverancier.Postcode = leverancierModel.Postcode;
                leverancier.Telefoon = leverancierModel.Telefoon;
                leverancier.Fax = leverancierModel.Fax;
                leverancier.Email = leverancierModel.Email;
                leverancier.Website = leverancierModel.Website;
                leverancier.BtwNummer = leverancierModel.BtwNummer;
                leverancier.Iban = leverancierModel.Iban;
                leverancier.Bic = leverancierModel.Bic;
                leverancier.ToegevoegdOp = leverancierModel.ToegevoegdOp;

                try
                {
                    return sr.LeverancierCreate(leverancier);
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
            return View(GetLeverancierById((int)id));
        }

        public LeverancierModel GetLeverancierById(int id)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                Leverancier leverancier = new Leverancier();

                try
                {
                    leverancier = sr.LeverancierGetById(id);
                }
                catch (Exception e)
                {

                }

                LeverancierModel leverancierModel = new LeverancierModel();
                leverancierModel.IdLeverancier = leverancier.IdLeverancier;
                leverancierModel.Naam = leverancier.Naam;
                leverancierModel.Afkorting = leverancier.Afkorting;
                leverancierModel.Straat = leverancier.Straat;
                leverancierModel.HuisNummer = leverancier.HuisNummer;
                leverancierModel.BusNummer = leverancier.BusNummer;
                leverancierModel.Postcode = leverancier.Postcode;
                leverancierModel.Telefoon = leverancier.Telefoon;
                leverancierModel.Fax = leverancier.Fax;
                leverancierModel.Email = leverancier.Email;
                leverancierModel.Website = leverancier.Website;
                leverancierModel.BtwNummer = leverancier.BtwNummer;
                leverancierModel.Iban = leverancier.Iban;
                leverancierModel.Bic = leverancier.Bic;
                leverancierModel.ToegevoegdOp = leverancier.ToegevoegdOp;

                return leverancierModel;
            }
        }


        // UPDATE:

        [HttpGet]
        public ActionResult Update(int? id)
        {
            return View(GetLeverancierById((int)id));
        }

        [HttpPost]
        public ActionResult updateLeverancier(LeverancierModel leverancierModel)
        {
            if (UpdateLeverancier(leverancierModel))
            {
                return View("Index", ReadAll());
            }
            else
            {
                return View("Update");
            }
        }

        public bool UpdateLeverancier(LeverancierModel leverancierModel)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                Leverancier leverancier = new Leverancier();
                leverancier.IdLeverancier = leverancierModel.IdLeverancier;
                leverancier.Naam = leverancierModel.Naam;
                leverancier.Afkorting = leverancierModel.Afkorting;
                leverancier.Straat = leverancierModel.Straat;
                leverancier.HuisNummer = leverancierModel.HuisNummer;
                leverancier.BusNummer = leverancierModel.BusNummer;
                leverancier.Postcode = leverancierModel.Postcode;
                leverancier.Telefoon = leverancierModel.Telefoon;
                leverancier.Fax = leverancierModel.Fax;
                leverancier.Email = leverancierModel.Email;
                leverancier.Website = leverancierModel.Website;
                leverancier.BtwNummer = leverancierModel.BtwNummer;
                leverancier.Iban = leverancierModel.Iban;
                leverancier.Bic = leverancierModel.Bic;
                leverancier.ToegevoegdOp = leverancierModel.ToegevoegdOp;

                try
                {
                    return sr.LeverancierUpdate(leverancier);
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
            return View(GetLeverancierById((int)id));
        }

        [HttpPost]
        public ActionResult deleteLeverancier(LeverancierModel leverancierModel)
        {
            if (DeleteLeverancier(leverancierModel))
            {
                return View("Index", ReadAll());
            }
            else
            {
                return View("Delete");
            }
        }

        public bool DeleteLeverancier(LeverancierModel leverancierModel)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                int id = leverancierModel.IdLeverancier;

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
}