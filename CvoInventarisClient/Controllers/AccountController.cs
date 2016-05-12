using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;
using System.Web.Security;
using System.Net.Mail;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

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
            catch (Exception)
            {

            }

            List<AccountModel> accs = new List<AccountModel>();

            foreach (Account acc in a)
            {
                AccountModel am = new AccountModel();
                am.IdAccount = acc.IdAccount;
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
            catch (Exception)
            {

            }

            AccountModel am = new AccountModel();
            am.IdAccount = acc.IdAccount;
            am.Email = acc.Email;
            am.Wachtwoord = acc.Wachtwoord;

            return am;
        }

        public bool UpdateAccount(AccountModel am)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            Account acc = new Account();
            acc.IdAccount = am.IdAccount;
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

        public ActionResult Loguit()
        {
            FormsAuthentication.SignOut();
            return Redirect(Url.Action("Index", "Login"));
        }

        public ActionResult ResetWachtwoord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetWachtwoord(AccountModel am)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            if (service.AccountVerstuurWachtwoordResetEmail(am.Email))
            {
                ViewBag.ResetWachtwoordMessage = "Een e-mail met instructies om uw wachtwoord te wijzigen werd naar uw geregistreerde e-mailadres verstuurd.";
            }
            else
            {
                ViewBag.ResetWachtwoordMessage = "E-mailadres niet gevonden!";
            }
            return View();
        }
    }
}