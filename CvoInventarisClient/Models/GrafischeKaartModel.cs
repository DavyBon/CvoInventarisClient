using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class GrafischeKaartModel
    {
        public int IdGrafischeKaart { get; set; }
        public string Merk { get; set; }
        public string Type { get; set; }
        public string Driver { get; set; }
        public string FabrieksNummer { get; set; }
    }
}