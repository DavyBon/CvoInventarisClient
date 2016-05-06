using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class LeverancierModel
    {
        public int IdLeverancier { get; set; }

        [Required(ErrorMessage = "Vul een in")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Een naam bestaat uit minimum 1 en maximum 255 characters")]
        [Display(Name = "naam")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Vul een afkorting in")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Een afkorting bestaat uit minimum 1 en maximum 10 characters")]
        [Display(Name = "afkorting")]
        public string Afkorting { get; set; }

        [Required(ErrorMessage = "Vul een straat in")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Een straat bestaat uit minimum 1 en maximum 255 characters")]
        [Display(Name = "straat")]
        public string Straat { get; set; }

        [Required(ErrorMessage = "Vul een huisnummer in")]
        [Range(1, 10000, ErrorMessage = "Een huisnummer moet tussen 1 en 10000 liggen")]
        [Display(Name = "huisNummer")]
        public int HuisNummer { get; set; }

        [Required(ErrorMessage = "Vul een busnummer in")]
        [Range(1, 10000, ErrorMessage = "Een busnummer moet tussen 1 en 10000 liggen")]
        [Display(Name = "busNummer")]
        public int BusNummer { get; set; }

        [Required(ErrorMessage = "Vul een postcode in")]
        [Range(1, 10000, ErrorMessage = "Een postcode moet tussen 1 en 10000 liggen")]
        [Display(Name = "postcode")]
        public int Postcode { get; set; }

        [Required(ErrorMessage = "Vul een telefoon in")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een telefoonnummer bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "telefoon")]
        public string Telefoon { get; set; }

        [Required(ErrorMessage = "Vul een fax in")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een faxnummer bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "fax")]
        public string Fax { get; set; }

        // DataType.EmailAddress: displays the string as a clickable emailaddress
        [Required(ErrorMessage = "Vul een emailadres in")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Vul een correct emailadres in")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een emailadres bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vul een website in")]
        [DataType(DataType.Url, ErrorMessage = "Vul een correcte website in")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een website bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "website")]
        public string Website { get; set; }

        [Required(ErrorMessage = "Vul een btwnummer in")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een btwnummer bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "btwNummer")]
        public string BtwNummer { get; set; }

        [Required(ErrorMessage = "Vul een iban nummer in")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een iban nummer bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "iban")]
        public string Iban { get; set; }

        [Required(ErrorMessage = "Vul een bic nummer in")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een bic nummer bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "bic")]
        public string Bic { get; set; }

        [Required(ErrorMessage = "Vul een datum in")]
        [DataType(DataType.Date, ErrorMessage = "Vul een correcte datum in in dd/mm/yyyy notatie")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "toegevoegdOp")]
        public DateTime ToegevoegdOp { get; set; }
    }
}