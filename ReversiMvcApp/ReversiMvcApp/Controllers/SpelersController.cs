using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reversi_CL.Data;
using Reversi_CL.Data.ReversiDbIdentityContext;
using Reversi_CL.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Controllers
{
    [Authorize(Roles = "Admin,Mediator")]
    public class SpelersController : Controller
    {
        private readonly UserAccessLayer _userAccessLayer;

        public SpelersController(UserAccessLayer userAccessLayer)
        {
            this._userAccessLayer = userAccessLayer;
        }

        // GET: Spelers
        public IActionResult Index()
        {
            return View(_userAccessLayer.GetUsersAsList());
        }

        // GET: Spelers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Speler speler = await _userAccessLayer.GetUserAsync(id);

            if (speler == null)
            {
                return NotFound();
            }

            return View(speler);
        }

        // GET: Spelers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Spelers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam,AantalGewonnen,AantalVerloren,AantalGelijk")] Speler speler)
        {
            if (ModelState.IsValid)
            {
                await _userAccessLayer.CreateUserAsync(speler);
                return RedirectToAction(nameof(Index));
            }

            return View(speler);
        }

        // GET: Spelers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Speler user = await _userAccessLayer.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Spelers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Naam,AantalGewonnen,AantalVerloren,AantalGelijk")] Speler speler)
        {
            if (id != speler.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _userAccessLayer.UpdateUserAsync(speler);
                
                return RedirectToAction(nameof(Index));
            }
            return View(speler);
        }

        // GET: Spelers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            Speler speler = await _userAccessLayer.GetUserAsync(id);

            if (speler == null) return NotFound();

            return View(speler);
        }

        // POST: Spelers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Speler speler = await _userAccessLayer.GetUserAsync(id);

            if (speler == null) return NotFound();

            await _userAccessLayer.DeleteUserAsync(speler);
            return RedirectToAction(nameof(Index));
        }
    }
}
