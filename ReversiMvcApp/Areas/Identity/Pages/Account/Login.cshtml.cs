using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Models;
using ReversiMvcApp.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ReversiMvcApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<Speler> _userManager;
        private readonly IReCaptchaValidatorService reCaptchaValidator;
        private readonly SignInManager<Speler> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<Speler> signInManager,
            ILogger<LoginModel> logger,
            UserManager<Speler> userManager,
            IReCaptchaValidatorService reCaptchaValidator)
        {
            _userManager = userManager;
            this.reCaptchaValidator = reCaptchaValidator;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [ValidateReCaptcha]
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            string recaptchaResponse = Request.Form["g-recaptcha-response"];
            if (!string.IsNullOrEmpty(recaptchaResponse))
            {
                var validationResult = await reCaptchaValidator.TryValidateReCaptchaResponse(HttpContext, recaptchaResponse);

                if (!validationResult.isValid)
                {
                    ModelState.AddModelError(string.Empty, validationResult.errorMessage);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please don't forget to fill in the reCaptcha");
            }

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                SignInResult result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                var userId = await _userManager.FindByEmailAsync(Input.Email);

                if (result.Succeeded)
                {
                    
                    if (!(await _userManager.FindByEmailAsync(Input.Email)).IsEnabled)
                    {
                        _logger.LogWarning($"User tried logging into an account that has been disabled. (id: {userId}");
                        ModelState.AddModelError(string.Empty, "This account has been disabled.");
                        return Page();
                    }

                    _logger.LogInformation($"User logged in succesfully. (id: {userId}");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    _logger.LogTrace($"User tried loggin in but requires 2FA. (id: {userId}");
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning($"User account locked out. (id: {userId})");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    _logger.LogTrace($"User tried logging into account but failed.");
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
