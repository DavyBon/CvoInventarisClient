using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class FactuurModel
    {
        public int IdFactuur { get; set; }
        [Display(Name = "boekjaar")]
        public string Boekjaar { get; set; }
        [Display(Name = "cvoVolgNummer")]
        public string CvoVolgNummer { get; set; }
        [Display(Name = "factuurNummer")]
        public string FactuurNummer { get; set; }
        [Display(Name = "factuurDatum")]
        public DateTime FactuurDatum { get; set; }
        [Display(Name = "factuurStatusGetekend")]
        public bool FactuurStatusGetekend { get; set; }
        [Display(Name = "verwerkingsDatum")]
        public DateTime VerwerkingsDatum { get; set; }
        [Display(Name = "idLeverancier")]
        public LeverancierModel Leverancier { get; set; }
        [Display(Name = "prijs")]
        public int Prijs { get; set; }
        [Display(Name = "garantie")]
        public int Garantie { get; set; }
        [Display(Name = "omschrijving")]
        public string Omschrijving { get; set; }
        [Display(Name = "opmerking")]
        public string Opmerking { get; set; }
        [Display(Name = "afschrijfperiode")]
        public int Afschrijfperiode { get; set; }
        [Display(Name = "oleDoc")]
        public string OleDoc { get; set; }
        [Display(Name = "oleDocPath")]
        public string OleDocPath { get; set; }
        [Display(Name = "oleDocFileName")]
        public string OleDocFileName { get; set; }
        [Display(Name = "datumInsert")]
        public DateTime DatumInsert { get; set; }
        [Display(Name = "userInsert")]
        public string UserInsert { get; set; }
        [Display(Name = "datumModified")]
        public DateTime DatumModified { get; set; }
        [Display(Name = "userModified")]
        public string UserModified { get; set; }
    }
}