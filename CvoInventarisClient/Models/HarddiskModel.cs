using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class HarddiskModel
    {

        public int IdHarddisk { get; set; }

        [Required(ErrorMessage = "Vul een merk in")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Een merk bestaat uit minimum 2 en maximum 50 characters")]
        [Display(Name = "merk")]
        public string Merk { get; set; }

        [Required(ErrorMessage = "Vul een grootte in")]
        [Range(1, 100000, ErrorMessage = "De grootte moet tussen 1 en 100000 liggen")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Vul een nummer in als grootte")]
        [Display(Name = "grootte")]
        public int Grootte { get; set; }

        [Required(ErrorMessage = "Vul een fabrieksnummer in")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Een fabrieksnummer bestaat uit minimum 2 en maximum 255 characters")]
        [Display(Name = "fabrieksnummer")]
        public string FabrieksNummer { get; set; }
    }
}