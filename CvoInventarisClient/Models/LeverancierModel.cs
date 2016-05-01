using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class LeverancierModel
    {
        public int IdLeverancier { get; set; }
        public string Naam { get; set; }
        public string Afkorting { get; set; }
        public string Straat { get; set; }
        public int HuisNummer { get; set; }
        public int BusNummer { get; set; }
        public int Postcode { get; set; }
        public string Telefoon { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string BtwNummer { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
        public DateTime ToegevoegdOp { get; set; }
    }
}