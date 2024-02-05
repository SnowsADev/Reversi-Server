using ReversiMvcApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReversiMvcApp.Interfaces
{
    public interface ISpelRepository
    {
        public void AddSpel(Spel spel);
        public void AddSpelerToSpel(Spel spel, Speler speler);
        public void DeleteSpel(string spelId);
        public void EditSpel(Spel spel);
        public Task<int> EditSpelAsync(Spel spel);
        public Spel GetSpel(string spelId);
        public Spel GetOnafgerondeSpel(string spelId);
        public Spel GetOnafgerondeSpelBySpeler(Speler currentUser);
        public Spel GetOnafgerondeSpelBySpelerId(string currentUserId);
        public List<Spel> GetLopendeSpellenAsList();
        public bool SpelerIsInSpel(string spelerId);
        public bool SpelerIsInSpel(Speler speler);
        public bool SpelExists(string spelId);

    }
}
