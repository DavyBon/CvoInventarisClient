using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class ObjectTypeModel
    {
        public int? Id { get; set; }
        public string Omschrijving { get; set; }

        public override string ToString()
        {
            return this.Omschrijving;
        }
    }
}