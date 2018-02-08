using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TomHouben.KULUurroosterfeed.HTMLParserServices.Abstractions;
using TomHouben.KULUurroosterfeed.ICalService.Abstractions;
using TomHouben.KULUurroosterfeed.Models;
using TomHouben.KULUurroosterfeed.Services.Abstractions;

namespace TomHouben.KULUurroosterfeed.Controllers
{
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
        public async Task<IActionResult> SelectCourses()
        {
            var courses = await _calendarService.GetCoursesAsync();
            var coursesArray = courses.ToArray();

            var model = new SelectCoursesViewModel
            {
                Courses = coursesArray,
                SelectedCourses = new bool[coursesArray.Length]
            };

            return View("SelectCourses", model);
        }

        [HttpPost("courses")]
        public IActionResult SelectCourses([FromForm] SelectCoursesViewModel model)
        {
            if (!ModelState.IsValid) return View("SelectCourses", model);

            var selectionString = BitArrayToString(new BitArray(model.SelectedCourses));

            var url = Url.Action("GetFeed", "Feed", new { selection = selectionString }, HttpContext.Request.Scheme);

            model.GeneratedLink = url;

            return View("SelectCourses", model);
        }

        [HttpGet("calendar/{userId}")]
        public async Task<IActionResult> GetFeed(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var selectedCourses = user.SelectedCourses;

            var ical = await _calendarService.GetICalAsync(selectedCourses);

            return File(ical, "text/calendar", "uurrooster.ics");
        }

        [HttpGet("error")]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        private string BitArrayToString(BitArray array)
        {
            var byteArray = new byte[(array.Length / 8) + 1];
            array.CopyTo(byteArray, 0);

            return Convert.ToBase64String(byteArray);
        }

		private BitArray StringToBitArray(string array)
		{
            var byteArray = Convert.FromBase64String(array);
            return new BitArray(byteArray);
		}
    }
}
