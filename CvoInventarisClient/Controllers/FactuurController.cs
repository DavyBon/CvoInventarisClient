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

            Factuur[] arrFacturen = sr.FactuurGetAll();

            List<FactuurModel> listFacturen = new List<FactuurModel>();

            foreach (var item in arrFacturen)
            {
                FactuurModel ft = new FactuurModel();
                ft.idFactuur = item.idFactuur;
                ft.Boekjaar = item.Boekjaar;
                ft.CvoVolgNummer = item.CvoVolgNummer;
                ft.FactuurNummer = item.FactuurNummer;
                ft.FactuurDatum = item.FactuurDatum;
                ft.FactuurStatusGetekend = item.FactuurStatusGetekend;
                ft.VerwerkingsDatum = item.VerwerkingsDatum;
                ft.idLeverancier = item.idLeverancier;
                ft.Prijs = item.Prijs;
                ft.Garantie = item.Garantie;
                ft.Omschrijving = item.Omschrijving;
                ft.Opmerking = item.Opmerking;
                ft.Afschrijfperiode = item.Afschrijfperiode;
                ft.OleDoc = item.OleDoc;
                ft.OleDocPath = item.OleDocPath;
                ft.OleDocFileName = item.OleDocFileName;
                ft.DatumInsert = item.DatumInsert;
                ft.UserInsert = item.UserInsert;
                ft.DatumModified = item.DatumModified;
                ft.UserModified = item.UserModified;

                listFacturen.Add(ft);
            }

            return listFacturen;
        }


        // INSERT:

        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult insertFactuur(Factuur ft)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            sr.FactuurCreate(ft);

            return View("Index", ReadAll());
        }


        // UPDATE:

        [HttpGet]
        public ActionResult Update(int? id)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Factuur factuur = sr.FactuurGetById((int)id);

            FactuurModel factuurModel = new FactuurModel();

            factuurModel.idFactuur = (int)id;
            factuurModel.Boekjaar = factuur.Boekjaar;
            factuurModel.CvoVolgNummer = factuur.CvoVolgNummer;
            factuurModel.FactuurNummer = factuur.FactuurNummer;
            factuurModel.FactuurDatum = factuur.FactuurDatum;
            factuurModel.FactuurStatusGetekend = factuur.FactuurStatusGetekend;
            factuurModel.VerwerkingsDatum = factuur.VerwerkingsDatum;
            factuurModel.idLeverancier = factuur.idLeverancier;
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
            

            return View(factuurModel);
        }

        [HttpPost]
        public ActionResult updateFactuur(Factuur ft)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            sr.FactuurUpdate(ft);

            return View("Index", ReadAll());
        }


        // DELETE:

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            Factuur factuur = sr.FactuurGetById((int)id);

            FactuurModel factuurModel = new FactuurModel();

            factuurModel.idFactuur = (int)id;
            factuurModel.Boekjaar = factuur.Boekjaar;
            factuurModel.CvoVolgNummer = factuur.CvoVolgNummer;
            factuurModel.FactuurNummer = factuur.FactuurNummer;
            factuurModel.FactuurDatum = factuur.FactuurDatum;
            factuurModel.FactuurStatusGetekend = factuur.FactuurStatusGetekend;
            factuurModel.VerwerkingsDatum = factuur.VerwerkingsDatum;
            factuurModel.idLeverancier = factuur.idLeverancier;
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

            return View(factuurModel);
        }

        [HttpPost]
        public ActionResult deleteFactuur(Factuur ft)
        {
            CvoInventarisServiceClient sr = new CvoInventarisServiceClient();

            sr.FactuurDelete(ft.idFactuur);

            return View("Index", ReadAll());
        }
    }
}