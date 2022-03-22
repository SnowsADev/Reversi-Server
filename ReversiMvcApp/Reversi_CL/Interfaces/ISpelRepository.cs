using Reversi_CL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi_CL.Interfaces
{
    public interface ISpelRepository
    {
        public void AddSpel(Spel spel);
        public void AddSpelerToSpel(Spel spel, Speler speler);
        public void DeleteSpel(string spelId);
        public void EditSpel(Spel spel);
        public Spel GetSpel(string spelId);
        public Spel GetSpelWithSpeler(Speler currentUser);
        public Spel GetSpelWithSpelerId(string currentUserId);
        public List<Spel> GetLopendeSpellenAsList();
        public bool SpelerIsInSpel(string spelerId);
        public bool SpelerIsInSpel(Speler speler);
        public bool SpelExists(string spelId);
    }
}
