using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;

namespace CvoInventarisClient.Controllers
{
    [Authorize]
    public class VerzekeringController : Controller
    {
        public List<VerzekeringModel> Getverzekeringen()
        {
            DAL.TblVerzekering dalVerzekering = new DAL.TblVerzekering();
            return dalVerzekering.GetAll();
            }

        public ActionResult Index(int? amount, string order, bool? refresh)
        {
            ViewBag.action = TempData["action"];
            VerzekeringViewModel model = new VerzekeringViewModel();
            if (Session["verzekeringviewwmodel"] == null || refresh == true)
            {
                DAL.TblVerzekering dalVerzekering = new DAL.TblVerzekering();
                List<VerzekeringModel> verzekeringen = dalVerzekering.GetAll().OrderBy(i => i.Id).Reverse().ToList();
                model.Verzekeringen = verzekeringen;
                
            }
            else
            {
                model = (VerzekeringViewModel)Session["verzekeringviewwmodel"];
            }
            Session["verzekeringviewwmodel"] = model.Clone();
            if (amount == null)
            {
                model.Verzekeringen = model.Verzekeringen.Take(100).ToList();
                ViewBag.amount = "100";
            }
            else
            {
                model.Verzekeringen = model.Verzekeringen.Take((int)amount).ToList();
                ViewBag.amount = amount.ToString();
            }

            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("Oudst"))
                {
                    model.Verzekeringen.Reverse();
                }
                ViewBag.ordertype = order.ToString();
            }
            else
            {
                ViewBag.ordertype = "Meest recent"; 
            }

            ViewBag.Heading = this.ControllerContext.RouteData.Values["controller"].ToString() + " (" + model.Verzekeringen.Count() + ")";
            return View(model);
            
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            DAL.TblVerzekering dalVerzekering = new DAL.TblVerzekering();
            VerzekeringModel verzekering = new VerzekeringModel();
            verzekering.Omschrijving = Request.Form["omschrijving"];
            dalVerzekering.Create(verzekering);
            TempData["action"] = "verzekering" + " " + Request.Form["omschrijving"] + " werd toegevoegd";
           
            return RedirectToAction("Index");
        }

        // GET: Inventaris/Edit/5
        public ActionResult Edit(int id)
        {
            VerzekeringViewModel model = new VerzekeringViewModel();
            DAL.TblVerzekering dalVerzekering = new DAL.TblVerzekering();
            VerzekeringModel verzekering = dalVerzekering.GetById(id);
            model.Verzekeringen.Add(verzekering);
            return View(model);
        }

        // POST: Inventaris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            
            DAL.TblVerzekering dalVerzekering = new DAL.TblVerzekering();
            VerzekeringModel verzekering = new VerzekeringModel();
            verzekering.Id = Convert.ToInt16(Request.Form["idVerzekering"]);
            verzekering.Omschrijving = Request.Form["omschrijving"];
            TempData["action"] = Request.Form["omschrijving"] + " werd aangepast";
            dalVerzekering.Update(verzekering);
            return RedirectToAction("Index");
        }

        // POST: Inventaris/Delete/5
        [HttpPost]
        public ActionResult Delete(int[] idArray)
        {
            if (idArray == null) { return RedirectToAction("Index"); }
            DAL.TblVerzekering dalVerzekering = new DAL.TblVerzekering();
            foreach (int id in idArray)
                {
                    dalVerzekering.Delete(id);
                }
            if (idArray.Length >= 2)
                {
                    TempData["action"] = idArray.Length + " verzekeringen werden verwijderd";
                }
            else
                {
                    TempData["action"] = idArray.Length + " verzekeringen werd verwijderd";
                }
            
            return RedirectToAction("Index");
        }
    }
}