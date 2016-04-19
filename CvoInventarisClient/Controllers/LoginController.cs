using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(AccountModel am)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            Account acc = new Account();
            acc.IdAccount = am.IdAccount;
            acc.Type = am.Type;
            acc.Gebruikersnaam = am.Gebruikersnaam;
            acc.Voornaam = am.Voornaam;
            acc.Achternaam = am.Achternaam;
            acc.Telefoonnummer = am.Telefoonnummer;
            acc.Email = am.Email;
            acc.Wachtwoord = am.Wachtwoord;

            try
            {
                if (service.AccountLogin(acc))
                {
                    ViewBag.LoginMessage = "Welkom, je bent ingelogd";
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