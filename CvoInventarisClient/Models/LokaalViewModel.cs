using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class LokaalViewModel
    {
        public List<LokaalModel> Lokaal { get; set; }
        public List<NetwerkModel> Netwerk { get; set; }
    }
}