using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class FactuurController : Controller
    {
        // INDEX:

        [HttpGet]
        public ActionResult Index()
        {
            return View(ReadAll());
        }

        private List<FactuurModel> ReadAll()
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {
                List<Factuur> listFactuur = sr.FactuurGetAll().ToList();

                List<FactuurModel> listFacturen = new List<FactuurModel>();

                foreach (Factuur factuur in listFactuur)
                {
                    Leverancier leverancier = sr.LeverancierGetById(factuur.IdLeverancier);

                    LeverancierModel leverancierModel = new LeverancierModel();
                    leverancierModel.IdLeverancier = leverancier.IdLeverancier;
                    leverancierModel.Afkorting = leverancier.Afkorting;
                    leverancierModel.Bic = leverancier.Bic;
                    leverancierModel.BtwNummer = leverancier.BtwNummer;
                    leverancierModel.BusNummer = leverancier.BusNummer;
                    leverancierModel.Email = leverancier.Email;
                    leverancierModel.Fax = leverancier.Fax;
                    leverancierModel.HuisNummer = leverancier.HuisNummer;
                    leverancierModel.Iban = leverancier.Iban;
                    leverancierModel.Naam = leverancier.Naam;
                    leverancierModel.Postcode = leverancier.Postcode;
                    leverancierModel.Straat = leverancier.Straat;
                    leverancierModel.Telefoon = leverancier.Telefoon;
                    leverancierModel.ToegevoegdOp = leverancier.ToegevoegdOp;
                    leverancierModel.Website = leverancier.Website;

                    FactuurModel factuurModel = new FactuurModel();
                    factuurModel.IdFactuur = factuur.IdFactuur;
                    factuurModel.Boekjaar = factuur.Boekjaar;
                    factuurModel.CvoVolgNummer = factuur.CvoVolgNummer;
                    factuurModel.FactuurNummer = factuur.FactuurNummer;
                    factuurModel.FactuurDatum = factuur.FactuurDatum;
                    factuurModel.FactuurStatusGetekend = factuur.FactuurStatusGetekend;
                    factuurModel.VerwerkingsDatum = factuur.VerwerkingsDatum;
                    factuurModel.Leverancier = leverancierModel;
                    factuurModel.Prijs = factuur.Prijs;
                    factuurModel.Garantie = factuur.Garantie;
                    factuurModel.Omschrijving = factuur.Omschrijving;
                    factuurModel.Opmerking = factuur.Opmerking;
                    factuurModel.Afschrijfperiode = factuur.Afschrijfperiode;
                    factuurModel.OleDoc = factuur.OleDoc;
                    factuurModel.OleDocPath = factuur.OleDocPath;
                    factuurModel.OleDocFileName = factuur.OleDocFileName;
                    factuurModel.DatumInsert = factuur.DatumInsert;
                    factuurModel.UserInsert = factuur.UserInsert;
                    factuurModel.DatumModified = factuur.DatumModified;
                    factuurModel.UserModified = factuur.UserModified;
                    listFacturen.Add(factuurModel);
                }
                return listFacturen;
            }
        }


        // INSERT:

        [HttpGet]
        public ActionResult Insert()
        {
            return View(new FactuurModel());
        }

        [HttpPost]
        public ActionResult insertFactuur(FactuurModel factuurModel)
        {
            if (InsertFactuur(factuurModel) >= 0)
            {
                return View("Index", ReadAll());
            }
            else
            {
                return View("Insert");
            }
        }

        public int InsertFactuur(FactuurModel factuurModel)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                Factuur factuur = new Factuur();
                factuur.Boekjaar = factuurModel.Boekjaar;
                factuur.CvoVolgNummer = factuurModel.CvoVolgNummer;
                factuur.FactuurNummer = factuurModel.FactuurNummer;
                factuur.FactuurDatum = factuurModel.FactuurDatum;
                factuur.FactuurStatusGetekend = factuurModel.FactuurStatusGetekend;
                factuur.VerwerkingsDatum = factuurModel.VerwerkingsDatum;
                factuur.IdLeverancier = Convert.ToInt32(factuurModel.Leverancier.IdLeverancier);
                factuur.Prijs = factuurModel.Prijs;
                factuur.Garantie = factuurModel.Garantie;
                factuur.Omschrijving = factuurModel.Omschrijving;
                factuur.Opmerking = factuurModel.Opmerking;
                factuur.Afschrijfperiode = factuurModel.Afschrijfperiode;
                factuur.OleDoc = factuurModel.OleDoc;
                factuur.OleDocPath = factuurModel.OleDocPath;
                factuur.OleDocFileName = factuurModel.OleDocFileName;
                factuur.DatumInsert = factuurModel.DatumInsert;
                factuur.UserInsert = factuurModel.UserInsert;
                factuur.DatumModified = factuurModel.DatumModified;
                factuur.UserModified = factuurModel.UserModified;

                try
                {
                    return sr.FactuurCreate(factuur);
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
            return View(GetFactuurById((int)id));
        }

        public FactuurModel GetFactuurById(int id)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                Factuur factuur = new Factuur();

                try
                {
                    factuur = sr.FactuurGetById(id);
                }
                catch (Exception e)
                {

                }

                Leverancier leverancier = sr.LeverancierGetById(factuur.IdLeverancier);

                LeverancierModel leverancierModel = new LeverancierModel();
                leverancierModel.IdLeverancier = leverancier.IdLeverancier;
                leverancierModel.Afkorting = leverancier.Afkorting;
                leverancierModel.Bic = leverancier.Bic;
                leverancierModel.BtwNummer = leverancier.BtwNummer;
                leverancierModel.BusNummer = leverancier.BusNummer;
                leverancierModel.Email = leverancier.Email;
                leverancierModel.Fax = leverancier.Fax;
                leverancierModel.HuisNummer = leverancier.HuisNummer;
                leverancierModel.Iban = leverancier.Iban;
                leverancierModel.Naam = leverancier.Naam;
                leverancierModel.Postcode = leverancier.Postcode;
                leverancierModel.Straat = leverancier.Straat;
                leverancierModel.Telefoon = leverancier.Telefoon;
                leverancierModel.ToegevoegdOp = leverancier.ToegevoegdOp;
                leverancierModel.Website = leverancier.Website;

                FactuurModel factuurModel = new FactuurModel();
                factuurModel.IdFactuur = factuur.IdFactuur;
                factuurModel.Boekjaar = factuur.Boekjaar;
                factuurModel.CvoVolgNummer = factuur.CvoVolgNummer;
                factuurModel.FactuurNummer = factuur.FactuurNummer;
                factuurModel.FactuurDatum = factuur.FactuurDatum;
                factuurModel.FactuurStatusGetekend = factuur.FactuurStatusGetekend;
                factuurModel.VerwerkingsDatum = factuur.VerwerkingsDatum;
                factuurModel.Leverancier = leverancierModel;
                factuurModel.Prijs = factuur.Prijs;
                factuurModel.Garantie = factuur.Garantie;
                factuurModel.Omschrijving = factuur.Omschrijving;
                factuurModel.Opmerking = factuur.Opmerking;
                factuurModel.Afschrijfperiode = factuur.Afschrijfperiode;
                factuurModel.OleDoc = factuur.OleDoc;
                factuurModel.OleDocPath = factuur.OleDocPath;
                factuurModel.OleDocFileName = factuur.OleDocFileName;
                factuurModel.DatumInsert = factuur.DatumInsert;
                factuurModel.UserInsert = factuur.UserInsert;
                factuurModel.DatumModified = factuur.DatumModified;
                factuurModel.UserModified = factuur.UserModified;

                return factuurModel;
            }
        }

        // UPDATE:

        [HttpGet]
        public ActionResult Update(int? id)
        {
            return View(GetFactuurById((int)id));
        }

        [HttpPost]
        public ActionResult updateFactuur(FactuurModel factuurModel)
        {
            if (UpdateFactuur(factuurModel))
            {
                return View("Index", ReadAll());
            }
            else
            {
                return View("update");
            }
        }

        public bool UpdateFactuur(FactuurModel factuurModel)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                Factuur factuur = new Factuur();
                factuur.IdFactuur = factuurModel.IdFactuur;
                factuur.Boekjaar = factuurModel.Boekjaar;
                factuur.CvoVolgNummer = factuurModel.CvoVolgNummer;
                factuur.FactuurNummer = factuurModel.FactuurNummer;
                factuur.FactuurDatum = factuurModel.FactuurDatum;
                factuur.FactuurStatusGetekend = factuurModel.FactuurStatusGetekend;
                factuur.VerwerkingsDatum = factuurModel.VerwerkingsDatum;
                factuur.IdLeverancier = Convert.ToInt32(factuurModel.Leverancier.IdLeverancier);
                factuur.Prijs = factuurModel.Prijs;
                factuur.Garantie = factuurModel.Garantie;
                factuur.Omschrijving = factuurModel.Omschrijving;
                factuur.Opmerking = factuurModel.Opmerking;
                factuur.Afschrijfperiode = factuurModel.Afschrijfperiode;
                factuur.OleDoc = factuurModel.OleDoc;
                factuur.OleDocPath = factuurModel.OleDocPath;
                factuur.OleDocFileName = factuurModel.OleDocFileName;
                factuur.DatumInsert = factuurModel.DatumInsert;
                factuur.UserInsert = factuurModel.UserInsert;
                factuur.DatumModified = factuurModel.DatumModified;
                factuur.UserModified = factuurModel.UserModified;

                try
                {
                    return sr.FactuurUpdate(factuur);
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
            return View(GetFactuurById((int)id));
        }

        [HttpPost]
        public ActionResult deleteFactuur(FactuurModel factuurModel)
        {
            if (DeleteFactuur(factuurModel))
            {
                return View("Index", ReadAll());
            }
            else
            {
                return View("Delete");
            }
        }

        public bool DeleteFactuur(FactuurModel factuurModel)
        {
            using (CvoInventarisServiceClient sr = new CvoInventarisServiceClient())
            {

                int id = factuurModel.IdFactuur;

                try
                {
                    return sr.FactuurDelete(id);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}