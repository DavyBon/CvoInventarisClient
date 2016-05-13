using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Controllers
{

    // Omdat het "HandleError" attribuut geen rekening houdt met status code 404 werd deze controller aangemaakt.
    // In web.config werd volgende aanpassing gemaakt om naar de correcte view te verwijzen indien status code 404 optreed.
    //
    // <customErrors mode="On">
    // <error statusCode = "404" redirect="~/Error/NotFound"/>
    // </customErrors>
    //
    // Wanneer error statusCode 404 optreed redirecten we naar de Error controller en daarin naar de NotFound actionmethod.
    // Deze actionmethod returned de view met dezelfde naam. Deze bevind zich in de shared folder.

    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            return View();
        }
    }
}