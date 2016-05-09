using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CvoInventarisClient.Models
{
    public class HardwareViewModel
    {
        public List<HardwareModel> Hardwares { get; set; }
        public List<SelectListItem> Cpus { get; set; }
        public List<SelectListItem> Devices { get; set; }
        public List<SelectListItem> GrafischeKaarten { get; set; }
        public List<SelectListItem> Harddisks { get; set; }
    }
}