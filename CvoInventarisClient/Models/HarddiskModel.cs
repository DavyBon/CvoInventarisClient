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
        [Display(Name = "merk")]
        public string Merk { get; set; }
        [Display(Name = "grootte")]
        public int Grootte { get; set; }
        [Display(Name = "fabriekNummer")]
        public string FabrieksNummer { get; set; }
    }
}