using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class InventarisModel
    {
        public int Id { get; set; }
        public LokaalModel Lokaal { get; set; }
        public ObjectModel Object { get; set; }
        public VerzekeringModel Verzekering { get; set; }
        public bool IsAanwezig { get; set; }
        public bool IsActief { get; set; }
        public string Label { get; set; }
    }
}