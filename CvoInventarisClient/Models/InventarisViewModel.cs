using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Models
{
    public class InventarisViewModel
    {
        public List<InventarisModel> Inventaris { get; set; }
        public List<ObjectModel> Objecten { get; set; }
        public List<SelectListItem> Verzekeringen { get; set; }
        public List<SelectListItem> Lokalen { get; set; }
    }
}