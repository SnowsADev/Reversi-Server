using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reversi_CL.Interfaces;
using Reversi_CL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi_CL.Data
{
    public class SpelAccessLayer : ISpelRepository
    {
        private readonly ReversiDbContext.ReversiDbContext _reversiContext;
        private readonly UserManager<Speler> _userManager;

        public SpelAccessLayer(ReversiDbContext.ReversiDbContext reversiContext, 
                            ReversiDbIdentityContext.ReversiDbIdentityContext reversiIdentityContext,
                            UserManager<Speler> userManager)
        {
            this._reversiContext = reversiContext;
            this._userManager = userManager;
        }

        public void AddSpelerToSpel(Spel spel, Speler speler)
        {
            if (spel.Spelers == null)
                throw new NullReferenceException("Dit spel heeft nog geen Collection! Ben je een include vergeten?");

            if (spel.Spelers.Count() >= 2)
                throw new Exception("Het spel zit al vol!");

            //update Spel
            spel.Spelers.Add(speler);

            EditSpel(spel);

            //update speler
            speler.Kleur = Kleur.Wit;
            _userManager.UpdateAsync(speler);
        }

        /// <summary>
        /// Checks if Spel Exists
        /// </summary>
        /// <param name="spelId"></param>
        /// <returns></returns>
        public bool SpelExists(string spelId)
        {
            return _reversiContext.Spellen.Any(e => e.ID == spelId);
        }

        /// <summary>
        /// Edits spel
        /// </summary>
        /// <param name="spel"></param>
        public void EditSpel(Spel spel)
        {
            _reversiContext.Update<Spel>(spel);
            _reversiContext.SaveChanges();
        }

        /// <summary>
        /// Deletes spel
        /// </summary>
        /// <param name="spelId"></param>
        public void DeleteSpel(string spelId)
        {
            Spel spel = _reversiContext.Spellen.Find(spelId);
            _reversiContext.Remove(spel);
            _reversiContext.SaveChanges();
        }

        /// <summary>
        /// Creates a new Spel
        /// </summary>
        /// <param name="spel"></param>
        public void AddSpel(Spel spel)
        {
            _reversiContext.Add(spel);
            _reversiContext.SaveChanges();
        }

        /// <summary>
        /// Returns a Spel
        /// </summary>
        /// <param name="spelId"></param>
        /// <returns></returns>
        public Spel GetSpel(string spelId)
        {
            Spel result = _reversiContext.Spellen
                .Where(spel => spel.SpelIsAfgelopen == false)
                .Include(spel => spel.Spelers)
                .FirstOrDefault(m => m.ID == spelId);

            return result;
        }

        /// <summary>
        /// Returns the active game from speler
        /// </summary>
        /// <param name="speler"></param>
        /// <returns></returns>
        public Spel GetSpelWithSpeler(Speler currentUser)
        {
            Spel result = _reversiContext.Spellen
                .AsNoTracking()
                .Where(spel => !spel.SpelIsAfgelopen)
                .Include(spel => spel.Spelers)
                .FirstOrDefault(spel => spel.Spelers
                    .Any(speler => speler.Id == currentUser.Id)
                );

            return result;
        }

        /// <summary>
        /// Returns the active game from spelerId
        /// </summary>
        /// <param name="speler"></param>
        /// <returns></returns>
        public Spel GetSpelWithSpelerId(string currentUserId)
        {
            Spel result = _reversiContext.Spellen
                .AsNoTracking()
                .Where(spel => !spel.SpelIsAfgelopen)
                .Include(spel => spel.Spelers)
                .FirstOrDefault(spel => spel.Spelers
                    .Any(speler => speler.Id == currentUserId)
                );

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Spel> GetLopendeSpellenAsList()
        {
            List<Spel> result = _reversiContext.Spellen
                    .Where(spel => !spel.SpelIsAfgelopen)
                    .Include(spel => spel.Spelers)
                    .ToList();

            return result;
        }

        public bool SpelerIsInSpel(string SpelerId)
        {
            // Checken of de speler niet al in een spel zit.
            List<Spel> spellen = _reversiContext.Spellen
                .AsNoTracking()
                .Where(spel => spel.SpelIsAfgelopen == false)
                .Include(spel => spel.Spelers)
                .ToList();

            foreach (Spel aangemaaktSpel in spellen)
            {
                foreach (Speler spelersVanSpel in aangemaaktSpel.Spelers)
                {
                    if (spelersVanSpel.Id.Equals(SpelerId)) return true;
                }
            }

            return false;
        }

        public bool SpelerIsInSpel(Speler speler)
        {
            return SpelerIsInSpel(speler.Id);
        }

    }
}
