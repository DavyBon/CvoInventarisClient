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
                ViewBag.EditMesage = "Het account is aangepst";
                //return View("Index");
                return View();
            }
            else
            {
                ViewBag.EditMesage = "Het account is niet aangepst";
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
                ViewBag.CreateMesage = "Het account is toegevoegd";
                return View();
            }
            else
            {
                ViewBag.CreateMesage = "Het account is niet toegevoegd";
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
                ViewBag.DeleteMesage = "Het account is verwijderd";
                return View();
            }
            else
            {
                ViewBag.DeleteMesage = "Het account is niet verwijderd";
                return View();
            }
        }

        public List<AccountModel> GetAccounts()
        {
            DAL.TblAccount tblAccount = new DAL.TblAccount();
            List<AccountModel> accs = new List<AccountModel>();

            try
            {
                accs = tblAccount.GetAll();
            }
            catch (Exception)
            {
            }

            return accs;
        }

        public AccountModel GetAccountById(int id)
        {
            DAL.TblAccount tblAccount = new DAL.TblAccount();
            AccountModel acc = new AccountModel();

            try
            {
                acc = tblAccount.GetById(id);
            }
            catch (Exception)
            {
            }

            return acc;
        }

        public bool UpdateAccount(AccountModel am)
        {
            DAL.TblAccount tblAccount = new DAL.TblAccount();

            try
            {
                return tblAccount.Update(am);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int InsertAccount(AccountModel am)
        {
            DAL.TblAccount tblAccount = new DAL.TblAccount();

            try
            {
                return tblAccount.Create(am);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool DeleteAccount(AccountModel am)
        {
            DAL.TblAccount tblAccount = new DAL.TblAccount();

            try
            {
                return tblAccount.Delete(am.IdAccount);
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
            DAL.TblAccount tblAccount = new DAL.TblAccount();

            if (tblAccount.VerstuurWachtwoordResetEmail(am.Email))
            {
                ViewBag.ResetWachtwoordMessage = "Een e-mail met instructies om uw wachtwoord te wijzigen werd naar uw geregistreerde e-mailadres verstuurd.";
            }
            else
            {
                ViewBag.ResetWachtwoordMessage = "E-mailadres niet gevonden!";
            }
            return View();
        }

        public ActionResult WijzigWachtwoord()
        {
            DAL.TblAccount tblAccount = new DAL.TblAccount();

            if (!tblAccount.IsWachtwoordResetLinkValid(Request.QueryString["uid"]))
            {
                ViewBag.WijzigWachtwoordMessage = "Opgelet, wachtwoord reset link is verlopen of ongeldig!";
            }

            return View();
        }

        [HttpPost]
        public ActionResult WijzigWachtwoord(AccountModel am, string GUID)
        {
            DAL.TblAccount tblAccount = new DAL.TblAccount();

            if (tblAccount.WijzigWachtwoord(GUID, am.Wachtwoord))
            {
                ViewBag.WijzigWachtwoordMessage = "Uw wachtwoord is gewijzigd! U kan nu inloggen met uw nieuwe wachtwoord.";
            }
            else
            {
                ViewBag.WijzigWachtwoordMessage = "Opgelet, wachtwoord reset link is verlopen of ongeldig!";
            }
            return View();
        }
    }
}