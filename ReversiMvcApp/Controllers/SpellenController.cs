using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ReversiMvcApp.Interfaces;
using ReversiMvcApp.Models;
using ReversiMvcApp.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Controllers
{
    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class SpellenController : Controller
    {
        private readonly ISpelRepository _spelAccessLayer;
        private readonly IUserRepository _userAccessLayer;
        private readonly IHubContext<SpelHub> _hubContext;

        public SpellenController(ISpelRepository spelAccessLayer, IUserRepository userAccessLayer, IHubContext<SpelHub> hubContext)
        {
            _spelAccessLayer = spelAccessLayer;
            _userAccessLayer = userAccessLayer;
            _hubContext = hubContext;
        }

        // GET: Spellen
        public async Task<IActionResult> Index()
        {
            //ToDo ViewModel maken
            Speler currentUser = await _userAccessLayer.GetUserAsync(User);

            bool bSpelerInSpel = _spelAccessLayer.SpelerIsInSpel(currentUser);
            ViewData["bSpelerInSpel"] = bSpelerInSpel;
            if (bSpelerInSpel)
            {
                Spel spel = _spelAccessLayer.GetOnafgerondeSpelBySpeler(currentUser);
                ViewData["currentSpelId"] = spel.ID;
            }

            var Spellen = _spelAccessLayer.GetLopendeSpellenAsList();

            ViewData["UserID"] = currentUser.Id;
            ViewData["UserNaam"] = currentUser.Naam;


            return View(Spellen);
        }

        // GET: Spellen/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id.Trim() == "" || id == null)
            {
                return NotFound();
            }

            Spel spel = _spelAccessLayer.GetOnafgerondeSpel(id);

            if (spel == null)
            {
                return RedirectToAction(nameof(Index));
            }

            Speler currentUser = await _userAccessLayer.GetUserAsync(User);

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

            Speler currentUser = await _userAccessLayer.GetUserAsync(User);

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
                Speler currentUser = await _userAccessLayer.GetUserAsync(User);

                // User zit al in spel
                if (_spelAccessLayer.SpelerIsInSpel(currentUser))
                {
                    return View(spel);
                }

                currentUser.Kleur = Kleur.Zwart;
                await _userAccessLayer.UpdateUserAsync(currentUser);

                spel.Spelers = new List<Speler>() { currentUser };

                _spelAccessLayer.AddSpel(spel);

                Spel nieuwSpel = _spelAccessLayer.GetOnafgerondeSpelBySpeler(currentUser);

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

            var spel = _spelAccessLayer.GetOnafgerondeSpel(id);

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
        public IActionResult Edit(string id, [Bind("AandeBeurt,ID,Omschrijving,Token")] Spel spel)
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
            var spel = _spelAccessLayer.GetOnafgerondeSpel(id);

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
        [HttpPost, ActionName("Joinspel")]
        public async Task<IActionResult> JoinSpel(string Id)
        {
            Spel spel = _spelAccessLayer.GetOnafgerondeSpel(Id);

            if (spel == null)
            {
                return NotFound();
            }

            Speler currentSpeler = await _userAccessLayer.GetUserAsync(User);

            _spelAccessLayer.AddSpelerToSpel(spel, currentSpeler);

            return Ok();
        }

        [HttpPost, ActionName("GeefOp")]
        public async Task<IActionResult> GeefOp(string Id)
        {
            Spel spel = _spelAccessLayer.GetOnafgerondeSpel(Id);
            Speler geeftOpSpeler = await _userAccessLayer.GetUserAsync(User);
            Speler winnaarSpeler = null;

            if (spel == null || geeftOpSpeler == null)
            {
                return NotFound();
            }

            if (spel.Spelers.Count() == 2)
            {
                winnaarSpeler = spel.Spelers.FirstOrDefault(s => s.Id != geeftOpSpeler.Id);
                winnaarSpeler.AantalGewonnen += 1;
                geeftOpSpeler.AantalVerloren += 1;
            }

            spel.SpelIsAfgelopen = true;

            _spelAccessLayer.EditSpel(spel);

            await _hubContext.Clients.Users(winnaarSpeler?.Id).SendAsync("ReceiveRefreshGameNotification");

            return RedirectToAction(nameof(Index));
        }

    }
}
