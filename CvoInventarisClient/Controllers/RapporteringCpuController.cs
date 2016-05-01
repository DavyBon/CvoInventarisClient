using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Controllers
{
    public class RapporteringCpuController : Controller
    {
        public ActionResult CpuRapportering()
        {
            ViewBag.styleCpuStap4 = "none";
            ViewBag.styleCpuStap5 = "none";
            return View();
        }
        public RapporteringCpuController()
        {

        }
    }
}