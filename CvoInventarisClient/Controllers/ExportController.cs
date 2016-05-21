using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Controllers
{
    public class ExportController : Controller
    {
        // GET: Export
        public ActionResult ExportToPdf(int[] modelList, string type)
        {
            List<object> objects = new List<object>();
            switch (type)
            {
                case "Object":
                    DAL.TblObject tblObject = new DAL.TblObject();
                    foreach (int id in modelList)
                    {
                        objects.Add(tblObject.GetById(id));
                    }
                    break;
            }
            //export code hier

            return RedirectToAction("Index", type);
        }
        public ActionResult ExportToExcel(int[] modelList, string type)
        {
            List<object> objects = new List<object>();
            switch (type)
            {
                case "Object":
                    DAL.TblObject tblObject = new DAL.TblObject();
                    foreach (int id in modelList)
                    {
                        objects.Add(tblObject.GetById(id));
                    }
                    break;
            }
            //export code hier

            return RedirectToAction("Index", type);
        }
    }
}