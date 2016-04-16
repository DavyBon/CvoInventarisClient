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
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Factuur[] arrFacturen = new Factuur[] { };

            try
            {
                arrFacturen = sr.FactuurGetAll();
            }
            catch(Exception e)
            {

            }                

            List<FactuurModel> listFacturen = new List<FactuurModel>();

            foreach (Factuur factuur in arrFacturen)
            {
                FactuurModel fctr = new FactuurModel();
                fctr.idFactuur = factuur.idFactuur;
                fctr.Boekjaar = factuur.Boekjaar;
                fctr.CvoVolgNummer = factuur.CvoVolgNummer;
                fctr.FactuurNummer = factuur.FactuurNummer;
                fctr.FactuurDatum = factuur.FactuurDatum;
                fctr.FactuurStatusGetekend = factuur.FactuurStatusGetekend;
                fctr.VerwerkingsDatum = factuur.VerwerkingsDatum;
                fctr.idLeverancier = factuur.idLeverancier;
                fctr.Prijs = factuur.Prijs;
                fctr.Garantie = factuur.Garantie;
                fctr.Omschrijving = factuur.Omschrijving;
                fctr.Opmerking = factuur.Opmerking;
                fctr.Afschrijfperiode = factuur.Afschrijfperiode;
                fctr.OleDoc = factuur.OleDoc;
                fctr.OleDocPath = factuur.OleDocPath;
                fctr.OleDocFileName = factuur.OleDocFileName;
                fctr.DatumInsert = factuur.DatumInsert;
                fctr.UserInsert = factuur.UserInsert;
                fctr.DatumModified = factuur.DatumModified;
                fctr.UserModified = factuur.UserModified;
                listFacturen.Add(fctr);
            }

            return listFacturen;
        }


        // INSERT:

        [HttpGet]
        public ActionResult Insert()
        {
            return View(new FactuurModel());
        }

        [HttpPost]
        public ActionResult insertFactuur(FactuurModel fctr)
        {
            if (InsertFactuur(fctr) >= 0)
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

        public int InsertFactuur(FactuurModel fctr)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Factuur factuur = new Factuur();
            factuur.Boekjaar = fctr.Boekjaar;
            factuur.CvoVolgNummer = fctr.CvoVolgNummer;
            factuur.FactuurNummer = fctr.FactuurNummer;
            factuur.FactuurDatum = fctr.FactuurDatum;
            factuur.FactuurStatusGetekend = fctr.FactuurStatusGetekend;
            factuur.VerwerkingsDatum = fctr.VerwerkingsDatum;
            factuur.idLeverancier = fctr.idLeverancier;
            factuur.Prijs = fctr.Prijs;
            factuur.Garantie = fctr.Garantie;
            factuur.Omschrijving = fctr.Omschrijving;
            factuur.Opmerking = fctr.Opmerking;
            factuur.Afschrijfperiode = fctr.Afschrijfperiode;
            factuur.OleDoc = fctr.OleDoc;
            factuur.OleDocPath = fctr.OleDocPath;
            factuur.OleDocFileName = fctr.OleDocFileName;
            factuur.DatumInsert = fctr.DatumInsert;
            factuur.UserInsert = fctr.UserInsert;
            factuur.DatumModified = fctr.DatumModified;
            factuur.UserModified = fctr.UserModified;

            try
            {
                return sr.FactuurCreate(factuur);
            }
            catch(Exception)
            {
                return -1;
            }

        }

        // DETAILS:

        public ActionResult Details(int? id)
        {
            return View(GetFactuurById((int)id));
        }

        public FactuurModel GetFactuurById(int id)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Factuur factuur = new Factuur();

            try
            {
                factuur = sr.FactuurGetById(id);
            }
            catch(Exception e)
            {

            }

            FactuurModel fctr = new FactuurModel();
            fctr.idFactuur = factuur.idFactuur;
            fctr.Boekjaar = factuur.Boekjaar;
            fctr.CvoVolgNummer = factuur.CvoVolgNummer;
            fctr.FactuurNummer = factuur.FactuurNummer;
            fctr.FactuurDatum = factuur.FactuurDatum;
            fctr.FactuurStatusGetekend = factuur.FactuurStatusGetekend;
            fctr.VerwerkingsDatum = factuur.VerwerkingsDatum;
            fctr.idLeverancier = factuur.idLeverancier;
            fctr.Prijs = factuur.Prijs;
            fctr.Garantie = factuur.Garantie;
            fctr.Omschrijving = factuur.Omschrijving;
            fctr.Opmerking = factuur.Opmerking;
            fctr.Afschrijfperiode = factuur.Afschrijfperiode;
            fctr.OleDoc = factuur.OleDoc;
            fctr.OleDocPath = factuur.OleDocPath;
            fctr.OleDocFileName = factuur.OleDocFileName;
            fctr.DatumInsert = factuur.DatumInsert;
            fctr.UserInsert = factuur.UserInsert;
            fctr.DatumModified = factuur.DatumModified;
            fctr.UserModified = factuur.UserModified;

            return fctr;
        }

        // UPDATE:

        [HttpGet]
        public ActionResult Update(int? id)
        {
            return View(GetFactuurById((int)id));
        }

        [HttpPost]
        public ActionResult updateFactuur(FactuurModel fctr)
        {
            if (UpdateFactuur(fctr))
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

        public bool UpdateFactuur(FactuurModel fctr)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Factuur factuur = new Factuur();
            factuur.idFactuur = fctr.idFactuur;
            factuur.Boekjaar = fctr.Boekjaar;
            factuur.CvoVolgNummer = fctr.CvoVolgNummer;
            factuur.FactuurNummer = fctr.FactuurNummer;
            factuur.FactuurDatum = fctr.FactuurDatum;
            factuur.FactuurStatusGetekend = fctr.FactuurStatusGetekend;
            factuur.VerwerkingsDatum = fctr.VerwerkingsDatum;
            factuur.idLeverancier = fctr.idLeverancier;
            factuur.Prijs = fctr.Prijs;
            factuur.Garantie = fctr.Garantie;
            factuur.Omschrijving = fctr.Omschrijving;
            factuur.Opmerking = fctr.Opmerking;
            factuur.Afschrijfperiode = fctr.Afschrijfperiode;
            factuur.OleDoc = fctr.OleDoc;
            factuur.OleDocPath = fctr.OleDocPath;
            factuur.OleDocFileName = fctr.OleDocFileName;
            factuur.DatumInsert = fctr.DatumInsert;
            factuur.UserInsert = fctr.UserInsert;
            factuur.DatumModified = fctr.DatumModified;
            factuur.UserModified = fctr.UserModified;

            try
            {
                return sr.FactuurUpdate(factuur);
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
            return View(GetFactuurById((int)id));
        }

        [HttpPost]
        public ActionResult deleteFactuur(FactuurModel fctr)
        {
            if (DeleteFactuur(fctr))
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

        public bool DeleteFactuur(FactuurModel fctr)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            int id = fctr.idFactuur;

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