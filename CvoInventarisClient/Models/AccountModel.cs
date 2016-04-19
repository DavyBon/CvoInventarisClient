using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class AccountModel
    {
        public int IdAccount { get; set; }
        public string Type { get; set; }
        public string Gebruikersnaam { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Telefoonnummer { get; set; }
        public string Email { get; set; }
        public string Wachtwoord { get; set; }
    }
}