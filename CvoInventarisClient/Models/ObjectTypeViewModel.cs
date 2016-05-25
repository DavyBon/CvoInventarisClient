using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class ObjectTypeViewModel : ICloneable
    {
        public List<ObjectTypeModel>objectTypes { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}