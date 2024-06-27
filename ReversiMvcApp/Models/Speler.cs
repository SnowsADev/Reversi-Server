using Microsoft.AspNetCore.Identity;
using ReversiMvcApp.Models.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReversiMvcApp.Models
{
    public class Speler : IdentityUser, IAuditable
    {
        public string Naam { get; set; } = string.Empty;
        public int AantalGewonnen { get; set; } = 0;
        public int AantalVerloren { get; set; } = 0;
        public int AantalGelijk { get; set; } = 0;
        public bool IsEnabled { get; set; } = true;

        public Kleur Kleur { get; set; } = Kleur.Geen;

        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdated { get; set; } = DateTime.UtcNow;


        //relations
        [ForeignKey("Spel")]
        public Spel ActueelSpel { get; set; }
        
        public Speler() { }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            Speler speler = (Speler)obj;
            return Id == speler.Id;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + Id.GetHashCode();

            return hash;
        }
    }
}
