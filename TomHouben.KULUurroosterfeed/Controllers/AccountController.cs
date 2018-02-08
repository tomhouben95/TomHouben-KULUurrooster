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
            return NotFound();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
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
                return RedirectToAction("Login");

            var result =
                await _signInManager.ExternalLoginSignInAsync(
                    info.LoginProvider, 
                    info.ProviderKey,
                    isPersistent: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var newUser = new TimeTableUser(email);
                var createUserResult = await _userManager.CreateAsync(newUser);

                if (!createUserResult.Succeeded)
                    return RedirectToAction("Error");

                var addLoginResult = await _userManager.AddLoginAsync(newUser, info);

                if (!addLoginResult.Succeeded)
                    return RedirectToAction("Error");
                _logger.LogInformation("User created in with {Name} provider.", info.LoginProvider);
                
                await _signInManager.ExternalLoginSignInAsync(
                        info.LoginProvider, 
                        info.ProviderKey,
                        isPersistent: false);
            }
           
            
            

            return RedirectToAction("SelectCourses", "Feed");
        }

        [HttpPost("remove-account")]
        [Authorize]
        public async Task<IActionResult> RemoveAccount()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Error");

            await _userManager.DeleteAsync(user);

            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
        
        [HttpGet("error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return NotFound();
        }
    }
}