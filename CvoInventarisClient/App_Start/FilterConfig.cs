using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.App_Start
{
    // Deze klasse laat ons toe om het "HandleError" attribuut automatisch toe te passen op alle controllers en actionmethods.
    // Deze klasse werd geregistreerd in de global.asax file.

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}