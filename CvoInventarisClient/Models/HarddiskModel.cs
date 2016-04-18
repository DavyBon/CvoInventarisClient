using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class HarddiskModel
    {
        public int IdHarddisk { get; set; }
        public string Merk { get; set; }
        public int Grootte { get; set; }
        public string FabrieksNummer { get; set; }
    }
}