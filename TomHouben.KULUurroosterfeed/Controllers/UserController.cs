using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TomHouben.KULUurroosterfeed.Models;

namespace TomHouben.KULUurroosterfeed.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly UserManager<TimeTableUser> _userManager;

        public UserController(UserManager<TimeTableUser> userManager)
        {
            _userManager = userManager;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var feedUrl = Url.Action("GetFeed", "Feed", new {userId = user.Id}, HttpContext.Request.Scheme);
 
            var model = new UserViewModel(feedUrl, user.SelectedCourses);

            return View("Index", model);
        }
    }
}