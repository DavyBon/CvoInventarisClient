using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;
using System.Web.Security;

namespace CvoInventarisClient.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(AccountModel am, FormCollection formCollection)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            Account acc = new Account();
            acc.Email = am.Email;
            acc.Wachtwoord = am.Wachtwoord;

            try
            {
                // Als de ingegeven email en paswoord combinatie juist is
                if (service.AccountLogin(acc))
                {
                    ViewBag.LoginMessage = "Welkom, je bent ingelogd";

                    // Haal het Account uit DB voor zijn id (username van de cookie is de id van de gebruiker (IdAccount))
                    Account accUitDB = service.AccountGetByEmail(acc.Email);
                    string username = accUitDB.IdAccount.ToString();             

                    // hou bij of dit een persistente cookie moet zijn, volgens de onthouden checkbox
                    bool onthouden = formCollection["remember-me"] == "on" ? true : false;

                    // Maak een nieuwe authentication cookie voor deze gebruiker en redirect hem naar de Home pagina
                    FormsAuthentication.RedirectFromLoginPage(username, onthouden);
                }
                else
                {
                    ViewBag.LoginMessage = "Ongeldig e-mailadres of wachtwoord";
                }
            }
            catch
            {
            }

            return View();
        }
    }
}