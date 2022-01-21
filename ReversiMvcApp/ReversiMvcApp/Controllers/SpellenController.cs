using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Data.ReversiDbContext;
using Reversi_CL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;

namespace ReversiMvcApp.Controllers
{
    [Authorize]
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
                .Include(spel => spel.Spelers)
                .ToListAsync();

            return View(Spellen);
        }

        // GET: Spellen/Details/5
        [EnableCors("Policy_EnableJQuery")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spel = await _context.Spellen
                .FirstOrDefaultAsync(m => m.ID == id);
            if (spel == null)
            {
                return NotFound();
            }

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
                    .Include(spel => spel.Spelers)
                    .ToListAsync();

                foreach (Spel aangemaaktSpel in spellen)
                {
                    foreach (Speler spelersVanSpel in aangemaaktSpel.Spelers)
                    {
                        if (spelersVanSpel.Equals(speler))
                            return View(spel);
                    }
                }

                spel.Spelers = new List<Speler>() { speler };
                
                _context.Add(spel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(spel);
        }

        // GET: Spellen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
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
        public async Task<IActionResult> Edit(int id, [Bind("AandeBeurt,ID,Omschrijving,Token")] Spel spel)
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spel = await _context.Spellen
                .FirstOrDefaultAsync(m => m.ID == id);
            if (spel == null)
            {
                return NotFound();
            }

            return View(spel);
        }

        // POST: Spellen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spel = await _context.Spellen.FindAsync(id);

            _context.Spellen.Remove(spel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpelExists(int id)
        {
            return _context.Spellen.Any(e => e.ID == id);
        }
    }
}
