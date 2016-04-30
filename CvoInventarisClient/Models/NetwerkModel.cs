using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class NetwerkModel
    {
        public int Id { get; set; }
        public string Driver { get; set; }
        [Required]
        [MaxLength(2,ErrorMessage ="test")]
        public string Merk { get; set; }
        public string  Snelheid { get; set; }
        public string Type { get; set; }
    }
}