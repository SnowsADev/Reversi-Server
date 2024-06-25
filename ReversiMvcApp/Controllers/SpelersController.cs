using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Interfaces;
using ReversiMvcApp.Models;
using ReversiMvcApp.Models.ViewModels.Identity;
using ReversiMvcApp.Models.ViewModels.Speler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Controllers
{

    public class SpelersController : Controller
    {
        private readonly IUserRepository _userAccessLayer;
        private readonly UserManager<Speler> _userManager;
        private readonly ILogger<Speler> _logger;
        private readonly SignInManager<Speler> _signInManager;
        private readonly IEmailSender _emailSender;

        public SpelersController(IUserRepository userAccessLayer,
            UserManager<Speler> userManager, ILogger<Speler> logger,
            SignInManager<Speler> signInManager, IEmailSender emailSender)
        {
            this._userAccessLayer = userAccessLayer;
            this._userManager = userManager;
            this._logger = logger;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
        }

        // GET: Spelers
        public async Task<IActionResult> Index()
        {
            List<Speler> users = (List<Speler>)await _userAccessLayer.GetUsersWithRole("Speler");
            //Order list
            users = users.OrderByDescending((user) =>
            {
                return (user.AantalGewonnen * 3) + user.AantalVerloren;
            }).ToList();

            if (User.IsInRole("Admin"))
            {
                users.AddRange(await _userAccessLayer.GetUsersWithRole("Mediator"));
            }

            _logger.LogInformation("User {0} accessed list of users", User.Identity.Name);

            return View(users);
        }

        // GET: Spelers/Details/5
        [Authorize(Roles = "Admin,Mediator")]
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

            _logger.LogInformation("User {0} accessed Details from user {1}", User.Identity.Name, speler.Email);
            return View(speler);
        }

        // GET: Spelers/Create
        [Authorize(Roles = "Admin,Mediator")]
        public IActionResult Create()
        {
            _logger.LogInformation("User {0} accessed create user page", User.Identity.Name);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Mediator")]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Speler { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "speler");

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // If ModelState is not valid, redisplay the registration form with validation errors
            return View(model);
        }

        // GET: Spelers/Edit/5
        [Authorize(Roles = "Admin,Mediator")]
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

            EditSpelerViewModel vm = new EditSpelerViewModel()
            {
                Id = user.Id,
                Naam = user.Naam,
                Password = "",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AantalGewonnen = user.AantalGewonnen,
                AantalVerloren = user.AantalVerloren,
                AantalGelijk = user.AantalGelijk,
            };

            _logger.LogInformation("User {0} accessed edit page for user {1}", User.Identity.Name, user.Email);

            return View(vm);
        }

        // POST: Spelers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Mediator")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Naam,Password,Email,PhoneNumber,AantalGewonnen,AantalVerloren,AantalGelijk")] EditSpelerViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Speler user = await _userAccessLayer.GetUserAsync(vm.Id);

                user.Naam = vm.Naam.Trim();
                user.PhoneNumber = vm.PhoneNumber;
                user.AantalGewonnen = vm.AantalGewonnen;
                user.AantalGelijk = vm.AantalGelijk;
                user.AantalVerloren = vm.AantalVerloren;

                if (!string.IsNullOrEmpty(vm.Password))
                {
                    PasswordHasher<Speler> passwordHasher = new PasswordHasher<Speler>();
                    user.PasswordHash = passwordHasher.HashPassword(user, vm.Password);
                }

                if (user.Email != vm.Email)
                {
                    user.Email = vm.Email;
                    user.UserName = vm.Email;
                    user.NormalizedEmail = vm.Email.ToUpper();
                    user.NormalizedUserName = vm.Email.ToUpper();
                }

                await _userAccessLayer.UpdateUserAsync(user);

                var emailSanitized = user.Email.Replace(Environment.NewLine, "");
                _logger.LogInformation("User {0} edited user {1} successfully", User.Identity.Name, emailSanitized);

                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Mediator")]
        public async Task<IActionResult> Reset2FA(string id)
        {
            var user = await _userAccessLayer.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            _logger.LogInformation("User {0} resetted 2FA for user {1}", User.Identity.Name, user.Email);
            await _userAccessLayer.Disable2FactorAuthentication(user);

            return RedirectToAction(nameof(Edit), new { id });

        }

        [HttpPost]
        [Authorize(Roles = "Admin,Mediator")]
        public async Task<IActionResult> DeactivateAccount(string id)
        {
            Speler user = await _userAccessLayer.GetUserAsync(id);

            if (user == null) return NotFound();

            _logger.LogInformation("User {0} deactivated accoutn with email {1}", User.Identity.Name, user.Email);

            await _userAccessLayer.DeleteUserAsync(user);

            return RedirectToAction(nameof(Index));
        }
    }
}
