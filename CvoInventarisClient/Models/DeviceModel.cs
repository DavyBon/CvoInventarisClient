using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class DeviceModel
    {
        public int IdDevice { get; set; }
        public string Merk { get; set; }
        public string Type { get; set; }
        public string Serienummer { get; set; }
        public bool IsPcCompatibel { get; set; }
        public string FabrieksNummer { get; set; }
    }
}