using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TomHouben.KULUurroosterfeed.Models;

namespace TomHouben.KULUurroosterfeed.Controllers
{
    [Route("account")]
    public class AccountController: Controller
    {
        
        private readonly ILogger _logger;
        private readonly SignInManager<TimeTableUser> _signInManager;
        private readonly UserManager<TimeTableUser> _userManager;

        public AccountController(
            SignInManager<TimeTableUser> signInManager, 
            UserManager<TimeTableUser> userManager,
            ILogger<AccountController> logger)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("Login");
        }
         
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("facebook-login")]
        [AllowAnonymous]
        public IActionResult FacebookLogin()
        {
            var properties =
                _signInManager.ConfigureExternalAuthenticationProperties(
                    "Facebook",
                    Url.Action("ExternalLoginCallback", "Account"));

            return Challenge(properties, "Facebook");
        }
        
        [HttpGet("google-login")]
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            var properties =
                _signInManager.ConfigureExternalAuthenticationProperties(
                    "Google",
                    Url.Action("ExternalLoginCallback", "Account"));

            return Challenge(properties, "Google");
        }

        [HttpGet("external-login-callback")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
                return RedirectToAction("Index", "Home");

            var result =
                await _signInManager.ExternalLoginSignInAsync(
                    info.LoginProvider, 
                    info.ProviderKey,
                    isPersistent: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToAction("Index", "User");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new TimeTableUser(email);

                var createUserResult = await _userManager.CreateAsync(user);

                if (!createUserResult.Succeeded)
                {
                    _logger.LogError("Error creating new user: {Email}", email);
                    return RedirectToAction("Error", "Home");
                }

                _logger.LogInformation("Created new user: {Email}", email);
            }
               
            var addLoginResult = await _userManager.AddLoginAsync(user, info);

            if (!addLoginResult.Succeeded)
            {
                _logger.LogError("Error adding login with {Name} provider", info.LoginProvider);
                return RedirectToAction("Error", "Home");
            }
            
            await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, 
                info.ProviderKey,
                isPersistent: false);

            _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);

            return RedirectToAction("Index", "User");
        }
        

        [HttpGet("remove-account")]
        [Authorize]
        public async Task<IActionResult> RemoveAccount()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                _logger.LogError("Tried to remove non-existing user");
                return RedirectToAction("Error", "Home");
            }
            await _userManager.DeleteAsync(user);

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("access-denied")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("Error");
        }
    }
}