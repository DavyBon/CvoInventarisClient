using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class InventarisViewModel
    {
        public List<InventarisModel> Inventaris { get; set; }
        public List<ObjectModel> Objecten { get; set; }
        public List<VerzekeringModel> Verzekeringen { get; set; }
        public List<LokaalModel> Lokalen { get; set; }
    }
}