using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Models
{
    public class InventarisViewModel:ICloneable
    {
        public List<InventarisModel> Inventaris { get; set; }
        public List<SelectListItem> Objecten { get; set; }
        public List<SelectListItem> Verzekeringen { get; set; }
        public List<SelectListItem> Lokalen { get; set; }
        public List<SelectListItem> Facturen { get; set; }
        public List<SelectListItem> Objecttypen { get; set; }
        public List<SelectListItem> Campussen { get; set; }
        public List<SelectListItem> Leverancieren { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}