using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    [Authorize]
    public class HarddiskController : Controller
    {
        // INDEX:
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                List<Models.HarddiskModel> model = new List<Models.HarddiskModel>();
                foreach (Harddisk harddisk in client.HarddiskGetAll())
                {
                    model.Add(new Models.HarddiskModel() { IdHarddisk = harddisk.IdHarddisk, Merk = harddisk.Merk, Grootte = harddisk.Grootte, FabrieksNummer = harddisk.FabrieksNummer });
                }
                return View(model);
            }
        }

        // CREATE:
        // ValidateInput(false) voorkomt dat er een errormessage verschijnt bij bv:
        // < of > input in velden (potentieel gevaarlijke input error)       
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection, Models.HarddiskModel hd)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                // Wanneer ModelState.IsValid == true
                // Nieuw object word aangemaakt adhv Request.Form
                // User wordt ge-redirect naar Index pagina
                if (ModelState.IsValid)
                {
                    Harddisk harddisk = new Harddisk();
                    harddisk.Merk = Request.Form["merk"];
                    harddisk.Grootte = Convert.ToInt32(Request.Form["grootte"]);
                    harddisk.FabrieksNummer = Request.Form["fabrieksnummer"];

                    client.HarddiskCreate(harddisk);

                    TempData["action"] = "harddisk van het merk" + " " + Request.Form["merk"] + " werd toegevoegd";
                }

                // Wanneer ModelState.IsValid == false
                // User wordt ge-redirect naar Index pagina en de errors zijn te zien bovenaan het scherm
                string validationErrors = string.Join(",", ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors).Select(E => E.ErrorMessage).ToArray());

                TempData["action"] = "harddisk niet toegevoegd: " + validationErrors;
                return RedirectToAction("Index");
            }
        }

        // EDIT:
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Harddisk harddisk = client.HarddiskGetById(id);
                return View(new Models.HarddiskModel() { IdHarddisk = harddisk.IdHarddisk, Merk = harddisk.Merk, Grootte = harddisk.Grootte, FabrieksNummer = harddisk.FabrieksNummer });
            }
        }

        
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection, Models.HarddiskModel hd)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                // Wanneer ModelState.IsValid == true
                // Bestaand object word aangepast adhv Request.Form
                // User wordt ge-redirect naar de Index pagina
                if (ModelState.IsValid)
                {
                    Harddisk harddisk = new Harddisk();
                    harddisk.IdHarddisk = Convert.ToInt16(Request.Form["idHarddisk"]);
                    harddisk.Merk = Request.Form["merk"];
                    harddisk.Grootte = Convert.ToInt32(Request.Form["grootte"]);
                    harddisk.FabrieksNummer = Request.Form["fabrieksnummer"];

                    client.HarddiskUpdate(harddisk);

                    TempData["action"] = "harddisk van het merk" + " " + Request.Form["merk"] + " werd aangepast";
                    return RedirectToAction("Index");
                }

                // Wanneer ModelState.IsValid == false
                // Model wordt aangemaakt adhv GetById(id) methode
                // User wordt naar dezelfde edit pagina ge-redirect
                // Velden worden opnieuw ingevuld met de originele waarden + de errormessages zijn te zien
                Harddisk harddisknv = client.HarddiskGetById(id);
                Models.HarddiskModel harddiskmodel = new Models.HarddiskModel();
                harddiskmodel.IdHarddisk = id;
                harddiskmodel.Merk = harddisknv.Merk;
                harddiskmodel.Grootte = harddisknv.Grootte;
                harddiskmodel.FabrieksNummer = harddisknv.FabrieksNummer;

                return View(harddiskmodel);
            }
        }


        // DELETE:        
        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                foreach (int id in idArray)
                {
                    client.HarddiskDelete(id);
                }
                if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " harddisks werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " harddisk werd verwijderd";
                }
            }
            return RedirectToAction("Index");
        }
    }
}