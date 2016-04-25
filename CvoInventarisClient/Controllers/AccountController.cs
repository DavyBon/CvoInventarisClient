using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;

namespace CvoInventarisClient.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View(GetAccounts());
        }

        public ActionResult Edit(int id)
        {
            return View(GetAccountById(id));
        }

        [HttpPost]
        public ActionResult Edit(AccountModel am)
        {
            if (UpdateAccount(am))
            {
                ViewBag.EditMesage = "Row updated";
                //return View("Index");
                return View();
            }
            else
            {
                ViewBag.EditMesage = "Row not updated";
                return View();
            }
        }

        public ActionResult Create()
        {
            return View(new AccountModel());
        }

        [HttpPost]
        public ActionResult Create(AccountModel am)
        {
            if (InsertAccount(am) >= 0)
            {
                ViewBag.CreateMesage = "Row inserted";
                //return View("Index");
                return View();
            }
            else
            {
                ViewBag.CreateMesage = "Row not inserted";
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            return View(GetAccountById(id));
        }

        public ActionResult Delete(int id)
        {
            return View(GetAccountById(id));
        }

        [HttpPost]
        public ActionResult Delete(AccountModel am)
        {
            if (DeleteAccount(am))
            {
                ViewBag.DeleteMesage = "Row deleted";
                //return View("Index");
                return View();
            }
            else
            {
                ViewBag.DeleteMesage = "Row not deleted";
                return View();
            }
        }

        public List<AccountModel> GetAccounts()
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            Account[] a = new Account[] { };

            try
            {
                a = service.AccountGetAll();
            }
            catch (Exception e)
            {

            }

            List<AccountModel> accs = new List<AccountModel>();

            foreach (Account acc in a)
            {
                AccountModel am = new AccountModel();
                am.IdAccount = acc.IdAccount;
                am.Type = acc.Type;
                am.Gebruikersnaam = acc.Gebruikersnaam;
                am.Voornaam = acc.Voornaam;
                am.Achternaam = acc.Achternaam;
                am.Telefoonnummer = acc.Telefoonnummer;
                am.Email = acc.Email;
                am.Wachtwoord = acc.Wachtwoord;
                accs.Add(am);              
            }

            return accs;
        }

        public AccountModel GetAccountById(int id)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            Account acc = new Account();

            try
            {
                acc = service.AccountGetById(id);
            }
            catch (Exception e)
            {

            }

            AccountModel am = new AccountModel();
            am.IdAccount = acc.IdAccount;
            am.Type = acc.Type;
            am.Gebruikersnaam = acc.Gebruikersnaam;
            am.Voornaam = acc.Voornaam;
            am.Achternaam = acc.Achternaam;
            am.Telefoonnummer = acc.Telefoonnummer;
            am.Email = acc.Email;
            am.Wachtwoord = acc.Wachtwoord;

            return am;
        }

        public bool UpdateAccount(AccountModel am)
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
                return service.AccountUpdate(acc);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int InsertAccount(AccountModel am)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            Account acc = new Account();
            acc.Type = am.Type;
            acc.Gebruikersnaam = am.Gebruikersnaam;
            acc.Voornaam = am.Voornaam;
            acc.Achternaam = am.Achternaam;
            acc.Telefoonnummer = am.Telefoonnummer;
            acc.Email = am.Email;
            acc.Wachtwoord = am.Wachtwoord;

            try
            {
                return service.AccountCreate(acc);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool DeleteAccount(AccountModel am)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            int id = am.IdAccount;

            try
            {
                return service.AccountDelete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}