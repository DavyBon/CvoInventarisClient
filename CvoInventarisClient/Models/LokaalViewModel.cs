using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Models
{
    public class LokaalViewModel : ICloneable
    {
        public List<LokaalModel> Lokalen { get; set; }
        public List<SelectListItem> Campussen { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}