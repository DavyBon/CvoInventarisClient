using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class HarddiskModel
    {
        public int idHarddisk { get; set; }
        public string merk { get; set; }
        public int grootte { get; set; }
        public string fabrieksNummer { get; set; }
    }
}