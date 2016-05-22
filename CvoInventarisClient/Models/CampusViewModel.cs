using CvoInventarisClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class CampusViewModel : ICloneable
    {
        public List<CampusModel> campussen { get; set;}
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}