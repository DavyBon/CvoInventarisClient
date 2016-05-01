using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class FactuurViewModel
    {
        public List<FactuurModel> Factuur { get; set; }
        public List<LeverancierModel> Leverancier { get; set; }
    }
}