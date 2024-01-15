using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Interfaces;
using ReversiMvcApp.Models;

namespace ReversiMvcApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Speler> _signInManager;
        private readonly ISpelRepository _spelAccessLayer;
        private readonly IUserRepository _userAccessLayer;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<Speler> signInManager, 
            ISpelRepository spelAccessLayer,
            IUserRepository userAccessLayer,
            ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _spelAccessLayer = spelAccessLayer;
            _userAccessLayer = userAccessLayer;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            string userId = _userAccessLayer.GetUserId(User);
            Spel spel = _spelAccessLayer.GetOnafgerondeSpelBySpelerId(userId);

            if (spel != null)
            {
                spel.SpelIsAfgelopen = true;
                foreach (Speler speler in spel.Spelers)
                {
                    speler.ActueelSpel = null;
                    _userAccessLayer.UpdateUser(speler);
                }

                spel.Spelers.Clear();
                await _spelAccessLayer.EditSpelAsync(spel);
            }

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
