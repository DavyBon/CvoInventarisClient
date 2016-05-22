using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CvoInventarisClient.Models
{
    public class CampusModel : ICloneable
    {
        public int IdCampus { get; set; }

        [Display(Name = "naam")]
        public string Naam { get; set; }

        [Display(Name = "postcode")]
        public string Postcode { get; set; }

        [Display(Name = "straat")]
        public string Straat { get; set; }

        [Display(Name = "nummer")]
        public string Nummer { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}