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

        // RegularExpression: The entered value has to match the regular expression (example: for a value between 1000 and 2999: (^[12][0-9]{3}$))
        // ^        = matchcheck starts at the start of the entered value
        // [12]     = first value can either be 1 or 2 
        // [0-9]{3} = next 3 values can be numbers between 0 and 9    
        // $        = matchcheck ends at the end of the entered value
        [Required(ErrorMessage = "Vul een boekjaar in")]
        [RegularExpression("^[12][0-9]{3}$", ErrorMessage = "Een boekjaar moet tussen 1000 en 2999 liggen")]
        [Display(Name = "boekjaar")]
        public string Boekjaar { get; set; }

        [Required(ErrorMessage = "Vul een CVO volgnummer in")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een CVO volgnummer bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "cvoVolgNummer")]
        public string CvoVolgNummer { get; set; }

        [Required(ErrorMessage = "Vul een factuurnummer in")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een factuurnummer bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "FactuurNummer")]
        public string FactuurNummer { get; set; }

        
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Een scholengroep nummer bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "scholengroepNummer")]
        public string ScholengroepNummer { get; set; }

        // DataType: specifies the name of an additional type to associate with a data field
        // DisplayFormat: specifies how the data should be displayed (example "{0:d}": 01/01/2001 00:00 becomes 01/01/2001)
        [Required(ErrorMessage = "Vul een factuurdatum in")]
        [DataType(DataType.Date, ErrorMessage = "Vul een correcte datum in in dd/mm/yyyy notatie")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "factuurDatum")]
        public DateTime FactuurDatum { get; set; }

        [Required(ErrorMessage = "Kies een leverancier")]
        [Display(Name = "Leverancier")]
        public LeverancierModel Leverancier { get; set; }

        // DataType.Currency: Displays the integer as a currency (example: 100 becomes €100,00)
        // Web.config: <globalization culture="nl-BE"/> adds the € sign in front of the integer
        [Required(ErrorMessage = "Vul een prijs in")]
        [DataType(DataType.Currency)]
        [Range(1, 1000000, ErrorMessage = "De prijs moet tussen 1 en 1000000 liggen")]
        [Display(Name = "prijs")]
        public decimal Prijs { get; set; }

        [Required(ErrorMessage = "Vul een garantie in")]
        [Range(1, 100, ErrorMessage = "De garantie moet tussen 1 en 100 liggen")]
        [Display(Name = "garantie")]
        public int Garantie { get; set; }

        [Required(ErrorMessage = "Vul een omschrijving in")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Een omschrijving bestaat uit minimum 1 en maximum 255 characters")]
        [Display(Name = "omschrijving")]
        public string Omschrijving { get; set; }

        [Required(ErrorMessage = "Vul een opmerking in")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Een opmerking bestaat uit minimum 1 en maximum 255 characters")]
        [Display(Name = "opmerking")]
        public string Opmerking { get; set; }

        [Required(ErrorMessage = "Vul een afschrijfperiode in")]
        [Range(1, 100, ErrorMessage = "De afschrijfperiode moet tussen 1 en 100 liggen")]
        [Display(Name = "afschrijfperiode")]
        public int Afschrijfperiode { get; set; }

        [Required(ErrorMessage = "Vul een insertdatum in")]
        [DataType(DataType.Date, ErrorMessage = "Vul een correcte datum in in dd/mm/yyyy notatie")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "datumInsert")]
        public DateTime DatumInsert { get; set; }

        [Required(ErrorMessage = "Vul de naam in van de user die deze insert ingeeft")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "De naam van een user bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "userInsert")]
        public string UserInsert { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Vul een correcte datum in in dd/mm/yyyy notatie")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "datumModified")]
        public DateTime DatumModified { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "De naam van een user bestaat uit minimum 1 en maximum 50 characters")]
        [Display(Name = "userModified")]
        public string UserModified { get; set; }
    }
}