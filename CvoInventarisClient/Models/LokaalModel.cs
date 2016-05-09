using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class LokaalModel
    {

        public int IdLokaal { get; set; }

        // For StringLength, Datatype etc check out database
         
        // Required: field has to be filled in
        // ErrorMessage: displays an errormessage when the (entered) value does not match the requirements
        // StringLenght: maximum length = 50 characters, minimumlength = 2 (example: A1, B1,...)
        // Display: display name used in views (example: @Html.DisplayNameFor...) | Request.Form["lokaalNaam"] gets it's data looking at the name attribute in the views (example: name="lokaalNaam")
        [Required(ErrorMessage = "Vul een lokaalnaam in")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Een lokaalnaam bestaat uit minimum 2 en maximum 50 characters")]
        [Display(Name = "lokaalNaam")]
        public string LokaalNaam { get; set; }

        // Range: minimum and maximum range of the integer entered.
        [Required(ErrorMessage = "Vul een aantal plaatsen in")]
        [Range(1, 100, ErrorMessage = "Het aantal plaatsen moet tussen 1 en 100 liggen")]
        [Display(Name = "aantalPlaatsen")]
        public int AantalPlaatsen { get; set; }


        [Display(Name = "isComputerLokaal")]
        public bool IsComputerLokaal { get; set; }

        [Display(Name = "Netwerk")]
        public NetwerkModel Netwerk { get; set; }
    }
}