using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class CpuModel
    {
        public int IdCpu { get; set; }
        public string Merk { get; set; }
        public string Type { get; set; }
        public int Snelheid { get; set; }
        public string FabrieksNummer { get; set; }
    }
}