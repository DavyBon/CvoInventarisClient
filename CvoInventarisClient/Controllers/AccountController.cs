﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
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
        [Authorize]
        public ActionResult Index()
        {
            return View(GetAccounts());
        }

        [Authorize]
        public ActionResult Create()
        {
            DAL.TblAccount tblAccount = new DAL.TblAccount();
            AccountModel account = new AccountModel();
            account.Email = Request.Form["Email"];
            account.Wachtwoord = Request.Form["Wachtwoord"];

            if(tblAccount.GetByEmail(account.Email).Email == account.Email) // als het e-mailadres al in de DB staat
            {                
                ViewBag.warningAccountEmailDuplicateMessage = "Er bestaat al een account met e-mailadres " + account.Email + ". Gelieve een ander e-mailadres te gebruiken.";
            }
            else // het email adres staat er nog niet in
            {
                if (tblAccount.Create(account) > 0)
                {
                    ViewBag.successAccountDeleteMessage = "Het account " + account.Email + " is aangemaakt.";
                }
                else
                {
                    ViewBag.warningAccountCreatedMessage = "Het account " + account.Email + " is niet aangemaakt.";
                }
            }

            return View("index", GetAccounts());
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            return View(GetAccountById(id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(AccountModel am)
        {
            DAL.TblAccount tblAccount = new DAL.TblAccount();
            if (tblAccount.Delete(am.IdAccount))
            {
                ViewBag.successAccountDeleteMessage = "Het account " + am.Email + " is verwijdert.";
            }
            else
            {
                ViewBag.warningAccountDeleteMessage = "Het account " + am.Email + " is niet verwijdert.";
            }
            return View("index", GetAccounts());
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        //public ActionResult Loguit()
        //{
        //    FormsAuthentication.SignOut();
        //    return Redirect(Url.Action("Index", "Login"));
        //}

        public ActionResult ResetWachtwoord()
        {
            string idAccount = HttpContext.User.Identity.Name;
            if(!string.IsNullOrWhiteSpace(idAccount))
            {
                DAL.TblAccount tblAccount = new DAL.TblAccount();
                AccountModel account = tblAccount.GetById(Convert.ToInt32(idAccount));
                ViewBag.accountEmail = account.Email;
            }
            else
            {
                ViewBag.accountEmail = "";
            }           

            return View();
        }

        [HttpPost]
        public ActionResult ResetWachtwoord(AccountModel am)
        {
            DAL.TblAccount tblAccount = new DAL.TblAccount();

            if (tblAccount.VerstuurWachtwoordResetEmail(am.Email))
            {
                ViewBag.successResetWachtwoordMessage = "Een e-mail met instructies om uw wachtwoord te wijzigen werd naar uw geregistreerde e-mailadres verstuurd.";
            }
            else
            {
                ViewBag.warningResetWachtwoordMessage = "E-mailadres niet gevonden!";
            }
            return View();
        }

        public ActionResult WijzigWachtwoord()
        {
            DAL.TblAccount tblAccount = new DAL.TblAccount();

            if (!tblAccount.IsWachtwoordResetLinkValid(Request.QueryString["uid"]))
            {
                ViewBag.warningWijzigWachtwoordMessage = "Wachtwoord reset link is verlopen of ongeldig!";
            }

            return View();
        }

        [HttpPost]
        public ActionResult WijzigWachtwoord(AccountModel am, string GUID)
        {
            DAL.TblAccount tblAccount = new DAL.TblAccount();

            if (tblAccount.WijzigWachtwoord(GUID, am.Wachtwoord))
            {
                ViewBag.successWijzigWachtwoordMessage = "U kan nu inloggen met uw nieuwe wachtwoord.";
            }
            else
            {
                ViewBag.warningWijzigWachtwoordMessage = "Wachtwoord reset link is verlopen of ongeldig!";
            }
            return View();
        }
    }
}