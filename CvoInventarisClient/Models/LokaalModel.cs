using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class LokaalModel
    {
        public int idLokaal { get; set; }
        public string lokaalNaam { get; set; }
        public int aantalPlaatsen { get; set; }
        public bool isComputerLokaal { get; set; }
        public int idNetwerk { get; set; }
    }
}