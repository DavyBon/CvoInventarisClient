using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Models
{
    public class ObjectViewModel
    {
        public List<ObjectModel> Objecten { get; set; }
        public List<SelectListItem> Facturen { get; set; }
        public List<SelectListItem> ObjectTypes { get; set; }
    }
}