using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reversi_CL.Data.ReversiDbContext;
using Reversi_CL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReversiMvcApp.Controllers
{
    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class SpellenController : Controller
    {
        private readonly ReversiDbContext _context;
        private readonly UserManager<Speler> _userManager;
        
        public SpellenController(ReversiDbContext context, UserManager<Speler> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private bool SpelerIsInSpel()
        {
            ClaimsPrincipal currentUser = this.User;
            string currentUserId = _userManager.GetUserId(currentUser);

            // Checken of de speler niet al in een spel zit.
            List<Spel> spellen = _context.Spellen
                .AsNoTracking()
                .Where(spel => spel.SpelIsAfgelopen == false)
                .Include(spel => spel.Spelers)
                .ToList();

            foreach (Spel aangemaaktSpel in spellen)
            {
                foreach (Speler spelersVanSpel in aangemaaktSpel.Spelers)
                {
                    if (spelersVanSpel.Id.Equals(currentUserId))
                        return true;
                }
            }

            return false;
        }

        // GET: Spellen
        public async Task<IActionResult> Index()
        {
            bool bSpelerInSpel = SpelerIsInSpel();
            ViewData["bSpelerInSpel"] = bSpelerInSpel;

            var Spellen = await _context.Spellen
                .Where(spel => !spel.SpelIsAfgelopen)
                .Include(spel => spel.Spelers)
                .ToListAsync();

            Speler speler = await _userManager.GetUserAsync(User);
            ViewData["UserID"] = speler.Id;
            ViewData["UserNaam"] = speler.Naam;

            return View(Spellen);
        }

        // GET: Spellen/Details/5
        [EnableCors("Policy_EnableJQuery")]
        public async Task<IActionResult> Details(string id)
        {
            if (id.Trim() == "" || id == null)
            {
                return NotFound();
            }

            var spel = await _context.Spellen
                .Where(spel => !spel.SpelIsAfgelopen)
                .Include(spel => spel.Spelers)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (spel == null)
            {
                return NotFound();
            }

            Speler currentSpeler = await _userManager.GetUserAsync(User);

            if (!spel.Spelers.Any(Speler => Speler.Id == currentSpeler.Id))
            {
                return RedirectToAction(nameof(Index));
            }

            
            ViewData["UserID"] = currentSpeler.Id;

            return View(spel);
        }

        // GET: Spellen/Create
        public IActionResult Create()
        {
            if (SpelerIsInSpel())
            {
                RedirectToAction(nameof(Index));
            }

            return View();
        }

        // POST: Spellen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Omschrijving")] Spel spel)
        {
            if (ModelState.IsValid)
            {
                Speler speler = await _userManager.GetUserAsync(User);

                // Checken of de speler niet al in een spel zit.
                List<Spel> spellen = await _context.Spellen
                    .AsNoTracking()
                    .Where(spel => !spel.SpelIsAfgelopen)
                    .Include(spel => spel.Spelers)
                    .ToListAsync();

                //if (speler.actueelSpelId == null || speler.actueelSpelId.Trim() == "") return RedirectToAction(nameof(Index));

                foreach (Spel aangemaaktSpel in spellen)
                {
                    foreach (Speler spelersVanSpel in aangemaaktSpel.Spelers)
                    {
                        if (spelersVanSpel.Equals(speler))
                            return View(spel);
                    }
                }

                speler.Kleur = Kleur.Zwart;
                await _userManager.UpdateAsync(speler);
                
                spel.Spelers = new List<Speler>() { speler };

                _context.Add(spel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), "Spellen", new { id = speler.Id });

            }
            return View(spel);
        }

        // GET: Spellen/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id.Trim() == "" || id == null)
            {
                return NotFound();
            }

            var spel = await _context.Spellen.FindAsync(id);

            if (spel == null)
            {
                return NotFound();
            }
            return View(spel);
        }

        // POST: Spellen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AandeBeurt,ID,Omschrijving,Token")] Spel spel)
        {
            if (id != spel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpelExists(spel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(spel);
        }

        // GET: Spellen/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            //if (id.Trim() == "" || id == null)
            //{
            //    return NotFound();
            //}

            var spel = await _context.Spellen.FirstOrDefaultAsync(m => m.ID == id);
            
            if (spel == null)
            {
                return NotFound();
            }

            return View(spel);
        }

        // POST: Spellen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var spel = await _context.Spellen.FindAsync(id);
            Speler speler = await _userManager.GetUserAsync(User);
            
            await _context.SaveChangesAsync();

            _context.Spellen.Remove(spel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpelExists(string id)
        {
            return _context.Spellen.Any(e => e.ID == id);
        }

        //ToDo: ValidationToken
        [HttpPost, ActionName("joinspel")]
        public async Task<IActionResult> JoinSpel(string Id)
        {
            Spel spel = await _context.Spellen
                .Where(spel => !spel.SpelIsAfgelopen)
                .Include(x => x.Spelers)
                .FirstOrDefaultAsync(x => x.ID == Id);

            if (spel == null)
            {
                return NotFound();
            }

            Speler currentSpeler = await _userManager.GetUserAsync(User);
            try
            {
                AddSpelerToSpel(spel, currentSpeler);
            }
            catch
            {
                return BadRequest();
            }

            _context.Spellen.Update(spel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), "Spellen", new { id = Id });
        }

        /// <summary>
        /// Adds Speler to Spel
        /// </summary>
        /// <param name="spel">Het spel waarbij de speler aan toegevoegd wordt</param>
        /// <param name="speler">De speler dat toegevoegt moet worden</param>
        private void AddSpelerToSpel(Spel spel, Speler speler)
        {
            if (spel.Spelers == null)
                throw new NullReferenceException("Dit spel heeft nog geen Collection! Ben je een include vergeten?");

            if (spel.Spelers.Count() >= 2)
                throw new Exception("Het spel zit al vol!");

            //update Spel
            spel.Spelers.Add(speler);

            _context.Spellen.Update(spel);
            _context.SaveChanges();

            //update speler
            speler.Kleur = Kleur.Wit;
            _userManager.UpdateAsync(speler);
        }
    }
}
