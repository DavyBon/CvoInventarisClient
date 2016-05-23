﻿using System;
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

        [Display(Name = "afkorting")]
        public string Afkorting { get; set; }

        [Display(Name = "straat")]
        public string Straat { get; set; }

        [Display(Name = "huisNummer")]
        public string HuisNummer { get; set; }

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

        [Display(Name = "iban")]
        public string Iban { get; set; }

        [Display(Name = "bic")]
        public string Bic { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "toegevoegdOp")]
        public string ToegevoegdOp { get; set; }


        public override string ToString()
        {
            return this.Naam;
        }
    }
}