﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Interfaces;
using ReversiMvcApp.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ReversiMvcApp.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<Speler> _userManager;
        private readonly SignInManager<Speler> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly IUserRepository _userAccessLayer;

        public DeletePersonalDataModel(
            UserManager<Speler> userManager,
            SignInManager<Speler> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IUserRepository userAccessLayer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _userAccessLayer = userAccessLayer;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            int result = await _userAccessLayer.DeleteUserAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);

            //No changes made in the database
            if (result == 0)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
