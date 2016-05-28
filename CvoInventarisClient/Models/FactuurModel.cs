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

        [Display(Name = "Cvo-factuurnummer")]
        public string CvoFactuurNummer { get; set; }

        [Display(Name = "Leverancier-factuurnummer")]
        public string LeverancierFactuurNummer { get; set; }

        [Display(Name = "Verwerkingsdatum")]
        public string VerwerkingsDatum { get; set; }

        [Display(Name = "Scholengroepnummer")]
        public string ScholengroepNummer { get; set; }

        [Display(Name = "Leverancier")]
        public LeverancierModel Leverancier { get; set; }

        [DisplayFormat(DataFormatString = "€ {0:n}")]
        public decimal? Prijs { get; set; }

        [Display(Name = "Garantie")]
        public int? Garantie { get; set; }

        [Display(Name = "Omschrijving")]
        public string Omschrijving { get; set; }

        [Display(Name = "Afschrijfperiode")]
        public int? Afschrijfperiode { get; set; }

        public override string ToString()
        {
            return this.CvoFactuurNummer;
        }
    }
}