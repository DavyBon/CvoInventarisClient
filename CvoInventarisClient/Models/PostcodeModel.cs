using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class PostcodeModel
    {
        public int Id { get; set; }
        public string Postcode { get; set; }
        public string Gemeente { get; set; }
    }
}