using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Models
{
    public class FactuurViewModel:ICloneable
    {
        public List<FactuurModel> Facturen { get; set; }
        public List<SelectListItem> Leveranciers { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}