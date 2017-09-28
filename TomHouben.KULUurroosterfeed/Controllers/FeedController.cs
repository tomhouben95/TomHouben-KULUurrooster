using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public FeedController(
            ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpGet("")]
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

        [HttpPost("")]
        public IActionResult SelectCourses([FromForm] SelectCoursesViewModel model)
        {
            if (!ModelState.IsValid) return View("SelectCourses", model);

            var selectionString = BitArrayToString(new BitArray(model.SelectedCourses));

            var url = Url.Action("GetFeed", "Feed", new { selection = selectionString }, HttpContext.Request.Scheme);

            model.GeneratedLink = url;

            return View("SelectCourses", model);
        }

        [HttpGet("calendar/{selection}")]
        public async Task<IActionResult> GetFeed(string selection)
        {
            var selectedArray = StringToBitArray(selection);

            var ical = await _calendarService.GetICalAsync(selectedArray);

            return File(ical, "text/calendar", "uurrooster");
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
