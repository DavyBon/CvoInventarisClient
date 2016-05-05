using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Models
{
    public class LokaalViewModel
    {
        public List<LokaalModel> Lokaal { get; set; }
        public List<SelectListItem> Netwerk { get; set; }
    }
}