using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class SessionModel
    {
        public int IdSession { get; set; }
        public int IdAccount { get; set; }
        public string Device { get; set; }
        public DateTime Tijdstip { get; set; }
    }
}