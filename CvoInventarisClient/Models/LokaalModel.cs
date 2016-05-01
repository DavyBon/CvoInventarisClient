﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class LokaalModel
    {
        public int IdLokaal { get; set; }
        [Display(Name = "LokaalNaam")]
        public string LokaalNaam { get; set; }
        [Display(Name = "aantalPlaatsen")]
        public int AantalPlaatsen { get; set; }
        [Display(Name = "isComputerLokaal")]
        public bool IsComputerLokaal { get; set; }
        [Display(Name = "idNetwerk")]
        public NetwerkModel Netwerk { get; set; }
    }
}