using Microsoft.AspNetCore.Mvc;

namespace TomHouben.KULUurroosterfeed.Controllers
{
    [Route("")]
    public class Home : Controller
    {
        // GET
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "User");
            else
                return RedirectToAction("Login", "Account");
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}