using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class VerzekeringViewModel : ICloneable
    {
        public List<VerzekeringModel> Verzekeringen { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}