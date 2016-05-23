using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Models
{
    public class LeverancierViewModel : ICloneable
    {
        public List<LeverancierModel> Leveranciers { get; set; }
        public List<SelectListItem> Postcodes { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}