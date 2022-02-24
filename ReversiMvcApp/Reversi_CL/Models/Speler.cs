using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Reversi_CL.Models
{
    public class Speler : IdentityUser
    {
        public string Naam { get; set; }
        public int AantalGewonnen { get; set; }
        public int AantalVerloren { get; set; }
        public int AantalGelijk { get; set; }

        public Kleur Kleur { get; set; } = Kleur.Geen; 

        //relations
        [ForeignKey("Spel")]
        public Spel ActueelSpel { get; set; }

        
        public Speler() 
        { 
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            Speler speler = (Speler)obj;
            return this.Id == speler.Id;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + Id.GetHashCode();

            return hash;
        }
    }
}
