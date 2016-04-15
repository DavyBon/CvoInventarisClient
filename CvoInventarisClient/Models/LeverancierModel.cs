using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class LeverancierModel
    {
        public int idLeverancier { get; set; }
        public string naam { get; set; }
        public string afkorting { get; set; }
        public string straat { get; set; }
        public int huisNummer { get; set; }
        public int busNummer { get; set; }
        public int postcode { get; set; }
        public string telefoon { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string btwNummer { get; set; }
        public string iban { get; set; }
        public string bic { get; set; }
        public DateTime toegevoegdOp { get; set; }
    }
}