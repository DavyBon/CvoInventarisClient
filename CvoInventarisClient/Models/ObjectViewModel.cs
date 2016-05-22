using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Models
{
    public class ObjectViewModel:ICloneable
    {
        public List<ObjectModel> Objecten { get; set; }
        public List<SelectListItem> Facturen { get; set; }
        public List<SelectListItem> ObjectTypes { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}