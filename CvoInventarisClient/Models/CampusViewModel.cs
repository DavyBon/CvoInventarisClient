using CvoInventarisClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Models
{
    public class CampusViewModel : ICloneable
    {
        public List<CampusModel> Campussen { get; set;}
        public List<SelectListItem> Postcodes { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}