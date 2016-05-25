using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class FactuurModel
    {
        public int? Id { get; set; }

        [Display(Name = "cvo-factuurnummer")]
        public string CvoFactuurNummer { get; set; }

        [Display(Name = "leverancier-factuurnummer")]
        public string LeverancierFactuurNummer { get; set; }

        [Display(Name = "verwerkingsdatum")]
        public string VerwerkingsDatum { get; set; }

        [Display(Name = "scholengroepnummer")]
        public string ScholengroepNummer { get; set; }

        [Display(Name = "leverancier")]
        public LeverancierModel Leverancier { get; set; }

        [Display(Name = "prijs(€)")]
        public string Prijs { get; set; }

        [Display(Name = "garantie")]
        public int Garantie { get; set; }

        [Display(Name = "omschrijving")]
        public string Omschrijving { get; set; }

        [Display(Name = "afschrijfperiode")]
        public int Afschrijfperiode { get; set; }

        public override string ToString()
        {
            return this.CvoFactuurNummer;
        }
    }
}
}