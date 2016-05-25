using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class ObjectModel
    {
        public int? Id { get; set; }
        public string Kenmerken { get; set; }
        public ObjectTypeModel ObjectType { get; set; }

        public override string ToString()
        {
            return this.Kenmerken;
        }

    }
}