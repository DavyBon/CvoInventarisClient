using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class TicketModel
    {
        public int IdTicket { get; set; }
        public string Verzender { get; set; }
        public string Ontvanger { get; set; }
        public DateTime Tijdstip { get; set; }
        public string Bericht { get; set; }
        public string Status { get; set; }
    }
}