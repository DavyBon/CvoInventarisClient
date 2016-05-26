using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class LeverancierModel
    {
        public int? Id { get; set; }

        [Display(Name = "naam")]
        public string Naam { get; set; }

        [Display(Name = "straat")]
        public string Straat { get; set; }

        [Display(Name = "straatNummer")]
        public string StraatNummer { get; set; }

        [Display(Name = "busNummer")]
        public string BusNummer { get; set; }

        [Display(Name = "postcode")]
        public PostcodeModel Postcode { get; set; }

        [Display(Name = "telefoon")]
        public string Telefoon { get; set; }

        [Display(Name = "fax")]
        public string Fax { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "email")]
        public string Email { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "website")]
        public string Website { get; set; }

        [Display(Name = "btwNummer")]
        public string BtwNummer { get; set; }

        [Display(Name = "Actief datum")]
        public string ActiefDatum { get; set; }

        public override string ToString()
        {
            return this.Naam;
        }
    }
}