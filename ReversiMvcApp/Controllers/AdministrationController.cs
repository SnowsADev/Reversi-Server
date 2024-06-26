using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReversiMvcApp.Models;
using ReversiMvcApp.Models.ViewModels.Administration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Speler> _userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<Speler> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        //Create
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = model.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            return View(model);
        }

        //Read
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<IdentityRole> roles = _roleManager.Roles.ToList();

            return View(roles);
        }

        //Update
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id \"{id}\" does not exist.";
                return NotFound();
            }

            EditRoleViewModel vm = new EditRoleViewModel()
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = new List<string>()
            };


            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    vm.Users.Add(user.Naam);
                }
            }

            return View(vm);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUsersInRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id \"{id}\" does not exist.";
                return NotFound();
            }

            List<UserRoleViewModel> vm = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                vm.Add(new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.Naam,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                });
            }

            ViewBag.RoleId = role.Id;
            ViewBag.RoleName = role.Name;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> models, string roleId)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id \"{roleId}\" does not exist.";
                return NotFound();
            }
            Speler speler;
            bool userIsInRole;

            foreach (var model in models)
            {
                speler = await _userManager.FindByIdAsync(model.UserId);
                userIsInRole = await _userManager.IsInRoleAsync(speler, role.Name);

                if (userIsInRole == false && model.IsSelected)
                {
                    await _userManager.AddToRoleAsync(speler, role.Name);
                }
                else if (userIsInRole)
                {
                    await _userManager.RemoveFromRoleAsync(speler, role.Name);
                }
            }

            return RedirectToAction(nameof(EditRole), new { id = role.Id });
        }
    }
}
