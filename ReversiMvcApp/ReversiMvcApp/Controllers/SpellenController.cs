using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reversi_CL.Data;
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
        private readonly SpelAccessLayer _spelAccessLayer;

        public SpellenController(SpelAccessLayer DAL)
        {
            _spelAccessLayer = DAL;
        }

        // GET: Spellen
        public async Task<IActionResult> Index()
        {
            //ToDo ViewModel maken

            Speler currentUser = await _userManager.GetUserAsync(User);

            bool bSpelerInSpel = _spelAccessLayer.SpelerIsInSpel(currentUser);
            ViewData["bSpelerInSpel"] = bSpelerInSpel;

            var Spellen = _spelAccessLayer.GetLopendeSpellenAsList();

            ViewData["UserID"] = currentUser.Id;
            ViewData["UserNaam"] = currentUser.Naam;

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

            Spel spel = _spelAccessLayer.GetSpel(id);

            if (spel == null)
            {
                return NotFound();
            }

            Speler currentUser = await _userManager.GetUserAsync(User);

            if (!spel.Spelers.Any(Speler => Speler.Id == currentUser.Id))
            {
                return RedirectToAction(nameof(Index));
            }

            
            ViewData["UserID"] = currentUser.Id;

            return View(spel);
        }

        // GET: Spellen/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Speler currentUser = await _userManager.GetUserAsync(User);

            if (_spelAccessLayer.SpelerIsInSpel(currentUser))
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
                Speler currentUser = await _userManager.GetUserAsync(User);

                // User zit al in spel
                if (_spelAccessLayer.SpelerIsInSpel(currentUser))
                {
                    return View(spel);
                }

                currentUser.Kleur = Kleur.Zwart;
                await _userManager.UpdateAsync(currentUser);
                
                spel.Spelers = new List<Speler>() { currentUser };

                _spelAccessLayer.AddSpel(spel);
                
                Spel nieuwSpel = _spelAccessLayer.GetSpelWithSpeler(currentUser);

                return RedirectToAction(nameof(Details), new { id = nieuwSpel.ID });

            }
            return View(spel);
        }

        // GET: Spellen/Edit/5
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (id.Trim() == "" || id == null)
            {
                return NotFound();
            }

            var spel = _spelAccessLayer.GetSpel(id);

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
                _spelAccessLayer.EditSpel(spel);
                return RedirectToAction(nameof(Index));
            }

            return View(spel);
        }

        // GET: Spellen/Delete/5
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var spel = _spelAccessLayer.GetSpel(id);
            
            if (spel == null)
            {
                return NotFound();
            }

            return View(spel);
        }

        // POST: Spellen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _spelAccessLayer.DeleteSpel(id);

            return RedirectToAction(nameof(Index));
        }


        //ToDo: ValidationToken
        [HttpPost, ActionName("joinspel")]
        public async Task<IActionResult> JoinSpel(string Id)
        {
            Spel spel = _spelAccessLayer.GetSpel(Id);

            if (spel == null)
            {
                return NotFound();
            }

            Speler currentSpeler = await _userManager.GetUserAsync(User);

            _spelAccessLayer.AddSpelerToSpel(spel, currentSpeler);

            return RedirectToAction(nameof(Details), "Spellen", new { id = Id });
        }

    }
}
