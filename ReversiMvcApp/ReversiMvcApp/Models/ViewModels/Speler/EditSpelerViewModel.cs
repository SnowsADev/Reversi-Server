using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Models.ViewModels.Speler
{
    public class EditSpelerViewModel
    {
        public string Id { get; set; }
        public string Naam { get; set; }
        [Range(8, 48)]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Range(0, int.MaxValue)]
        public int AantalGewonnen { get; set; }
        [Range(0, int.MaxValue)]
        public int AantalGelijk { get; set; }
        [Range(0, int.MaxValue)]
        public int AantalVerloren { get; set; }
    }
}
