using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class LokaalModel
    {
        public int IdLokaal { get; set; }
        public string LokaalNaam { get; set; }
        public int AantalPlaatsen { get; set; }
        public bool IsComputerLokaal { get; set; }
        public NetwerkModel Netwerk { get; set; }
        //public int IdNetwerk { get; set; }
    }
}