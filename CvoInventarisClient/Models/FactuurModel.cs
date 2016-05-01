using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class FactuurModel
    {
        public int IdFactuur { get; set; }
        public string Boekjaar { get; set; }
        public string CvoVolgNummer { get; set; }
        public string FactuurNummer { get; set; }
        public DateTime FactuurDatum { get; set; }
        public bool FactuurStatusGetekend { get; set; }
        public DateTime VerwerkingsDatum { get; set; }
        public LeverancierModel Leverancier { get; set; }
        //public int IdLeverancier { get; set; }
        public int Prijs { get; set; }
        public int Garantie { get; set; }
        public string Omschrijving { get; set; }
        public string Opmerking { get; set; }
        public int Afschrijfperiode { get; set; }
        public string OleDoc { get; set; }
        public string OleDocPath { get; set; }
        public string OleDocFileName { get; set; }
        public DateTime DatumInsert { get; set; }
        public string UserInsert { get; set; }
        public DateTime DatumModified { get; set; }
        public string UserModified { get; set; }
    }
}