using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CvoInventarisClient.Models
{
    public class CampusModel
    {
        public int IdCampus { get; set; }

        [Required(ErrorMessage = "Vul een naam in")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een naam bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "naam")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Vul een postcode in")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een postcode bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "postcode")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Vul een straat in")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een straat bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "straat")]
        public string Straat { get; set; }

        [Required(ErrorMessage = "Vul een nummer in")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een nummer bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "nummer")]
        public string Nummer { get; set; }
    }
}