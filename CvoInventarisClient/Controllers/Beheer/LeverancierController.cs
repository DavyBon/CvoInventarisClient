﻿using System;
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
        public ActionResult Index()
        {
            ViewBag.action = TempData["action"];
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.LeverancierGetAll());
            }
        }

        // CREATE:
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Leverancier leverancier = new Leverancier();
                leverancier.Naam = Request.Form["naam"];
                leverancier.Afkorting = Request.Form["afkorting"];
                leverancier.Straat = Request.Form["straat"];
                leverancier.HuisNummer = Convert.ToInt32(Request.Form["huisNummer"]);
                leverancier.BusNummer = Convert.ToInt32(Request.Form["busNummer"]);
                leverancier.Postcode = Convert.ToInt32(Request.Form["postcode"]);
                leverancier.Telefoon = Request.Form["telefoon"];
                leverancier.Fax = Request.Form["fax"];
                leverancier.Email = Request.Form["email"];
                leverancier.Website = Request.Form["website"];
                leverancier.BtwNummer = Request.Form["btwNummer"];
                leverancier.Iban = Request.Form["iban"];
                leverancier.Bic = Request.Form["bic"];
                leverancier.ToegevoegdOp = Convert.ToDateTime(Request.Form["toegevoegdOp"]);

                client.LeverancierCreate(leverancier);

                TempData["action"] = "leverancier" + " " + Request.Form["naam"] + " werd toegevoegd";
            }
            return RedirectToAction("Index");
        }
        
        // EDIT:
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                return View(client.LeverancierGetById(id));
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                Leverancier leverancier = new Leverancier();
                leverancier.Naam = Request.Form["naam"];
                leverancier.Afkorting = Request.Form["afkorting"];
                leverancier.Straat = Request.Form["straat"];
                leverancier.HuisNummer = Convert.ToInt32(Request.Form["huisNummer"]);
                leverancier.BusNummer = Convert.ToInt32(Request.Form["busNummer"]);
                leverancier.Postcode = Convert.ToInt32(Request.Form["postcode"]);
                leverancier.Telefoon = Request.Form["telefoon"];
                leverancier.Fax = Request.Form["fax"];
                leverancier.Email = Request.Form["email"];
                leverancier.Website = Request.Form["website"];
                leverancier.BtwNummer = Request.Form["btwNummer"];
                leverancier.Iban = Request.Form["iban"];
                leverancier.Bic = Request.Form["bic"];
                leverancier.ToegevoegdOp = Convert.ToDateTime(Request.Form["toegevoegdOp"]);

                client.LeverancierUpdate(leverancier);

                TempData["action"] = "leverancier" + " " + Request.Form["naam"] + " werd aangepast";
            }
            return RedirectToAction("Index");
        }

        // DELETE:
        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            using (CvoInventarisServiceClient client = new CvoInventarisServiceClient())
            {
                foreach(int id in idArray)
                {
                    client.LeverancierDelete(id);
                }
                if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " leveranciers werden verwijderd";
                }
                else
                {
                    TempData["action"] = idArray.Length + " leverancier werd verwijderd";
                }
            }
            return RedirectToAction("Index");
        }
    }
}