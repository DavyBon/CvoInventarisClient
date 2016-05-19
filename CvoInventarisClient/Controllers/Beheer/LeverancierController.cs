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
    public class LeverancierController : Controller
    {

        // INDEX:
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
            return View(tblLeverancier.GetAll());
        }

        // CREATE:
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
            LeverancierModel leverancier = new LeverancierModel();
            leverancier.Naam = Request.Form["naam"];
            leverancier.Afkorting = Request.Form["afkorting"];
            leverancier.Straat = Request.Form["straat"];
            leverancier.HuisNummer = Request.Form["huisNummer"];
            leverancier.BusNummer = Request.Form["busNummer"];
            leverancier.Postcode = Convert.ToInt32(Request.Form["postcode"]);
            leverancier.Telefoon = Request.Form["telefoon"];
            leverancier.Fax = Request.Form["fax"];
            leverancier.Email = Request.Form["email"];
            leverancier.Website = Request.Form["website"];
            leverancier.BtwNummer = Request.Form["btwNummer"];
            leverancier.Iban = Request.Form["iban"];
            leverancier.Bic = Request.Form["bic"];
            leverancier.ToegevoegdOp = Request.Form["toegevoegdOp"];

            tblLeverancier.Create(leverancier);

            TempData["action"] = "leverancier" + " " + Request.Form["naam"] + " werd toegevoegd";

            return RedirectToAction("Index");
        }
        
        // EDIT:
        public ActionResult Edit(int id)
        {
            DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
            return View(tblLeverancier.GetById(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
            LeverancierModel leverancier = new LeverancierModel();
            leverancier.IdLeverancier = Convert.ToInt16(Request.Form["idLeverancier"]);
            leverancier.Naam = Request.Form["naam"];
            leverancier.Afkorting = Request.Form["afkorting"];
            leverancier.Straat = Request.Form["straat"];
            leverancier.HuisNummer = Request.Form["huisNummer"];
            leverancier.BusNummer = Request.Form["busNummer"];
            leverancier.Postcode = Convert.ToInt32(Request.Form["postcode"]);
            leverancier.Telefoon = Request.Form["telefoon"];
            leverancier.Fax = Request.Form["fax"];
            leverancier.Email = Request.Form["email"];
            leverancier.Website = Request.Form["website"];
            leverancier.BtwNummer = Request.Form["btwNummer"];
            leverancier.Iban = Request.Form["iban"];
            leverancier.Bic = Request.Form["bic"];
            leverancier.ToegevoegdOp = Request.Form["toegevoegdOp"];

            tblLeverancier.Update(leverancier);

            TempData["action"] = "leverancier" + " " + Request.Form["naam"] + " werd aangepast";
            
            return RedirectToAction("Index");
        }

        // DELETE:
        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            if (idArray == null) { return RedirectToAction("Index"); }
            DAL.TblLeverancier tblLeverancier = new DAL.TblLeverancier();
            foreach (int id in idArray)
            {
                tblLeverancier.Delete(id);
            }
            if (idArray.Length >= 2)
            {
                TempData["action"] = idArray.Length + " leveranciers werden verwijderd";
            }
            else
            {
                TempData["action"] = idArray.Length + " leverancier werd verwijderd";
            }

            return RedirectToAction("Index");
        }
    }
}