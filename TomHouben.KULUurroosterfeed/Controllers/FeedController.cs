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

            return File(ical, "text/calendar");
        }

        [HttpGet("error")]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        private string BitArrayToString(BitArray array)
        {
            var result = new StringBuilder(array.Length);

            for (int i = 0; i < array.Length; i ++)
            {
              result.Append(array[i] ? "1": "0");
            }
            return result.ToString();
        }

		private BitArray StringToBitArray(string array)
		{
			var result = new BitArray(array.Length);

			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == '1') result[i] = true;
			}

			return result;
		}
    }
}
