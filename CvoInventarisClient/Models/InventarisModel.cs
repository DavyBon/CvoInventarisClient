using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class InventarisModel
    {
        public int Id { get; set; }
        [Display(Name = "Lokaal")]
        public LokaalModel Lokaal { get; set; }
        [Display(Name = "Object")]
        public ObjectModel Object { get; set; }
        [Display(Name = "Verzekering")]
        public VerzekeringModel Verzekering { get; set; }
        [Display(Name = "Aanwezig")]
        public bool IsAanwezig { get; set; }
        [Display(Name = "Actief")]
        public bool IsActief { get; set; }
        [Display(Name = "Label")]
        public string Label { get; set; }
        [Display(Name = "Historiek")]
        public string Historiek { get; set; }
        [Display(Name = "Aankoopjaar")]
        public int Aankoopjaar { get; set; }
        [Display(Name = "Afschrijvingsperiode")]
        public int Afschrijvingsperiode { get; set; }


        public override string ToString()
        {
            return this.Label;
        }
    }
}