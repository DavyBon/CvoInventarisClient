using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class ObjectModel
    {
        public int Id { get; set; }
        public FactuurModel Factuur { get; set; }
        public LeverancierModel Leverancier { get; set; }
        public ObjectTypeModel ObjectType { get; set; }
        public string Kenmerken { get; set; }
    }
}