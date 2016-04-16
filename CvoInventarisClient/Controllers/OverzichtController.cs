using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Controllers
{
    public class OverzichtController : Controller
    {
        // GET: Overzicht
        public ActionResult Index()
        {
            return View();
        }
    }
}