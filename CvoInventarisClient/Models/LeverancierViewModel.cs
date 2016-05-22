using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class LeverancierViewModel : ICloneable
    {
        public List<LeverancierModel> Leveranciers { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}