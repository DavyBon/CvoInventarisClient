using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    [Authorize]
    public class LokaalController : Controller
    {
        // INDEX:
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
            return View(tblLokaal.GetAll());            
        }
     
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
                
            LokaalModel lokaal = new LokaalModel();
            lokaal.LokaalNaam = Request.Form["lokaalNaam"];
            lokaal.AantalPlaatsen = Convert.ToInt32(Request.Form["aantalPlaatsen"]);
            
            if (Request.Form["isComputerLokaal"] != null)
            {
                lokaal.IsComputerLokaal = true;
            }
            else
            {
                lokaal.IsComputerLokaal = false;
            }

            DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
            tblLokaal.Create(lokaal);

            TempData["action"] = "lokaal" + " " + Request.Form["lokaalNaam"] + " werd toegevoegd";

            return RedirectToAction("Index");
        }

        // EDIT:
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
                return View(tblLokaal.GetById(id));
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            LokaalModel lokaal = new LokaalModel();
            lokaal.IdLokaal = Convert.ToInt16(Request.Form["idLokaal"]);
            lokaal.LokaalNaam = Request.Form["lokaalNaam"];
            lokaal.AantalPlaatsen = Convert.ToInt32(Request.Form["aantalPlaatsen"]);

            if (Request.Form["isComputerLokaal"] != null) { lokaal.IsComputerLokaal = true; }
            else
            {
                lokaal.IsComputerLokaal = false;
            }

            DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
            tblLokaal.Update(lokaal);

            TempData["action"] = "lokaal " + Request.Form["lokaalNaam"] + " werd aangepast";

            return RedirectToAction("Index");
        }


        // DELETE:
        [HttpPost]
        public ActionResult Delete(int[] idArray, FormCollection collection)
        {
            DAL.TblLokaal tblLokaal = new DAL.TblLokaal();
            foreach (int id in idArray)
            {
                tblLokaal.Delete(id);
            }
            if (idArray.Length >= 2)
            {
                TempData["action"] = idArray.Length + " lokalen werden verwijderd";
            }
            else
            {
                TempData["action"] = idArray.Length + " lokaal werd verwijderd";
            }

            return RedirectToAction("Index");            
        }
    }
}