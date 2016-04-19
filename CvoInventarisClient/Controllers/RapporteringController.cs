﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
using CvoInventarisClient.ServiceReference;
using CvoInventarisClient.Models;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Controllers
{

    public class RapporteringController : Controller
    {
        CvoInventarisServiceClient test = new CvoInventarisServiceClient();

        public ActionResult HarddiskRapportering()
        {
            ViewBag.styleCpuStap4 = "none";
            ViewBag.styleCpuStap5 = "none";
            return View();
        }

        public ActionResult LeverancierRapportering()
        {
            ViewBag.styleCpuStap4 = "none";
            ViewBag.styleCpuStap5 = "none";
            return View();
        }

        public ActionResult NetwerkRapportering()
        {
            ViewBag.styleCpuStap4 = "none";
            ViewBag.styleCpuStap5 = "none";
            return View();
        }

        public ActionResult CpuRapportering()
        {
            ViewBag.styleCpuStap4 = "none";
            ViewBag.styleCpuStap5 = "none";
            return View();
        }

        public ActionResult GrafischeKaartRapportering()
        {
            ViewBag.styleCpuStap4 = "none";
            ViewBag.styleCpuStap5 = "none";
            return View();
        }

        public ActionResult DeviceRapportering()
        {
            ViewBag.styleCpuStap4 = "none";
            ViewBag.styleCpuStap5 = "none";
            return View();
        }

        public ActionResult Index()
        {
            ViewBag.ObjectType = "none";
            ViewBag.Verzekering = "none";
            @ViewBag.Cpu = "none";
            ViewBag.Device = "none";
            ViewBag.Factuur = "none";
            ViewBag.GrafischeKaart = "none";
            ViewBag.Harddisk = "none";
            ViewBag.Hardware = "none";
            ViewBag.Inventaris = "none";
            ViewBag.Leverancier = "none";
            ViewBag.Lokaal = "none";
            ViewBag.Netwerk = "none";
            ViewBag.Object = "none";
            return View();
        }

        [HttpPost]
        public ActionResult Stap1(string stap1)
        {
            string request = Request["action"];
            string[] requestOplossing = request.Split('/');
            if (requestOplossing[0].Equals("stap2ZonderConditie"))
            {
                if (stap1.Equals("verzekering"))
                {
                    ViewBag.VerzekeringList = test.VerzekeringGetAll();
                    ViewBag.Verzekering = "inline";
                    ViewBag.resultaat = stap1;
                }
                if (stap1.Equals("objectType"))
                {
                    ViewBag.MyList = test.ObjectTypeGetAll();
                    ViewBag.ObjectType = "inline";
                    ViewBag.resultaat = stap1;
                }
                if (stap1.Equals("cpu"))
                {
                    ViewBag.CpuList = test.CpuGetAll();
                    ViewBag.Cpu = "inline";
                    ViewBag.resultaat = stap1;

                }
                if (stap1.Equals("device"))
                {
                    ViewBag.DeviceList = test.DeviceGetAll();
                    ViewBag.Device = "inline";
                    ViewBag.resultaat = stap1;
                }
                if (stap1.Equals("factuur"))
                {
                    
                }
                if (stap1.Equals("grafischeKaart"))
                {
                    ViewBag.GrafischeKaartList = test.GrafischeKaartGetAll();
                    ViewBag.GrafischeKaart = "inline";
                    ViewBag.resultaat = stap1;
                }
                if (stap1.Equals("harddisk"))
                {
                    ViewBag.HarddiskList = test.HarddiskGetAll();
                    ViewBag.Harddisk = "inline";
                    ViewBag.resultaat = stap1;
                }
                if (stap1.Equals("hardware"))
                {
                    ViewBag.HardwareList = test.HardwareGetAll();
                    ViewBag.Hardware = "inline";
                    ViewBag.resultaat = stap1;
                }
                if (stap1.Equals("inventaris"))
                {
                    ViewBag.InventarisList = test.InventarisGetAll();
                    ViewBag.Inventaris = "inline";
                    ViewBag.resultaat = stap1;
                }
                if (stap1.Equals("leverancier"))
                {
                    ViewBag.LeverancierList = test.LeverancierGetAll();
                    ViewBag.Leverancier = "inline";
                    ViewBag.resultaat = stap1;
                }
                if (stap1.Equals("lokaal"))
                {
                    ViewBag.LokaalList = test.LokaalGetAll();
                    ViewBag.Lokaal = "inline";
                    ViewBag.resultaat = stap1;
                }
                if (stap1.Equals("netwerk"))
                {
                    ViewBag.NetwerkList = test.NetwerkGetAll();
                    ViewBag.Netwerk = "inline";
                    ViewBag.resultaat = stap1;
                }
                if (stap1.Equals("object"))
                {
                    ViewBag.ObjectList = test.ObjectGetAll();
                    ViewBag.Object = "inline";
                    ViewBag.resultaat = stap1;
                }
                return View("Index");
            }
            if (requestOplossing[0].Equals("stap2Conditie"))
            {
                if (stap1.Equals("cpu")){
                    return View("CpuRapportering");
                }
                if (stap1.Equals("device"))
                {
                    return View("DeviceRapportering");
                }
                if (stap1.Equals("grafischeKaart"))
                {
                    return View("GrafischeKaartRapportering");
                }
                if (stap1.Equals("harddisk"))
                {
                    return View("HarddiskRapportering");
                }
                if (stap1.Equals("leverancier"))
                {
                    return View("LeverancierRapportering");
                }
                if (stap1.Equals("netwerk"))
                {
                    return View("NetwerkRapportering");
                }
            }
            if (requestOplossing[0].Equals("OpslaanPdf"))
            {
                OpslaanPdf(requestOplossing[1]);
            }
            return null;

        }

        [HttpPost]
        public ActionResult Stap3Cpu(FormCollection collection)
        {
            string request = Request["action"];
            string[] requestOplossing = request.Split('/');
            if (requestOplossing[0].Equals("OpslaanPdf"))
            {
                OpslaanPdfCpu(requestOplossing[1], requestOplossing[2]);
                ViewBag.testj = requestOplossing[1] + requestOplossing[2];
            }
            if (requestOplossing[0].Equals("kolomKeuze"))
            {
                string resultaat = "";
                bool checkId = Convert.ToBoolean(collection["idCpu"].Split(',')[0]);
                bool checkMerk = Convert.ToBoolean(collection["merk"].Split(',')[0]);
                bool checkType = Convert.ToBoolean(collection["type"].Split(',')[0]);
                bool checkSnelheid = Convert.ToBoolean(collection["snelheid"].Split(',')[0]);
                bool checkFabrieksnummer = Convert.ToBoolean(collection["fabrieksnummer"].Split(',')[0]);
                if (checkId == true)
                    resultaat += "idCpu";
                if (checkMerk == true)
                    resultaat += " merk";
                if (checkType == true)
                    resultaat += " type";
                if (checkSnelheid == true)
                    resultaat += " snelheid";
                if (checkFabrieksnummer == true)
                    resultaat += " fabrieksNummer";
                ViewBag.styleCpuStap4 = "inline";
                ViewBag.resultaatCheck = resultaat;
            }
            if (requestOplossing[0].Equals("conditieKeuze"))
            {
                ViewBag.styleCpuStap5 = "inline";
                string keuzeKolom = requestOplossing[1];
                string[] tussen = keuzeKolom.Split(' ');
                List<string> keuzeKolommen = new List<string>();
                foreach(string s in tussen)
                {
                    if (!s.Equals(""))
                    {
                        keuzeKolommen.Add(s);
                    }
                }
                
                var lijstConditie = new List<KeyValuePair<string, string>>();
                bool checkMerk = Convert.ToBoolean(collection["merkKeuzeCheck"].Split(',')[0]);
                bool checkType = Convert.ToBoolean(collection["typeKeuzeCheck"].Split(',')[0]);
                bool checkSnelheid = Convert.ToBoolean(collection["snelheidKeuzeCheck"].Split(',')[0]);
                bool checkFabrieksnummer = Convert.ToBoolean(collection["fabrieksnummerKeuzeCheck"].Split(',')[0]);
                if(checkMerk == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("merk"," = " + "'"+ collection["merkKeuzeTekst"].Split(',')[0] + "'"));
                }
                if(checkType == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("type", " = " + "'" + collection["typeKeuzeTekst"].Split(',')[0] + "'"));
                }
                if(checkSnelheid == true)
                {
                    string resultaat = collection["snelheidKeuze"].Split(',')[0] + " " + collection["snelheidKeuze1"].Split(',')[0];
                    lijstConditie.Add(new KeyValuePair<string, string>("snelheid", " " + resultaat));
                }
                if(checkFabrieksnummer == true)
                {
                    string resultaat = collection["fabrieksnummerKeuze"].Split(',')[0] + " " + collection["fabrieksnummerKeuze1"].Split(',')[0];
                    lijstConditie.Add(new KeyValuePair<string, string>("fabrieksNummer"," " + resultaat));
                }
                string query = "SELECT ";
                foreach (string s in keuzeKolommen)
                {
                    query += s + ", ";
                }
                query = query.Substring(0, query.Length - 2);
                query += " FROM TblCpu ";
                query += " WHERE ";
                foreach(var element in lijstConditie)
                {
                    query += element.Key + element.Value + " AND ";
                }
                query = query.Substring(0, query.Length - 4);
                query += " ;";
                ViewBag.Rapport = test.RapporteringCpu(query, keuzeKolommen.ToArray());
                
                foreach(string s in keuzeKolommen)
                {
                    ViewBag.tables += s + " ";
                    if (s.Equals("idCpu"))
                        ViewBag.test = s;
                    if (s.Equals("merk"))
                        ViewBag.test1 = s;
                    if (s.Equals("type"))
                        ViewBag.test2 = s;
                    if (s.Equals("snelheid"))
                        ViewBag.test3 = s;
                    if (s.Equals("fabrieksNummer"))
                        ViewBag.test4 = s;
                }
                ViewBag.query = query;
            }
            
            return View("CpuRapportering");
        }

        [HttpPost]
        public ActionResult Stap3Device(FormCollection collection)
        {
            string request = Request["action"];
            string[] requestOplossing = request.Split('/');
            if (requestOplossing[0].Equals("OpslaanPdf"))
            {
                OpslaanPdfDevice(requestOplossing[1], requestOplossing[2]);
            }
            if (requestOplossing[0].Equals("kolomKeuze"))
            {
                string resultaat = "";
                bool checkId = Convert.ToBoolean(collection["idDevice"].Split(',')[0]);
                bool checkMerk = Convert.ToBoolean(collection["merk"].Split(',')[0]);
                bool checkType = Convert.ToBoolean(collection["type"].Split(',')[0]);
                bool checkSerienummer = Convert.ToBoolean(collection["serienummer"].Split(',')[0]);
                bool checkIsPcCompatibel = Convert.ToBoolean(collection["isPcCompatibel"].Split(',')[0]);
                bool checkFabrieksnummer = Convert.ToBoolean(collection["fabrieksnummer"].Split(',')[0]);
                if (checkId == true)
                    resultaat += "idDevice";
                if (checkMerk == true)
                    resultaat += " merk";
                if (checkType == true)
                    resultaat += " type";
                if (checkSerienummer == true)
                    resultaat += " serienummer";
                if (checkIsPcCompatibel == true)
                    resultaat += " isPcCompatibel";
                if (checkFabrieksnummer == true)
                    resultaat += " fabrieksNummer";
                ViewBag.styleCpuStap4 = "inline";
                ViewBag.resultaatCheck = resultaat;
            }
            if (requestOplossing[0].Equals("conditieKeuze"))
            {
                ViewBag.styleCpuStap5 = "inline";
                string keuzeKolom = requestOplossing[1];
                string[] tussen = keuzeKolom.Split(' ');
                List<string> keuzeKolommen = new List<string>();
                foreach (string s in tussen)
                {
                    if (!s.Equals(""))
                    {
                        keuzeKolommen.Add(s);
                    }
                }

                var lijstConditie = new List<KeyValuePair<string, string>>();
                bool checkMerk = Convert.ToBoolean(collection["merkKeuzeCheck"].Split(',')[0]);
                bool checkType = Convert.ToBoolean(collection["typeKeuzeCheck"].Split(',')[0]);
                bool checkSerienummer = Convert.ToBoolean(collection["serienummerKeuzeCheck"].Split(',')[0]);
                bool checkIsPcCompatibel = Convert.ToBoolean(collection["isPcCompatibelKeuzeCheck"].Split(',')[0]);
                bool checkFabrieksnummer = Convert.ToBoolean(collection["fabrieksnummerKeuzeCheck"].Split(',')[0]);
                if (checkMerk == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("merk", " = " + "'" + collection["merkKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkType == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("type", " = " + "'" + collection["typeKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkSerienummer == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("serienummer", " = " + "'" + collection["serienummerKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkIsPcCompatibel == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("isPcCompatibel", " = " + "'" + collection["isPcCompatibelKeuze"].Split(',')[0] + "'"));
                }
                if (checkFabrieksnummer == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("fabrieksNummer", " = " + "'" + collection["fabrieksNummerKeuzeTekst"].Split(',')[0] + "'"));
                }
                string query = "SELECT ";
                foreach (string s in keuzeKolommen)
                {
                    query += s + ", ";
                }
                query = query.Substring(0, query.Length - 2);
                query += " FROM TblDevice ";
                query += " WHERE ";
                foreach (var element in lijstConditie)
                {
                    query += element.Key + element.Value + " AND ";
                }
                query = query.Substring(0, query.Length - 4);
                query += " ;";
                ViewBag.Rapport = test.RapporteringDevice(query, keuzeKolommen.ToArray());
                foreach (string s in keuzeKolommen)
                {
                    ViewBag.tables += s + " ";
                    if (s.Equals("idDevice"))
                        ViewBag.test = s;
                    if (s.Equals("merk"))
                        ViewBag.test1 = s;
                    if (s.Equals("type"))
                        ViewBag.test2 = s;
                    if (s.Equals("serienummer"))
                        ViewBag.test3 = s;
                    if (s.Equals("isPcCompatibel"))
                        ViewBag.test3 = s;
                    if (s.Equals("fabrieksNummer"))
                        ViewBag.test4 = s;
                }
                ViewBag.query = query;
            }

            return View("DeviceRapportering");
        }

        [HttpPost]
        public ActionResult Stap3Netwerk(FormCollection collection)
        {
            string request = Request["action"];
            string[] requestOplossing = request.Split('/');
            if (requestOplossing[0].Equals("OpslaanPdf"))
            {
                OpslaanPdfNetwerk(requestOplossing[1], requestOplossing[2]);
                ViewBag.testj = requestOplossing[1] + requestOplossing[2];
            }
            if (requestOplossing[0].Equals("kolomKeuze"))
            {
                string resultaat = "";
                bool checkId = Convert.ToBoolean(collection["idNetwerk"].Split(',')[0]);
                bool checkMerk = Convert.ToBoolean(collection["merk"].Split(',')[0]);
                bool checkSnelheid = Convert.ToBoolean(collection["snelheid"].Split(',')[0]);
                bool checkType = Convert.ToBoolean(collection["type"].Split(',')[0]);
                bool checkDriver = Convert.ToBoolean(collection["driver"].Split(',')[0]);
                if (checkId == true)
                    resultaat += "idNetwerk";
                if (checkMerk == true)
                    resultaat += " merk";
                if (checkType == true)
                    resultaat += " type";
                if (checkSnelheid == true)
                    resultaat += " snelheid";
                if (checkDriver == true)
                    resultaat += " driver";
                ViewBag.styleCpuStap4 = "inline";
                ViewBag.resultaatCheck = resultaat;
            }
            if (requestOplossing[0].Equals("conditieKeuze"))
            {
                ViewBag.styleCpuStap5 = "inline";
                string keuzeKolom = requestOplossing[1];
                string[] tussen = keuzeKolom.Split(' ');
                List<string> keuzeKolommen = new List<string>();
                foreach (string s in tussen)
                {
                    if (!s.Equals(""))
                    {
                        keuzeKolommen.Add(s);
                    }
                }

                var lijstConditie = new List<KeyValuePair<string, string>>();
                bool checkMerk = Convert.ToBoolean(collection["merkKeuzeCheck"].Split(',')[0]);
                bool checkType = Convert.ToBoolean(collection["typeKeuzeCheck"].Split(',')[0]);
                bool checkSnelheid = Convert.ToBoolean(collection["snelheidKeuzeCheck"].Split(',')[0]);
                bool checkDriver = Convert.ToBoolean(collection["driverKeuzeCheck"].Split(',')[0]);
                if (checkMerk == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("merk", " = " + "'" + collection["merkKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkType == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("type", " = " + "'" + collection["typeKeuzeTekst"].Split(',')[0] + "'"));
                }
                if(checkSnelheid == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("snelheid", " = " + "'" + collection["snelheidKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkDriver == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("driver", " = " + "'" + collection["driverKeuzeTekst"].Split(',')[0] + "'"));
                }
                string query = "SELECT ";
                foreach (string s in keuzeKolommen)
                {
                    query += s + ", ";
                }
                query = query.Substring(0, query.Length - 2);
                query += " FROM TblNetwerk ";
                query += " WHERE ";
                foreach (var element in lijstConditie)
                {
                    query += element.Key + element.Value + " AND ";
                }
                query = query.Substring(0, query.Length - 4);
                query += " ;";
                ViewBag.Rapport = test.RapporteringNetwerk(query, keuzeKolommen.ToArray());

                foreach (string s in keuzeKolommen)
                {
                    ViewBag.tables += s + " ";
                    if (s.Equals("idNetwerk"))
                        ViewBag.test = s;
                    if (s.Equals("merk"))
                        ViewBag.test1 = s;
                    if (s.Equals("type"))
                        ViewBag.test2 = s;
                    if (s.Equals("snelheid"))
                        ViewBag.test4 = s;
                    if (s.Equals("driver"))
                        ViewBag.test3 = s; 
                }
                ViewBag.query = query;
            }

            return View("NetwerkRapportering");
        }

        [HttpPost]
        public ActionResult Stap3GrafischeKaart(FormCollection collection)
        {
            string request = Request["action"];
            string[] requestOplossing = request.Split('/');
            if (requestOplossing[0].Equals("OpslaanPdf"))
            {
                OpslaanPdfGrafischeKaart(requestOplossing[1], requestOplossing[2]);
                ViewBag.testj = requestOplossing[1] + requestOplossing[2];
            }
            if (requestOplossing[0].Equals("kolomKeuze"))
            {
                string resultaat = "";
                bool checkId = Convert.ToBoolean(collection["idGrafischeKaart"].Split(',')[0]);
                bool checkMerk = Convert.ToBoolean(collection["merk"].Split(',')[0]);
                bool checkType = Convert.ToBoolean(collection["type"].Split(',')[0]);
                bool checkDriver = Convert.ToBoolean(collection["driver"].Split(',')[0]);
                bool checkFabrieksnummer = Convert.ToBoolean(collection["fabrieksnummer"].Split(',')[0]);
                if (checkId == true)
                    resultaat += "idGrafischeKaart";
                if (checkMerk == true)
                    resultaat += " merk";
                if (checkType == true)
                    resultaat += " type";
                if (checkDriver == true)
                    resultaat += " driver";
                if (checkFabrieksnummer == true)
                    resultaat += " fabrieksNummer";
                ViewBag.styleCpuStap4 = "inline";
                ViewBag.resultaatCheck = resultaat;
            }
            if (requestOplossing[0].Equals("conditieKeuze"))
            {
                ViewBag.styleCpuStap5 = "inline";
                string keuzeKolom = requestOplossing[1];
                string[] tussen = keuzeKolom.Split(' ');
                List<string> keuzeKolommen = new List<string>();
                foreach (string s in tussen)
                {
                    if (!s.Equals(""))
                    {
                        keuzeKolommen.Add(s);
                    }
                }

                var lijstConditie = new List<KeyValuePair<string, string>>();
                bool checkMerk = Convert.ToBoolean(collection["merkKeuzeCheck"].Split(',')[0]);
                bool checkType = Convert.ToBoolean(collection["typeKeuzeCheck"].Split(',')[0]);
                bool checkDriver = Convert.ToBoolean(collection["driverKeuzeCheck"].Split(',')[0]);
                bool checkFabrieksnummer = Convert.ToBoolean(collection["fabrieksnummerKeuzeCheck"].Split(',')[0]);
                if (checkMerk == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("merk", " = " + "'" + collection["merkKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkType == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("type", " = " + "'" + collection["typeKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkDriver == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("driver", " = " + "'" + collection["driverKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkFabrieksnummer == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("fabrieksNummer", " = " + "'" + collection["fabrieksNummerKeuzeTekst"].Split(',')[0] + "'"));
                }
                string query = "SELECT ";
                foreach (string s in keuzeKolommen)
                {
                    query += s + ", ";
                }
                query = query.Substring(0, query.Length - 2);
                query += " FROM TblGrafischeKaart ";
                query += " WHERE ";
                foreach (var element in lijstConditie)
                {
                    query += element.Key + element.Value + " AND ";
                }
                query = query.Substring(0, query.Length - 4);
                query += " ;";
                ViewBag.Rapport = test.RapporteringGrafischeKaart(query, keuzeKolommen.ToArray());

                foreach (string s in keuzeKolommen)
                {
                    ViewBag.tables += s + " ";
                    if (s.Equals("idGrafischeKaart"))
                        ViewBag.test = s;
                    if (s.Equals("merk"))
                        ViewBag.test1 = s;
                    if (s.Equals("type"))
                        ViewBag.test2 = s;
                    if (s.Equals("driver"))
                        ViewBag.test3 = s;
                    if (s.Equals("fabrieksNummer"))
                        ViewBag.test4 = s;
                }
                ViewBag.query = query;
            }

            return View("GrafischeKaartRapportering");
        }

        [HttpPost]
        public ActionResult Stap3Leverancier(FormCollection collection)
        {
            string request = Request["action"];
            string[] requestOplossing = request.Split('/');
            if (requestOplossing[0].Equals("OpslaanPdf"))
            {
                OpslaanPdfLeverancier(requestOplossing[1], requestOplossing[2]);
                ViewBag.testj = requestOplossing[1] + requestOplossing[2];
            }
            if (requestOplossing[0].Equals("kolomKeuze"))
            {
                string resultaat = "";
                bool checkId = Convert.ToBoolean(collection["idLeverancier"].Split(',')[0]);
                bool checkNaam = Convert.ToBoolean(collection["naam"].Split(',')[0]);
                bool checkAfkorting = Convert.ToBoolean(collection["afkorting"].Split(',')[0]);
                bool checkStraat = Convert.ToBoolean(collection["straat"].Split(',')[0]);
                bool checkHuisNummer = Convert.ToBoolean(collection["huisNummer"].Split(',')[0]);
                bool checkBusNummer = Convert.ToBoolean(collection["busNummer"].Split(',')[0]);
                bool checkPostcode = Convert.ToBoolean(collection["postcode"].Split(',')[0]);
                bool checkTelefoon = Convert.ToBoolean(collection["telefoon"].Split(',')[0]);
                bool checkFax = Convert.ToBoolean(collection["fax"].Split(',')[0]);
                bool checkEmail = Convert.ToBoolean(collection["email"].Split(',')[0]);
                bool checkWebsite = Convert.ToBoolean(collection["website"].Split(',')[0]);
                bool checkBtwNummer = Convert.ToBoolean(collection["btwNummer"].Split(',')[0]);
                bool checkIban = Convert.ToBoolean(collection["iban"].Split(',')[0]);
                bool checkBic = Convert.ToBoolean(collection["bic"].Split(',')[0]);
                bool checkToegevoegdOp = Convert.ToBoolean(collection["toegevoegdOp"].Split(',')[0]);

                if (checkId == true)
                    resultaat += "idLeverancier";
                if (checkNaam == true)
                    resultaat += " naam";
                if (checkAfkorting == true)
                    resultaat += " afkorting";
                if (checkStraat == true)
                    resultaat += " straat";
                if (checkHuisNummer == true)
                    resultaat += " huisNummer";
                if (checkBusNummer == true)
                    resultaat += " busNummer";
                if (checkPostcode == true)
                    resultaat += " postcode";
                if (checkTelefoon == true)
                    resultaat += " telefoon";
                if (checkFax == true)
                    resultaat += " fax";
                if (checkEmail == true)
                    resultaat += " email";
                if (checkWebsite == true)
                    resultaat += " website";
                if (checkBtwNummer == true)
                    resultaat += " btwNummer";
                if (checkIban == true)
                    resultaat += " iban";
                if (checkBic == true)
                    resultaat += " bic";
                if (checkToegevoegdOp == true)
                    resultaat += " toegevoegdOp";
                ViewBag.styleCpuStap4 = "inline";
                ViewBag.resultaatCheck = resultaat;
            }
            if (requestOplossing[0].Equals("conditieKeuze"))
            {
                ViewBag.styleCpuStap5 = "inline";
                string keuzeKolom = requestOplossing[1];
                string[] tussen = keuzeKolom.Split(' ');
                List<string> keuzeKolommen = new List<string>();
                foreach (string s in tussen)
                {
                    if (!s.Equals(""))
                    {
                        keuzeKolommen.Add(s);
                    }
                }

                var lijstConditie = new List<KeyValuePair<string, string>>();
                bool checkNaam = Convert.ToBoolean(collection["naamKeuzeCheck"].Split(',')[0]);
                bool checkAfkorting = Convert.ToBoolean(collection["afkortingKeuzeCheck"].Split(',')[0]);
                bool checkStraat = Convert.ToBoolean(collection["straatKeuzeCheck"].Split(',')[0]);
                bool checkHuisNummer = Convert.ToBoolean(collection["huisNummerKeuzeCheck"].Split(',')[0]);
                bool checkBusNummer = Convert.ToBoolean(collection["busNummerKeuzeCheck"].Split(',')[0]);
                bool checkPostcode = Convert.ToBoolean(collection["postcodeKeuzeCheck"].Split(',')[0]);
                bool checkTelefoon = Convert.ToBoolean(collection["telefoonKeuzeCheck"].Split(',')[0]);
                bool checkFax = Convert.ToBoolean(collection["faxKeuzeCheck"].Split(',')[0]);
                bool checkEmail = Convert.ToBoolean(collection["emailKeuzeCheck"].Split(',')[0]);
                bool checkWebsite = Convert.ToBoolean(collection["websiteKeuzeCheck"].Split(',')[0]);
                bool checkBtwNummer = Convert.ToBoolean(collection["btwNummerKeuzeCheck"].Split(',')[0]);
                bool checkIban = Convert.ToBoolean(collection["ibanKeuzeCheck"].Split(',')[0]);
                bool checkBic = Convert.ToBoolean(collection["bicKeuzeCheck"].Split(',')[0]);
                bool checkToegevoegdOp = Convert.ToBoolean(collection["toegevoegdOpKeuzeCheck"].Split(',')[0]);
                if (checkNaam == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("naam", " = " + "'" + collection["naamKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkAfkorting == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("afkorting", " = " + "'" + collection["afkortingKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkStraat == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("straat", " = " + "'" + collection["straatKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkHuisNummer == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("huisNummer", " = " + "'" + collection["huisNummerKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkBusNummer == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("busNummer", " = " + "'" + collection["busNummerKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkPostcode == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("postcode", " = " + "'" + collection["postcodeKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkTelefoon == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("telefoon", " = " + "'" + collection["telefoonKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkFax == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("fax", " = " + "'" + collection["faxKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkEmail == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("email", " = " + "'" + collection["emailKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkWebsite == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("website", " = " + "'" + collection["websiteKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkBtwNummer == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("btwNummer", " = " + "'" + collection["btwNummerKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkIban == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("iban", " = " + "'" + collection["ibanKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkBic == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("bic", " = " + "'" + collection["bicKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkToegevoegdOp == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("toegevoegdOp", " = " + "'" + collection["toegevoegdOpKeuzeTekst"].Split(',')[0] + "'"));
                }
                string query = "SELECT ";
                foreach (string s in keuzeKolommen)
                {
                    query += s + ", ";
                }
                query = query.Substring(0, query.Length - 2);
                query += " FROM TblLeverancier ";
                query += " WHERE ";
                foreach (var element in lijstConditie)
                {
                    query += element.Key + element.Value + " AND ";
                }
                query = query.Substring(0, query.Length - 4);
                query += " ;";
                ViewBag.Rapport = test.RapporteringLeverancier(query, keuzeKolommen.ToArray());

                foreach (string s in keuzeKolommen)
                {
                    ViewBag.tables += s + " ";
                    if (s.Equals("idLeverancier"))
                        ViewBag.test = s;
                    if (s.Equals("naam"))
                        ViewBag.test1 = s;
                    if (s.Equals("afkorting"))
                        ViewBag.test2 = s;
                    if (s.Equals("straat"))
                        ViewBag.test3 = s;
                    if (s.Equals("huisNummer"))
                        ViewBag.test4 = s;
                    if (s.Equals("busNummer"))
                        ViewBag.test = s;
                    if (s.Equals("postcode"))
                        ViewBag.test1 = s;
                    if (s.Equals("telefoon"))
                        ViewBag.test2 = s;
                    if (s.Equals("fax"))
                        ViewBag.test3 = s;
                    if (s.Equals("email"))
                        ViewBag.test4 = s;
                    if (s.Equals("website"))
                        ViewBag.test = s;
                    if (s.Equals("btwNummer"))
                        ViewBag.test1 = s;
                    if (s.Equals("iban"))
                        ViewBag.test2 = s;
                    if (s.Equals("bic"))
                        ViewBag.test3 = s;
                    if (s.Equals("toegevoegdOp"))
                        ViewBag.test4 = s;
                }
                ViewBag.query = query;
            }

            return View("LeverancierRapportering");
        }

        [HttpPost]
        public ActionResult Stap3Harddisk(FormCollection collection)
        {
            string request = Request["action"];
            string[] requestOplossing = request.Split('/');
            if (requestOplossing[0].Equals("OpslaanPdf"))
            {
                OpslaanPdfHarddisk(requestOplossing[1], requestOplossing[2]);
                ViewBag.testj = requestOplossing[1] + requestOplossing[2];
            }
            if (requestOplossing[0].Equals("kolomKeuze"))
            {
                string resultaat = "";
                bool checkId = Convert.ToBoolean(collection["idHarddisk"].Split(',')[0]);
                bool checkMerk = Convert.ToBoolean(collection["merk"].Split(',')[0]);
                bool checkGrootte = Convert.ToBoolean(collection["grootte"].Split(',')[0]);
                bool checkFabrieksnummer = Convert.ToBoolean(collection["fabrieksnummer"].Split(',')[0]);
                if (checkId == true)
                    resultaat += "idHarddisk";
                if (checkMerk == true)
                    resultaat += " merk";
                if (checkGrootte == true)
                    resultaat += " grootte";
                if (checkFabrieksnummer == true)
                    resultaat += " fabrieksNummer";
                ViewBag.styleCpuStap4 = "inline";
                ViewBag.resultaatCheck = resultaat;
            }
            if (requestOplossing[0].Equals("conditieKeuze"))
            {
                ViewBag.styleCpuStap5 = "inline";
                string keuzeKolom = requestOplossing[1];
                string[] tussen = keuzeKolom.Split(' ');
                List<string> keuzeKolommen = new List<string>();
                foreach (string s in tussen)
                {
                    if (!s.Equals(""))
                    {
                        keuzeKolommen.Add(s);
                    }
                }

                var lijstConditie = new List<KeyValuePair<string, string>>();
                bool checkMerk = Convert.ToBoolean(collection["merkKeuzeCheck"].Split(',')[0]);
                bool checkGrootte = Convert.ToBoolean(collection["grootteKeuzeCheck"].Split(',')[0]);
                bool checkFabrieksnummer = Convert.ToBoolean(collection["fabrieksnummerKeuzeCheck"].Split(',')[0]);
                if (checkMerk == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("merk", " = " + "'" + collection["merkKeuzeTekst"].Split(',')[0] + "'"));
                }
                if (checkGrootte == true)
                {
                    string resultaat = collection["grootteKeuze"].Split(',')[0] + " " + collection["grootteKeuze1"].Split(',')[0];
                    lijstConditie.Add(new KeyValuePair<string, string>("grootte", " " + resultaat));
                }
                if (checkFabrieksnummer == true)
                {
                    lijstConditie.Add(new KeyValuePair<string, string>("fabrieksNummer", " = " + "'" + collection["fabrieksNummerKeuzeTekst"].Split(',')[0] + "'"));
                }
                string query = "SELECT ";
                foreach (string s in keuzeKolommen)
                {
                    query += s + ", ";
                }
                query = query.Substring(0, query.Length - 2);
                query += " FROM TblHarddisk ";
                query += " WHERE ";
                foreach (var element in lijstConditie)
                {
                    query += element.Key + element.Value + " AND ";
                }
                query = query.Substring(0, query.Length - 4);
                query += " ;";
                ViewBag.Rapport = test.RapporteringHarddisk(query, keuzeKolommen.ToArray());

                foreach (string s in keuzeKolommen)
                {
                    ViewBag.tables += s + " ";
                    if (s.Equals("idHarddisk"))
                        ViewBag.test = s;
                    if (s.Equals("merk"))
                        ViewBag.test1 = s;
                    if (s.Equals("grootte"))
                        ViewBag.test2 = s;
                    if (s.Equals("fabrieksNummer"))
                        ViewBag.test3 = s;
                }
                ViewBag.query = query;
            }

            return View("HarddiskRapportering");
        }



        public void OpslaanPdf(string stap1)
        {
            try
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();

                if (stap1.Equals("objectType"))
                {
                    PdfPTable table = new PdfPTable(2);
                    List<ObjectTypes> objectTypes = test.ObjectTypeGetAll().ToList();
                    foreach (var item in objectTypes)
                    {
                        table.AddCell(item.Id.ToString());
                        table.AddCell(item.Omschrijving);
                    }
                    pdfDoc.Add(table);
                }
                if (stap1.Equals("verzekering"))
                {
                    PdfPTable table = new PdfPTable(2);
                    List<Verzekering> verzekeringen = test.VerzekeringGetAll().ToList();
                    foreach (var item in verzekeringen)
                    {
                        table.AddCell(item.Id.ToString());
                        table.AddCell(item.Omschrijving);
                    }
                    pdfDoc.Add(table);
                }
                if (stap1.Equals("cpu"))
                {
                    PdfPTable table = new PdfPTable(5);
                    List<Cpu> cpus = test.CpuGetAll().ToList();
                    foreach (var item in cpus)
                    {
                        table.AddCell(item.IdCpu.ToString());
                        table.AddCell(item.Merk);
                        table.AddCell(item.Type);
                        table.AddCell(item.Snelheid.ToString());
                        table.AddCell(item.FabrieksNummer.ToString());
                    }
                    pdfDoc.Add(table);
                }
                if (stap1.Equals("device"))
                {
                    PdfPTable table = new PdfPTable(6);
                    List<Device> devices = test.DeviceGetAll().ToList();
                    foreach (var item in devices)
                    {
                        table.AddCell(item.IdDevice.ToString());
                        table.AddCell(item.Merk);
                        table.AddCell(item.Type);
                        table.AddCell(item.Serienummer);
                        table.AddCell(item.IsPcCompatibel.ToString());
                        table.AddCell(item.FabrieksNummer.ToString());
                    }
                    pdfDoc.Add(table);
                }
                if (stap1.Equals("factuur"))
                {
                    PdfPTable table = new PdfPTable(6);
                    pdfDoc.SetPageSize(PageSize.A4.Rotate());
                }
                if (stap1.Equals("grafischeKaart"))
                {
                    PdfPTable table = new PdfPTable(5);
                    List<GrafischeKaart> grafischeKaarten = test.GrafischeKaartGetAll().ToList();
                    foreach (var item in grafischeKaarten)
                    {
                        table.AddCell(item.IdGrafischeKaart.ToString());
                        table.AddCell(item.Merk);
                        table.AddCell(item.Type);
                        table.AddCell(item.Driver);
                        table.AddCell(item.FabrieksNummer.ToString());
                    }
                    pdfDoc.Add(table);
                }
                if (stap1.Equals("harddisk"))
                {
                    PdfPTable table = new PdfPTable(4);
                    List<Harddisk> harddisks = test.HarddiskGetAll().ToList();
                    foreach(var item in harddisks)
                    {
                        table.AddCell(item.idHarddisk.ToString());
                        table.AddCell(item.merk);
                        table.AddCell(item.grootte.ToString());
                        table.AddCell(item.fabrieksNummer.ToString());
                    }
                    pdfDoc.Add(table);
                }
                if (stap1.Equals("hardware"))
                {
                    PdfPTable table = new PdfPTable(5);
                    List<Hardware> hardwares = test.HardwareGetAll().ToList();
                    foreach (var item in hardwares)
                    {
                        table.AddCell(item.IdHardware.ToString());
                        table.AddCell(item.IdCpu.ToString());
                        table.AddCell(item.IdDevice.ToString());
                        table.AddCell(item.IdGrafischeKaart.ToString());
                        table.AddCell(item.IdHarddisk.ToString());
                    }
                    pdfDoc.Add(table);
                }
                if (stap1.Equals("inventaris"))
                {
                    PdfPTable table = new PdfPTable(10);
                    pdfDoc.SetPageSize(PageSize.A4.Rotate());
                    List<Inventaris> inventarissen = test.InventarisGetAll().ToList();
                    foreach (var item in inventarissen)
                    {
                        table.AddCell(item.id.ToString());
                        table.AddCell(item.label);
                        table.AddCell(item.idLokaal.ToString());
                        table.AddCell(item.idObject.ToString());
                        table.AddCell(item.aankoopjaar.ToString());
                        table.AddCell(item.afschrijvingsperiode.ToString());
                        table.AddCell(item.historiek);
                        table.AddCell(item.isActief.ToString());
                        table.AddCell(item.isAanwezig.ToString());
                        table.AddCell(item.idVerzekering.ToString());
                    }
                    pdfDoc.Add(table);
                }
                if (stap1.Equals("leverancier"))
                {
                    PdfPTable table = new PdfPTable(15);
                    List<Leverancier> leveranciers = test.LeverancierGetAll().ToList();
                    foreach (var item in leveranciers)
                    {
                        table.AddCell(item.idLeverancier.ToString());
                        table.AddCell(item.naam);
                        table.AddCell(item.afkorting);
                        table.AddCell(item.straat);
                        table.AddCell(item.huisNummer.ToString());
                        table.AddCell(item.busNummer.ToString());
                        table.AddCell(item.postcode.ToString());
                        table.AddCell(item.telefoon);
                        table.AddCell(item.fax);
                        table.AddCell(item.email.ToString());
                        table.AddCell(item.website.ToString());
                        table.AddCell(item.btwNummer.ToString());
                        table.AddCell(item.iban);
                        table.AddCell(item.bic);
                        table.AddCell(item.toegevoegdOp.ToString());
                    }
                    pdfDoc.Add(table);
                }
                if (stap1.Equals("lokaal"))
                {
                    PdfPTable table = new PdfPTable(5);
                    List<Lokaal> lokalen = test.LokaalGetAll().ToList();
                    foreach (var item in lokalen)
                    {
                        table.AddCell(item.idLokaal.ToString());
                        table.AddCell(item.lokaalNaam.ToString());
                        table.AddCell(item.aantalPlaatsen.ToString());
                        table.AddCell(item.isComputerLokaal.ToString());
                        table.AddCell(item.idNetwerk.ToString());
                    }
                    pdfDoc.Add(table);
                }
                if (stap1.Equals("netwerk"))
                {
                    PdfPTable table = new PdfPTable(5);
                    List<Netwerk> netwerken = test.NetwerkGetAll().ToList();
                    foreach (var item in netwerken)
                    {
                        table.AddCell(item.id.ToString());
                        table.AddCell(item.merk.ToString());
                        table.AddCell(item.type.ToString());
                        table.AddCell(item.snelheid.ToString());
                        table.AddCell(item.driver.ToString());
                    }
                    pdfDoc.Add(table);
                }
                if (stap1.Equals("object"))
                {
                    PdfPTable table = new PdfPTable(5);
                    List<CvoInventarisClient.ServiceReference.Object> objecten = test.ObjectGetAll().ToList();
                    foreach (var item in objecten)
                    {
                        table.AddCell(item.id.ToString());
                        table.AddCell(item.idObjectType.ToString());
                        table.AddCell(item.kenmerken.ToString());
                        table.AddCell(item.idLeverancier.ToString());
                        table.AddCell(item.idFactuur.ToString());
                    }
                    pdfDoc.Add(table);
                }
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Example.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }
        }

        public ActionResult OpslaanExcel()
        {

            return View("Index");
        }

       public void OpslaanPdfCpu(string query, string tables)
        {
            try
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                string[] tablesOplossingVoorlopig = tables.Split(' ');
                string[] tabllesOplossing = new string[tablesOplossingVoorlopig.Count()];
                int teller = 0;
                foreach (string s in tablesOplossingVoorlopig)
                {
                    if (!s.Equals(" ")) { 
                        tabllesOplossing[teller] = s; teller++; }
                }
                tabllesOplossing = tablesOplossingVoorlopig.Take(tablesOplossingVoorlopig.Count() - 1).ToArray();

                PdfPTable table = new PdfPTable(tabllesOplossing.Length);
                List<Cpu> cpuRapport = test.RapporteringCpu(query, tabllesOplossing).ToList();
                foreach (string s in tabllesOplossing)
                {
                    if (s.Equals("idCpu"))
                        table.AddCell(s);
                    if (s.Equals("merk"))
                        table.AddCell(s);
                    if (s.Equals("type"))
                        table.AddCell(s);
                    if (s.Equals("snelheid"))
                        table.AddCell(s);
                    if (s.Equals("fabrieksNummer"))
                        table.AddCell(s);
                }
                foreach (Cpu c in cpuRapport)
                {
                    if(c.IdCpu != 0)
                    {
                        table.AddCell(c.IdCpu.ToString());
                    }
                    if(c.Merk != null)
                    {
                        table.AddCell(c.Merk);
                    }
                    if(c.Type != null)
                    {
                        table.AddCell(c.Type);
                    }
                    if(c.Snelheid != 0)
                    {
                        table.AddCell(c.Snelheid.ToString());
                    }
                    if(c.FabrieksNummer != null)
                    {
                        table.AddCell(c.FabrieksNummer);
                    }
                }
                pdfDoc.Add(table);
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Example.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }
        }

        public void OpslaanPdfNetwerk(string query, string tables)
        {
            try
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                string[] tablesOplossingVoorlopig = tables.Split(' ');
                string[] tabllesOplossing = new string[tablesOplossingVoorlopig.Count()];
                int teller = 0;
                foreach (string s in tablesOplossingVoorlopig)
                {
                    if (!s.Equals(" "))
                    {
                        tabllesOplossing[teller] = s; teller++;
                    }
                }
                tabllesOplossing = tablesOplossingVoorlopig.Take(tablesOplossingVoorlopig.Count() - 1).ToArray();

                PdfPTable table = new PdfPTable(tabllesOplossing.Length);
                List<Netwerk> Rapport = test.RapporteringNetwerk(query, tabllesOplossing).ToList();
                foreach (string s in tabllesOplossing)
                {
                    if (s.Equals("idNetwerk"))
                        table.AddCell(s);
                    if (s.Equals("merk"))
                        table.AddCell(s);
                    if (s.Equals("type"))
                        table.AddCell(s);
                    if (s.Equals("snelheid"))
                        table.AddCell(s);
                    if (s.Equals("driver"))
                        table.AddCell(s);
                }
                foreach (Netwerk n in Rapport)
                {
                    if (n.id != 0)
                    {
                        table.AddCell(n.id.ToString());
                    }
                    if (n.merk != null)
                    {
                        table.AddCell(n.merk);
                    }
                    if (n.type != null)
                    {
                        table.AddCell(n.type);
                    }
                    if (n.snelheid != null)
                    {
                        table.AddCell(n.snelheid.ToString());
                    }
                    if (n.driver != null)
                    {
                        table.AddCell(n.driver);
                    }
                }
                pdfDoc.Add(table);
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Example.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }
        }

        public void OpslaanPdfDevice(string query,string tables)
        {
            try
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                string[] tablesOplossingVoorlopig = tables.Split(' ');
                string[] tabllesOplossing = new string[tablesOplossingVoorlopig.Count()];
                int teller = 0;
                foreach (string s in tablesOplossingVoorlopig)
                {
                    if (!s.Equals(" "))
                    {
                        tabllesOplossing[teller] = s; teller++;
                    }
                }
                tabllesOplossing = tablesOplossingVoorlopig.Take(tablesOplossingVoorlopig.Count() - 1).ToArray();

                PdfPTable table = new PdfPTable(tabllesOplossing.Length);
                List<Device> Rapport = test.RapporteringDevice(query, tabllesOplossing).ToList();
                foreach (string s in tabllesOplossing)
                {
                        ViewBag.tables += s + " ";
                        if (s.Equals("idDevice"))
                        table.AddCell(s);
                    if (s.Equals("merk"))
                        table.AddCell(s);
                    if (s.Equals("type"))
                        table.AddCell(s);
                    if (s.Equals("serienummer"))
                        table.AddCell(s);
                    if (s.Equals("isPcCompatibel"))
                        table.AddCell(s);
                    if (s.Equals("fabrieksNummer"))
                        table.AddCell(s);

                }
                foreach (Device d in Rapport)
                {
                    if (d.IdDevice != 0)
                    {
                        table.AddCell(d.IdDevice.ToString());
                    }
                    if (d.Merk != null)
                    {
                        table.AddCell(d.Merk);
                    }
                    if (d.Type != null)
                    {
                        table.AddCell(d.Type);
                    }
                    if (d.Serienummer != null)
                    {
                        table.AddCell(d.Serienummer.ToString());
                    }
                    if (d.IsPcCompatibel != null)
                    {
                        table.AddCell(d.IsPcCompatibel.ToString());
                    }
                    if (d.FabrieksNummer != null)
                    {
                        table.AddCell(d.FabrieksNummer);
                    }
                }
                pdfDoc.Add(table);
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Example.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }
        }

        public void OpslaanPdfGrafischeKaart(string query, string tables)
        {
            try
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                string[] tablesOplossingVoorlopig = tables.Split(' ');
                string[] tabllesOplossing = new string[tablesOplossingVoorlopig.Count()];
                int teller = 0;
                foreach (string s in tablesOplossingVoorlopig)
                {
                    if (!s.Equals(" "))
                    {
                        tabllesOplossing[teller] = s; teller++;
                    }
                }
                tabllesOplossing = tablesOplossingVoorlopig.Take(tablesOplossingVoorlopig.Count() - 1).ToArray();

                PdfPTable table = new PdfPTable(tabllesOplossing.Length);
                List<GrafischeKaart> Rapport = test.RapporteringGrafischeKaart(query, tabllesOplossing).ToList();
                foreach (string s in tabllesOplossing)
                {
                    ViewBag.tables += s + " ";
                    if (s.Equals("idGrafischeKaart"))
                        table.AddCell(s);
                    if (s.Equals("merk"))
                        table.AddCell(s);
                    if (s.Equals("type"))
                        table.AddCell(s);
                    if (s.Equals("driver"))
                        table.AddCell(s);
                    if (s.Equals("fabrieksNummer"))
                        table.AddCell(s);
                }
                foreach (GrafischeKaart g in Rapport)
                {
                    if (g.IdGrafischeKaart != 0)
                    {
                        table.AddCell(g.IdGrafischeKaart.ToString());
                    }
                    if (g.Merk != null)
                    {
                        table.AddCell(g.Merk);
                    }
                    if (g.Type != null)
                    {
                        table.AddCell(g.Type);
                    }
                    if (g.Driver != null)
                    {
                        table.AddCell(g.Driver.ToString());
                    }
                    if (g.FabrieksNummer != null)
                    {
                        table.AddCell(g.FabrieksNummer.ToString());
                    }
                }
                pdfDoc.Add(table);
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Example.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }
        }

        public void OpslaanPdfHarddisk(string query,string tables) {
            try
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                string[] tablesOplossingVoorlopig = tables.Split(' ');
                string[] tabllesOplossing = new string[tablesOplossingVoorlopig.Count()];
                int teller = 0;
                foreach (string s in tablesOplossingVoorlopig)
                {
                    if (!s.Equals(" "))
                    {
                        tabllesOplossing[teller] = s; teller++;
                    }
                }
                tabllesOplossing = tablesOplossingVoorlopig.Take(tablesOplossingVoorlopig.Count() - 1).ToArray();

                PdfPTable table = new PdfPTable(tabllesOplossing.Length);
                List<Harddisk> Rapport = test.RapporteringHarddisk(query, tabllesOplossing).ToList();
                foreach (string s in tabllesOplossing)
                {
                    ViewBag.tables += s + " ";
                    if (s.Equals("idHarddisk"))
                        table.AddCell(s);
                    if (s.Equals("merk"))
                        table.AddCell(s);
                    if (s.Equals("grootte"))
                        table.AddCell(s);
                    if (s.Equals("fabrieksNummer"))
                        table.AddCell(s);
                }
                foreach (Harddisk h in Rapport)
                {
                    if (h.idHarddisk != 0)
                    {
                        table.AddCell(h.idHarddisk.ToString());
                    }
                    if (h.merk != null)
                    {
                        table.AddCell(h.merk);
                    }
                    if (h.grootte != 0)
                    {
                        table.AddCell(h.grootte.ToString());
                    }
                    if (h.fabrieksNummer != null)
                    {
                        table.AddCell(h.fabrieksNummer.ToString());
                    }
                }
                pdfDoc.Add(table);
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Example.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }
        }

        public void OpslaanPdfLeverancier(string query, string tables)
        {
            try
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                string[] tablesOplossingVoorlopig = tables.Split(' ');
                string[] tabllesOplossing = new string[tablesOplossingVoorlopig.Count()];
                int teller = 0;
                foreach (string s in tablesOplossingVoorlopig)
                {
                    if (!s.Equals(" "))
                    {
                        tabllesOplossing[teller] = s; teller++;
                    }
                }
                tabllesOplossing = tablesOplossingVoorlopig.Take(tablesOplossingVoorlopig.Count() - 1).ToArray();

                PdfPTable table = new PdfPTable(tabllesOplossing.Length);
                List<Leverancier> Rapport = test.RapporteringLeverancier(query, tabllesOplossing).ToList();
                foreach (string s in tabllesOplossing)
                {
                    ViewBag.tables += s + " ";
                    if (s.Equals("idLeverancier"))
                        table.AddCell(s);
                    if (s.Equals("naam"))
                        table.AddCell(s);
                    if (s.Equals("afkorting"))
                        table.AddCell(s);
                    if (s.Equals("straat"))
                        table.AddCell(s);
                    if (s.Equals("huisNummer"))
                        table.AddCell(s);
                    if (s.Equals("busNummer"))
                        table.AddCell(s);
                    if (s.Equals("postcode"))
                        table.AddCell(s);
                    if (s.Equals("telefoon"))
                        table.AddCell(s);
                    if (s.Equals("fax"))
                        table.AddCell(s);
                    if (s.Equals("email"))
                        table.AddCell(s);
                    if (s.Equals("website"))
                        table.AddCell(s);
                    if (s.Equals("btwNummer"))
                        table.AddCell(s);
                    if (s.Equals("iban"))
                        table.AddCell(s);
                    if (s.Equals("bic"))
                        table.AddCell(s);
                    if (s.Equals("toegevoegdOp"))
                        table.AddCell(s);
                }
                foreach (Leverancier l in Rapport)
                {
                    if (l.idLeverancier != 0)
                    {
                        table.AddCell(l.idLeverancier.ToString());
                    }
                    if (l.naam != null)
                    {
                        table.AddCell(l.naam);
                    }
                    if (l.afkorting != null)
                    {
                        table.AddCell(l.afkorting.ToString());
                    }
                    if (l.straat != null)
                    {
                        table.AddCell(l.straat.ToString());
                    }
                    if (l.huisNummer != 0)
                    {
                        table.AddCell(l.huisNummer.ToString());
                    }
                    if (l.busNummer != 0)
                    {
                        table.AddCell(l.busNummer.ToString());
                    }
                    if (l.postcode != 0)
                    {
                        table.AddCell(l.postcode.ToString());
                    }
                    if (l.telefoon != null)
                    {
                        table.AddCell(l.telefoon.ToString());
                    }
                    if (l.fax != null)
                    {
                        table.AddCell(l.fax.ToString());
                    }
                    if (l.email != null)
                    {
                        table.AddCell(l.email);
                    }
                    if (l.website != null)
                    {
                        table.AddCell(l.website.ToString());
                    }
                    if (l.btwNummer != null)
                    {
                        table.AddCell(l.btwNummer.ToString());
                    }
                    if (l.iban != null)
                    {
                        table.AddCell(l.iban);
                    }
                    if (l.bic != null)
                    {
                        table.AddCell(l.bic.ToString());
                    }
                    if (l.toegevoegdOp != new DateTime(0001, 01, 1).Add(new TimeSpan(0, 00, 00)))
                    {
                        table.AddCell(l.toegevoegdOp.ToString());
                    }
                }
                pdfDoc.Add(table);
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Example.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }
        }
    }
}