using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class NetwerkModel
    {
        public int Id { get; set; }
        public string Driver { get; set; }
        public string Merk { get; set; }
        public string  Snelheid { get; set; }
        public string Type { get; set; }
    }
}