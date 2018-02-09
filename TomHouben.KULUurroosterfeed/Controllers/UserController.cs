using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TomHouben.KULUurroosterfeed.Models;
using TomHouben.KULUurroosterfeed.Services.Abstractions;

namespace TomHouben.KULUurroosterfeed.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly UserManager<TimeTableUser> _userManager;
        private readonly ITimeTableEntryService _timeTableEntryService;

        public UserController(UserManager<TimeTableUser> userManager, ITimeTableEntryService timeTableEntryService)
        {
            _userManager = userManager;
            _timeTableEntryService = timeTableEntryService;
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

        [HttpGet("courses")]
        [Authorize]
        public async Task<IActionResult> SelectCourses()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Error", "Home");

            var allCourses = await _timeTableEntryService.GetCoursesAsync();
            var selectedCourses = user.SelectedCourses;

            var courseSelections = allCourses.Select(x => new CourseSelectionViewModel(x, selectedCourses.Contains(x))).ToList();

            var model = new SelectCoursesViewModel
            {
                SelectedCourses = courseSelections
            };

            return View("SelectCourses", model);
        }

        [HttpPost("courses")]
        [Authorize]
        public async Task<IActionResult> SelectCourses([FromForm] SelectCoursesViewModel model)
        {
            if (!ModelState.IsValid) return View("SelectCourses", model);
            
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Error", "Home");

            var selectedCourses = model.SelectedCourses.Where(x => x.Selected).Select(x => x.Name).ToList();

            user.SelectedCourses = selectedCourses;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index", "User");
        }
    }
}