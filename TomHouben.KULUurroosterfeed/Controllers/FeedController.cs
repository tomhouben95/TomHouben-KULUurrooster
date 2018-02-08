using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TomHouben.KULUurroosterfeed.HTMLParserServices.Abstractions;
using TomHouben.KULUurroosterfeed.ICalService.Abstractions;
using TomHouben.KULUurroosterfeed.Models;
using TomHouben.KULUurroosterfeed.Services.Abstractions;

namespace TomHouben.KULUurroosterfeed.Controllers
{
    [Route("feed")]
    public class FeedController: Controller
    {
        private readonly ICalendarService _calendarService;
        private readonly UserManager<TimeTableUser> _userManager;

        public FeedController(
            ICalendarService calendarService,
            UserManager<TimeTableUser> userManager)
        {
            _calendarService = calendarService;
            _userManager = userManager;
        }

        [HttpGet("courses")]
        [Authorize]
        public async Task<IActionResult> SelectCourses()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                throw new NotImplementedException();//TODO: Error page

            var allCourses = await _calendarService.GetCoursesAsync();
            var selectedCourses = user.SelectedCourses;

            var courseSelections = allCourses.Select(x => new CourseSelection(x, selectedCourses.Contains(x))).ToList();

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
                return Error();

            var selectedCourses = model.SelectedCourses.Where(x => x.Selected).Select(x => x.Name).ToList();

            user.SelectedCourses = selectedCourses;

            await _userManager.UpdateAsync(user);
            
            return View("SelectCourses", model);
        }

        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeed(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Error();

            var selectedCourses = user.SelectedCourses;

            var ical = await _calendarService.GetICalAsync(selectedCourses);

            return File(ical, "text/calendar", "uurrooster.ics");
        }

        [HttpGet("error")]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
    }
}
